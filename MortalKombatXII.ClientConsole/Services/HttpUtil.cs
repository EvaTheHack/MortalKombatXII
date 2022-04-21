using System;
using System.Net.Http;

namespace MortalKombatXII.ClientConsole.Services
{
    public class HttpUtil
    {
        private static readonly HttpClient _client = new HttpClient();

        public static string Get(string url)
        {
            var response = _client.GetAsync(url).Result;
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Server is unreachable");
            }
            return response.Content.ReadAsStringAsync().Result;
        }

        public static string Post(string url)
        {
            var response = _client.PostAsync(url, null).Result;
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Server is unreachable");
            }
            return response.Content.ReadAsStringAsync().Result;
        }
    }
}
