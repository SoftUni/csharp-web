namespace BattleCards.ViewModels.Cards
{
    public class DoAddViewModel
    {
        public int Attack { get; set; }

        public int Health { get; set; }

        public int Damage => Attack * 10 + Health;
    }
}
