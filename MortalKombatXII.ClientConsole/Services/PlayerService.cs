using MortalKombatXII.ClientConsole.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace MortalKombatXII.ClientConsole
{
    public class PlayerService
    {
        private readonly HttpClient _client = new HttpClient();

        public async Task<Player> CreatePlayerAsync()
        {
            var response = await _client.GetAsync($"{ApiConfig.Url}/players");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Server is unreachable");
            }
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Player>(json);
        }

        public async Task<List<Room>> GetRoomsAsync()
        {
            var response = await _client.GetAsync($"{ApiConfig.Url}/rooms/open");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Server is unreachable");
            }
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Room>>(json);
        }

        public async Task<Room> CreateRoomAsync(Guid playerId)
        {
            var response = await _client.PostAsync($"{ApiConfig.Url}/rooms/create/{playerId}", null);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Server is unreachable");
            }
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Room>(json);
        }

        public async Task<Room> ConnectToRoomAsync(Guid roomId, Guid playerId)
        {
            var response = await _client.PostAsync($"{ApiConfig.Url}/rooms/connect/{roomId}/{playerId}", null);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Server is unreachable");
            }
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Room>(json);
        }

        public async Task<dynamic> GetRoomStatusAsync(Guid roomId)
        {
            var response = await _client.GetAsync($"{ApiConfig.Url}/rooms/{roomId}/status");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Server is unreachable");
            }
            var json = await response.Content.ReadAsStringAsync();

            return GetRoomByType(json);
        }

        private dynamic GetRoomByType(string json)
        {
            if (json.Contains("Pending"))
            {
                return JsonConvert.DeserializeObject<PendingRoomStatus>(json);
            }
            if (json.Contains("Battle"))
            {
                return JsonConvert.DeserializeObject<BattleRoomStatus>(json);
            }

            return JsonConvert.DeserializeObject<FinishedRoomStatus>(json);
        }
    }
}
