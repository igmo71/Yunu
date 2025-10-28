using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Yunu.Api.Domain
{
    [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
    public class OrderList
    {
        public List<Order>? list { get; set; }
        public int total { get; set; }
        public string? cisExportLink { get; set; }
        public bool hasDefectiveOrders { get; set; }
    }

    [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
    public class Order
    {
        public int id { get; set; }
        public string? uid { get; set; }

        public int CurrentStatusId { get; set; } // My Key
        public CurrentStatus? currentStatus { get; set; }

        [NotMapped]
        public List<StatusList>? statusList { get; set; }

        public DateTime updateDate { get; set; }
        public DateTime addedDate { get; set; }
        public string? paymentStatus { get; set; }
        public int amount { get; set; }
        public bool isFake { get; set; }
        
        public Consumer? consumer { get; set; }

        public int TransportCompanyId { get; set; } // My Key
        public TransportCompany? transportCompany { get; set; }

        public string? departureNumber { get; set; }
        public string? tracking_number { get; set; }

        public int DeliveryId { get; set; } // My Key
        public Delivery? delivery { get; set; }

        public int WarehouseId { get; set; } // My Key
        public Warehouse? warehouse { get; set; }

        public int CabinetId { get; set; } // My Key
        public Cabinet? cabinet { get; set; }

        public bool fromMarketplace { get; set; }
        public bool is_allowed_accept_as_defective { get; set; }
        public int serviceCommission { get; set; }
    }    

    [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
    public class Consumer
    {
        public string? firstName { get; set; }
        public string? lastName { get; set; }
        public string? patronymic { get; set; }
        public string? phone { get; set; }
    }

    [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
    public class StatusList
    {
        public int id { get; set; }
        public string? label { get; set; }
    }
}
