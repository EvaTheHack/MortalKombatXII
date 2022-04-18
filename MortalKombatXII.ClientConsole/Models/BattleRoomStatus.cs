using System;
using System.Collections.Generic;

namespace MortalKombatXII.ClientConsole.Models
{
    public class BattleRoomStatus
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Dictionary<string, int> Warriors { get; set; }
        public RoomStatus Status { get; set; }

    }
}
