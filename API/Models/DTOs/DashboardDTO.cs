namespace API.Models.DTOs
{
    public class DashboardDTO
    {
        public int TotalProducts { get; set; }
        public int TotalOrders { get; set; }
        public int PendingOrders { get; set; }
        public decimal TotalSales { get; set; }
    }

    public class StoreInfoDTO
    {
        public string Name { get; set; } = "Cafe Punto de Venta";
        public string Address { get; set; } = "Centro, CDMX";
        public decimal Latitude { get; set; } = 19.4326m;
        public decimal Longitude { get; set; } = -99.1332m;
        public string Weather { get; set; } = "";
    }
}
