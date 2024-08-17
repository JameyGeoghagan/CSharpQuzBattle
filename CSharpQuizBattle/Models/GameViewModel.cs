using CSharpQuizBattle.Services;

namespace CSharpQuizBattle.Models
{
    public class GameViewModel
    {
        public Player Player { get; set; }
        public Enemy Enemy { get; set; }
        public Question CurrentQuestion { get; set; } // Assuming the question class from Services
    }
}
