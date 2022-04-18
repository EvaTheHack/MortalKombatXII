using Microsoft.EntityFrameworkCore;
using MortalKombatXII.Core.Models;

namespace MortalKombatXII.Core.Repositories
{
    public class ApplicationContext: DbContext
    {
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Warrior> Warriors { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
                :base(options)
        {
            Database.EnsureCreated();
        }
    }
}
