using Monopoly_V4.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopoly_V4
{
    public class TransactionInfo(PlayerToken? payee, PlayerToken? payer, int amount, bool required, Func<TransactionState> getTransactionState)
    {
        public int Amount { get => amount; }
        public bool Required { get => required; }
        public TransactionState State { get => getTransactionState(); }
        public PlayerToken? Payee { get => payee; }
        public PlayerToken? Payer { get => payer; }
    }
}
