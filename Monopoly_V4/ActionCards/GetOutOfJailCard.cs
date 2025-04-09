using Monopoly_V4.Enums;
using Monopoly_V4.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopoly_V4.ActionCards
{
    public class GetOutOfJailCard(CardType cardType, IJail jail) : Valuable, IUsable, ICard
    {
        private readonly IJail jail = jail ?? throw new ArgumentNullException(nameof(jail));
        public CardType CardType { get; } = cardType;
        public bool IsResolved { get => Owner == null; }
        

        // When the card is drawn. Player is stored as owner.
        public void Resolve(Player player)
        {
            ArgumentNullException.ThrowIfNull(player, nameof(player));
            if (!IsResolved)            
                throw new InvalidOperationException("Card is already owned and cannot be reassigned.");

            // Store player as owner if the card is not already drawn, otherwise draw another card.            
            Owner = player;            
        }

        public void Discard()
        {
            if (Owner == null)
                throw new InvalidOperationException("Card is already not in use.");

            Owner = null;
        }        

        public void Use()
        {
            if (Owner == null) throw new InvalidOperationException("Card is not owned.");

            jail.ReleasePlayer(Owner);
            Discard();
        }
    }
}
