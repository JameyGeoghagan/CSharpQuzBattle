namespace CSharpQuizBattle.Models
{
    public class Enemy : Character
    {
        public Enemy(string name, int health, int attack, int defense)
        {
            Name = name;
            Health = health;
            Attack = attack;
            Defense = defense;
        }

        public override int DealDamage()
        {
            return Attack;
        }
    }
}
