using Bogus;
using Microsoft.EntityFrameworkCore;
using MortalKombatXII.Core.Models;

namespace MortalKombatXII.Core.Repositories
{
    public class RoomsRepository
    {
        private readonly ApplicationContext _context;
        private readonly Faker<Room> _fakeRoom;

        public RoomsRepository(ApplicationContext context)
        {
            _context = context;
            _fakeRoom = new Faker<Room>()
                .RuleFor(x => x.Id, y => Guid.NewGuid())
                .RuleFor(x => x.Name, y => y.Address.City())
                .RuleFor(x => x.Warriors, y => new List<Warrior>())
                .RuleFor(x => x.Winner, y => string.Empty)
                .RuleFor(x => x.Status, y => RoomStatus.Pending);
        }

        public Room CteateRoom(Player player)
        {
            var room = _fakeRoom.Generate();
            room.Warriors.Add(new Warrior { PlayerId = player.Id, Name = player.Name});

            _context.Rooms.Add(room);
            _context.SaveChanges();

            return room;
        }

        public List<Room> GetRooms()
        {
            return _context.Rooms.Include(x => x.Warriors).ToList();
        }

        public Room GetRoom(Guid roomId)
        {
            try
            {
                return _context.Rooms.Include(x => x.Warriors).First(x => x.Id == roomId);
            }
            catch
            {
                throw new Exception("Unknown room id");
            }
        }

        public Room ConnectToRoom(Guid roomId, Player player)
        {
            var room = GetRoom(roomId);

            if (room.Status == RoomStatus.Finished)
            {
                throw new Exception($"This room currently closed; Winner is {room.Winner}");
            }
            if (room.Warriors.Count == 2)
            {
                throw new Exception("Room may has only 2 players. Room is full");
            }
            
            room.Warriors.Add(new Warrior { PlayerId = player.Id, Name = player.Name });
            _context.SaveChanges();
            
            return GetRoom(roomId);
        }

        public Room? GetPendingRoom()
        {
            return _context.Rooms.Include(x => x.Warriors)
                                 .FirstOrDefault(x => x.Status == RoomStatus.Pending && x.Warriors.Count == 2);
        }

        public void ChangeStatus(Guid roomId, RoomStatus status)
        {
            var room = _context.Rooms.First(x => x.Id == roomId);
            room.Status = status;
            _context.Rooms.Attach(room).Property(x => x.Status).IsModified = true;
            _context.SaveChanges();
        }

        public void SetWinner(Guid roomId, string name)
        {
            var room = _context.Rooms.First(x => x.Id == roomId);
            room.Winner = name;
            _context.Rooms.Attach(room).Property(x => x.Winner).IsModified = true;
            _context.SaveChanges();
        }
    }
}
