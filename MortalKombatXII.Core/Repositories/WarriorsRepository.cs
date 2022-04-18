using MortalKombatXII.Core.Models;

namespace MortalKombatXII.Core.Repositories
{
    public class WarriorsRepository
    {
        private readonly ApplicationContext _context;

        public WarriorsRepository(ApplicationContext context)
        {
            _context = context;
        }

        public Warrior DecreaseHealth(Guid playerId, int damage)
        {
            var w = _context.Warriors.First(x => x.PlayerId == playerId);
            _context.Warriors.First(x => x.PlayerId == playerId).Health = w.Health - damage;
            _context.SaveChanges();
            return w;
        }
    }
}
    