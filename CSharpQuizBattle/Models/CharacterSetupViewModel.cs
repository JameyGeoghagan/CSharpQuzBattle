namespace CSharpQuizBattle.Models
{
    public class CharacterSetupViewModel
    {
        public string PlayerName { get; set; }
        public string SelectedAvatar { get; set; }
        public List<string> AvailableAvatars { get; set; } = new List<string>();
    }
}
