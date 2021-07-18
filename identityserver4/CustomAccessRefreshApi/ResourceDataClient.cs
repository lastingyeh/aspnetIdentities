using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Serilog;

namespace CustomAccessRefreshApi
{
    public static class ResourceDataClient
    {
        public static async Task GetDataAndDisplayInConsoleAsync(string access_token)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.SetBearerToken(access_token);

            var payloadFromResourceServer = await httpClient.GetAsync("https://localhost:5011/api/DataEventRecords");
            
            if (!payloadFromResourceServer.IsSuccessStatusCode)
            {
                Log.Logger.Warning("Response unsuccessful: {StatusCode}", payloadFromResourceServer.StatusCode);
            }
            else
            {
                var content = await payloadFromResourceServer.Content.ReadAsStringAsync();
                
                Log.Logger.Verbose("Response successful: {StatusCode}", payloadFromResourceServer.StatusCode);
                Log.Logger.Warning("Response successful: {Payload}", content);
            }
        }
    }
}
