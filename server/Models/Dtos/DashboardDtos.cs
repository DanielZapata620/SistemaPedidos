namespace PedidoApi.Models.Dtos;

public class DashboardDto
{
    public int TotalProducts { get; set; }
    public int TotalOrders { get; set; }
    public int PendingOrders { get; set; }
    public decimal TotalSales { get; set; }
}

public class StoreInfoDto
{
    public string StoreName { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string WeatherSummary { get; set; } = string.Empty;
    public decimal PickupPaymentFee { get; set; }
}
