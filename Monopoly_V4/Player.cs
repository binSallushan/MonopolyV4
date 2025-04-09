using Monopoly_V4.Enums;

namespace Monopoly_V4
{
    public class Player
    {
        public int TurnNumber { get; internal set; }
        public int DiceTotal { get; internal set; }
        public PlayerToken PlayerToken { get; internal set; }

        internal void DrawCard(CardType cardType)
        {
            throw new NotImplementedException();
        }

        internal void TerminateCard()
        {
            throw new NotImplementedException();
        }
    }
}