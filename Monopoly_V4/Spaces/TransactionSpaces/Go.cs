using Monopoly_V4.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopoly_V4.Spaces.TransactionSpaces
{
    public class Go : TransactionSpace, IPassable
    {
        public Go(Bank bank, int salary) : base(bank, salary, "GO")
        {
            ArgumentOutOfRangeException.ThrowIfNegative(salary, nameof(salary));
        }
    }
}
