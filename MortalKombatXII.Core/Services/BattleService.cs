using MortalKombatXII.Core.Models;
using MortalKombatXII.Core.Repositories;

namespace MortalKombatXII.Core.Services
{
    public class BattleService
    {
        private readonly RoomsRepository _rooms;
        private readonly WarriorsRepository _warriors;

        private readonly Random random = new();

        public BattleService(RoomsRepository rooms, WarriorsRepository warriors)
        {
            _rooms = rooms;
            _warriors = warriors;
        }

        public void Battle()
        {
            var preparedRoom = _rooms.GetPendingRoom();
            if (preparedRoom == null)
            {
                return;
            }

            StartBattle(preparedRoom);
            FinishBattle(preparedRoom.Id);
        }

        private Room StartBattle(Room preparedRoom)
        {
            _rooms.ChangeStatus(preparedRoom.Id, RoomStatus.Battle);

            while (true)
            {
                Thread.Sleep(1000);
                var aliveWarriors = preparedRoom.Warriors.Where(x => x.IsAlive);

                foreach (var aliveWarrior in aliveWarriors)
                {
                    var damage = random.Next(0, 2);
                    aliveWarrior.Health = _warriors.DecreaseHealth(aliveWarrior.PlayerId, damage).Health;

                    if (IsBattleFinished(aliveWarriors))
                    {
                        return preparedRoom;
                    }
                }
            }
        }

        private void FinishBattle(Guid roomId)
        {
            var room = _rooms.GetRoom(roomId);
            var winner = room.GetAlliveWarrior().Name;

            _rooms.ChangeStatus(roomId, RoomStatus.Finished);
            _rooms.SetWinner(roomId, winner);
        }

        private bool IsBattleFinished(IEnumerable<Warrior> warriors)
        {
            return warriors.Count(x => x.Health > 0) == 1;
        }
    }
}
