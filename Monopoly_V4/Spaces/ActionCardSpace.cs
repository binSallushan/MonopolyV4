using Monopoly_V4.Enums;
using Monopoly_V4.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopoly_V4.Spaces
{
    public class ActionCardSpace : ISpace
    {
        public string Name { get; }
        public CardType Type { get; }
        public ActionCardSpace(CardType cardType)
        {
            ArgumentNullException.ThrowIfNull(cardType, nameof(cardType));

            Type = cardType;
            Name = cardType.ToString();
        }
        public void PlayerLanded(Player player)
        {
            ArgumentNullException.ThrowIfNull(player, nameof(player));
            player.DrawCard(Type);
        }
    }
}
