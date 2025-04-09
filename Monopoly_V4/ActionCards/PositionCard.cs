using Monopoly_V4.Enums;
using Monopoly_V4.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopoly_V4.ActionCards
{
    public class PositionCard(CardType cardType, ISpace space, bool advance, Board board) : MovementCard(cardType, advance, board)
    {
        private readonly ISpace value = space;

        protected override ISpace GetDestinationSpace() => value;         
    }
}
