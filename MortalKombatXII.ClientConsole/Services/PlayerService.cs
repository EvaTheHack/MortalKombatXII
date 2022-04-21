using MortalKombatXII.ClientConsole.Models;
using MortalKombatXII.ClientConsole.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace MortalKombatXII.ClientConsole
{
    public class PlayerService
    {

        public Player CreatePlayer()
        {
            var json = HttpUtil.Get($"{ApiConfig.Url}/players");

            return JsonConvert.DeserializeObject<Player>(json);
        }

        public List<Room> GetRooms()
        {
            var json = HttpUtil.Get($"{ApiConfig.Url}/rooms/open");

            return JsonConvert.DeserializeObject<List<Room>>(json);
        }

        public Room CreateRoom(Guid playerId)
        {
            var json = HttpUtil.Post($"{ApiConfig.Url}/rooms/create/{playerId}");

            return JsonConvert.DeserializeObject<Room>(json);
        }

        public Room ConnectToRoom(Guid roomId, Guid playerId)
        {
            var json = HttpUtil.Post($"{ApiConfig.Url}/rooms/connect/{roomId}/{playerId}");

            return JsonConvert.DeserializeObject<Room>(json);
        }

        public dynamic GetRoomStatus(Guid roomId)
        {
            var json = HttpUtil.Get($"{ApiConfig.Url}/rooms/{roomId}/status");

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
