namespace Yunu.Api.Common
{
    public class Utils
    {
        public static int GetIterationCount(int totalItems, int itemsPerPage)
        {
            int totalPages = totalItems / itemsPerPage;

            if (totalItems % itemsPerPage != 0)
                totalPages++;

            return totalPages;
        }
    }
}
