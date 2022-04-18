using System;
using System.Collections.Generic;

namespace MortalKombatXII.ClientConsole.Models
{
    public class Room
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<Warrior> Warriors { get; set; }
        public string Winner { get; set; }
        public RoomStatus Status { get; set; }
    }
}
