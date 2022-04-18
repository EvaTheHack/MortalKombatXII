using Bogus;
using MortalKombatXII.Core.Models;

namespace MortalKombatXII.Core.Repositories
{
    public class PlayersRepository
    {
        private readonly ApplicationContext _context;
        private readonly Faker<Player> _fakePlayer;

        public PlayersRepository(ApplicationContext context)
        {
            _context = context;
            _fakePlayer = new Faker<Player>()
                .RuleFor(x => x.Id, y => Guid.NewGuid())
                .RuleFor(x => x.Name, y => y.Name.FirstName());
        }

        public Player Get(Guid playerId)
        {
            try
            {
                return _context.Players.First(x => x.Id == playerId);
            }
            catch
            {
                throw new Exception("Unknown player id");
            }
        
        }

        public Player CreatePlayer()
        {
            var player = _fakePlayer.Generate();
            _context.Players.Add(player);
            _context.SaveChanges();
            return player;
        }
    }
}
