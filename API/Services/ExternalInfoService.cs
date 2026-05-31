using API.Models.DTOs;

namespace API.Services
{
    public class ExternalInfoService
    {
        public async Task<StoreInfoDTO> Obtener()
        {
            var info = new StoreInfoDTO();
            try
            {
                using var client = new HttpClient();
                var url = "https://api.open-meteo.com/v1/forecast?latitude=19.4326&longitude=-99.1332&current=temperature_2m";
                var json = await client.GetStringAsync(url);
                info.Weather = json;
            }
            catch
            {
                info.Weather = "No disponible";
            }
            return info;
        }
    }
}
