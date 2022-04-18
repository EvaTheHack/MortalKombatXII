namespace MortalKombatXII.Core.Models
{
    public class PendingRoomStatus
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int CountPlayers { get; set; }
        public RoomStatus Status { get; set; }
    }
}
