using Monopoly_V4.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopoly_V4.Spaces.TransactionSpaces
{
    public class TransactionSpace : ISpace
    {
        public string Name { get; }
        readonly Bank bank;
        readonly int amount;

        public TransactionSpace(Bank bank, int amount, string name)
        {
            if (amount == 0) throw new ArgumentException("Amount can not be 0.");
            this.bank = bank ?? throw new ArgumentNullException(nameof(bank));
            this.amount = amount;
            Name = name;
        }

        public void PlayerLanded(Player player)
        {
            ArgumentNullException.ThrowIfNull(player, nameof(player));
            if (amount > 0)
            {
                //var transaction = bank.StartTransaction(player.PlayerToken, null, money, true);
                //bank.CompleteTransaction(transaction);
            }
            else
            {
                //var transaction = bank.StartTransaction(null, player.PlayerToken, Math.Abs(amount), true);
                //bank.CompleteTransaction(transaction);
            }

        }
    }
}
