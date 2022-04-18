using System;

namespace MortalKombatXII.ClientConsole.Models
{
    public class FinishedRoomStatus
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Winner { get; set; }
        public RoomStatus Status { get; set; }

    }
}
