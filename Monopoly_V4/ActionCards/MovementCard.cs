using Monopoly_V4.Enums;
using Monopoly_V4.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopoly_V4.ActionCards
{
    public abstract class MovementCard(CardType cardType, bool advance, Board board) : ICard
    {
        public bool IsResolved => true;

        public CardType CardType { get; } = cardType;
        protected bool advance = advance;
        protected readonly Board board = board ?? throw new ArgumentNullException(nameof(board));

        protected abstract ISpace GetDestinationSpace();

        public virtual void Resolve(Player player)
        {
            ArgumentNullException.ThrowIfNull(player, nameof(player));

            if (advance)
                board.AdvancePlayer(player, GetDestinationSpace());
            else
                board.MovePlayer(player, board.AddressBySpace[GetDestinationSpace()]);
        }
    }
}
