using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopoly_V4.Spaces.TransactionSpaces
{
    public class TaxSpace : TransactionSpace
    {
        public TaxSpace(Bank bank, int amount, string name) : base(bank, amount, name)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(amount, nameof(amount));
        }
    }
}
