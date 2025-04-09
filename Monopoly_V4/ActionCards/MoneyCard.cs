using Monopoly_V4.Enums;
using Monopoly_V4.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopoly_V4.ActionCards
{
    public class MoneyCard(CardType cardType, int amount, bool involveAllPlayers, Bank bank) : ICard
    {
        private readonly Bank bank = bank ?? throw new ArgumentNullException(nameof(bank));
        public bool IsResolved => true;
        public CardType CardType => cardType;

        public void Resolve(Player player)
        {            
            ArgumentNullException.ThrowIfNull(player, nameof(player));

            if (involveAllPlayers)
                ResolveWithPlayers(player);
            else
                ResolveWithBank(player);
        }

        private void ResolveWithPlayers(Player player)
        {
            ArgumentNullException.ThrowIfNull(player, nameof(player));

            foreach (var p in bank.MoneyByPlayersPlaying.Keys)
            {
                if (p == player.PlayerToken) continue;

                if (amount > 0)
                {
                    // Transaction
                }
                else
                {
                    // Transaction
                }
            }            
        }

        private void ResolveWithBank(Player player)
        {
            ArgumentNullException.ThrowIfNull(player, nameof(player));
            if (amount > 0)
            {
                // Transaction
            }
            else
            {
                // Transaction
            }
        }
    }
}
