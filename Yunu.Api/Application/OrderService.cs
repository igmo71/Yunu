using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Yunu.Api.Common;
using Yunu.Api.Domain;
using Yunu.Api.Infrastructure.Data;

namespace Yunu.Api.Application
{
    [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
    public record OrderListParameters(int page, int perPage, Dictionary<string, string>? order, int? scopeId);

    public interface IOrderService
    {
        Task<int> LoadOrderListAsync();
        Task<int> ClearOrderListAsync();
    }

    public class OrderService(
        IYunuClient yunuClient,
        //AppDbContext dbContext,
        IDbContextFactory<AppDbContext> dbContextFactory,
        IOptions<YunuConfig> options,
        ILogger<OrderService> logger,
        IConfiguration configuration
        ) : IOrderService
    {
        private readonly IYunuClient _yunuClient = yunuClient;
        //private readonly AppDbContext _dbContext = dbContext;
        private readonly IDbContextFactory<AppDbContext> _dbContextFactory = dbContextFactory;
        private readonly YunuConfig _yunuConfig = options.Value;
        private readonly ILogger<OrderService> _logger = logger;
        private readonly IConfiguration _configuration = configuration;

        public async Task<int> ClearOrderListAsync()
        {
            using var dbContext = _dbContextFactory.CreateDbContext();

            var result = await dbContext.Order.ExecuteDeleteAsync();

            return result;
        }

        public async Task<int> LoadOrderListAsync()
        {
            var source = nameof(LoadOrderListAsync);

            var deleted = await ClearOrderListAsync();
            _logger.LogInformation("{Source }Deleted: {Deleted}", nameof(LoadOrderListAsync), deleted);

            var startingTimestamp = Stopwatch.GetTimestamp();
            int result = 0;
            int page = 1;
            var orderList = new OrderList();
            do
            {
                //var uri = $"{AppRouting.Prefix}{AppRouting.OrderListUri}?page={page}&perPage={_yunuConfig.DefaultPerPage}";
                var uri = $"{AppRouting.Prefix}{AppRouting.OrderListUri}?page={page}&perPage={_yunuConfig.DefaultPerPage}" +
                    $"&order[by]=addedDate&order[direction]=desc&scopeId={_yunuConfig.ScopeId}";

                var pageRequestTimestamp = Stopwatch.GetTimestamp();
                orderList = await _yunuClient.GetAsync<OrderList>(uri);
                _logger.LogInformation("{Source} Request in {Elapsed}", source, Stopwatch.GetElapsedTime(pageRequestTimestamp));

                if (orderList?.list is null || orderList.list.Count == 0)
                {
                    _logger.LogWarning("{Source} Order List is Empty. Total: {Total}", source, result);
                    return result;
                }

                var dbSavingTimestamp = Stopwatch.GetTimestamp();
                result += await BulkInsertOrdersAsync(orderList.list);
                _logger.LogInformation("{Source} Save to DB in {Elapsed} ({Total})", source, Stopwatch.GetElapsedTime(dbSavingTimestamp), result);

                page++;

            } while (result < orderList.total);

            _logger.LogInformation("{Source} {Total} in {Elapsed}", source, result, Stopwatch.GetElapsedTime(startingTimestamp));

            return result;
        }

        private async Task<int> BulkInsertOrdersAsync(List<Order> orders)
        {
            var table = new DataTable();

            table.Columns.Add("id", typeof(int));
            table.Columns.Add("uid", typeof(string));
            table.Columns.Add("CurrentStatusId", typeof(int));
            table.Columns.Add("updateDate", typeof(DateTime));
            table.Columns.Add("addedDate", typeof(DateTime));
            table.Columns.Add("paymentStatus", typeof(string));
            table.Columns.Add("amount", typeof(double));
            table.Columns.Add("isFake", typeof(bool));
            table.Columns.Add("consumer_firstName", typeof(string));
            table.Columns.Add("consumer_lastName", typeof(string));
            table.Columns.Add("consumer_patronymic", typeof(string));
            table.Columns.Add("consumer_phone", typeof(string));
            table.Columns.Add("TransportCompanyId", typeof(int));
            table.Columns.Add("departureNumber", typeof(string));
            table.Columns.Add("tracking_number", typeof(string));
            table.Columns.Add("DeliveryId", typeof(int));
            table.Columns.Add("WarehouseId", typeof(int));
            table.Columns.Add("CabinetId", typeof(int));
            table.Columns.Add("fromMarketplace", typeof(bool));
            table.Columns.Add("is_allowed_accept_as_defective", typeof(bool));
            table.Columns.Add("serviceCommission", typeof(int));

            foreach (var o in orders)
            {
                table.Rows.Add(
                    o.id,
                    o.uid,
                    o.CurrentStatusId,
                    o.updateDate,
                    o.addedDate,
                    o.paymentStatus,
                    o.amount,
                    o.isFake,
                    o.consumer?.firstName,
                    o.consumer?.lastName,
                    o.consumer?.patronymic,
                    o.consumer?.phone,
                    o.TransportCompanyId,
                    o.departureNumber,
                    o.tracking_number,
                    o.DeliveryId,
                    o.WarehouseId,
                    o.CabinetId,
                    o.fromMarketplace,
                    o.is_allowed_accept_as_defective,
                    o.serviceCommission);
            }
            var connectionString = _configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("DefaultConnection Not Found");

            using var connection = new SqlConnection(connectionString);

            await connection.OpenAsync();

            using var transaction = connection.BeginTransaction();

            try
            {
                using var bulk = new SqlBulkCopy(connection, SqlBulkCopyOptions.TableLock, transaction)
                {
                    DestinationTableName = "[dbo].[Order]",
                    BatchSize = 0,
                    BulkCopyTimeout = 0
                };

                await bulk.WriteToServerAsync(table);

                transaction.Commit();

                return orders.Count;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError(ex, "{Source}", nameof(BulkInsertOrdersAsync));
                return await InsertOrdersOneByOneAsync(orders);
            }
        }

        private async Task<int> InsertOrdersOneByOneAsync(List<Order> orders)
        {
            var result = 0;
            foreach (var order in orders)
            {
                try
                {
                    using var dbContext = _dbContextFactory.CreateDbContext();
                    await dbContext.Order.AddAsync(order);
                    result += await dbContext.SaveChangesAsync();
                    _logger.LogInformation("{Source} {@Order}", nameof(InsertOrdersOneByOneAsync), order);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "{Source} {@Order}", nameof(InsertOrdersOneByOneAsync), order);
                }
            }
            return result;
        }
    }
}
