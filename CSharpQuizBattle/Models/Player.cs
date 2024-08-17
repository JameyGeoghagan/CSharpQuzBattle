namespace CSharpQuizBattle.Models
{
    public class Player : Character
    {
        public string Avatar { get; set; } // Add this line to store the avatar information

        public Player(string name, string avatar)
        {
            Name = name;
            Health = 100;
            Attack = 10;
            Defense = 5;
            Avatar = avatar; // Set the avatar property
        }

        public override int DealDamage()
        {
            return Attack;
        }
    }
}
