namespace MortalKombatXII.Core.Models
{
    public class Warrior
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid PlayerId { get; set; }
        public int Health { get; set; } = 10;
        public bool IsAlive => Health > 0;
    }
}
