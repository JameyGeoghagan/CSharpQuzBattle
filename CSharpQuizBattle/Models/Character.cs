namespace CSharpQuizBattle.Models
{
    public abstract class Character
    {
        public string Name { get; set; }
        public int Health { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public int RecentDamage { get; private set; }
        public string Avatar { get; set; }

        public bool IsAlive => Health > 0;

        public virtual int TakeDamage(int damage)
        {
            int damageTaken = damage - Defense;
            if (damageTaken > 0)
            {
                Health -= damageTaken;
            }
            RecentDamage = damageTaken > 0 ? damageTaken : 0;
            return RecentDamage;
        }

        public abstract int DealDamage();
    }
}
