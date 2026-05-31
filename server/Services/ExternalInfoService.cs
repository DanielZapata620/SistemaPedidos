using PedidoApi.Models.Dtos;

namespace PedidoApi.Services;

public class ExternalInfoService
{
    private readonly HttpClient _httpClient = new();

    public async Task<StoreInfoDto> GetStoreInfo()
    {
        var weather = "Clima no disponible";

        try
        {
            var url = "https://api.open-meteo.com/v1/forecast?latitude=19.4326&longitude=-99.1332&current=temperature_2m";
            using var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                weather = "Clima consultado con Open-Meteo";
            }
        }
        catch
        {
            weather = "Clima no disponible por conexion";
        }

        return new StoreInfoDto
        {
            StoreName = "Cafe Punto de Venta",
            Address = "Centro, CDMX",
            Latitude = 19.4326,
            Longitude = -99.1332,
            WeatherSummary = weather,
            PickupPaymentFee = 0
        };
    }
}
