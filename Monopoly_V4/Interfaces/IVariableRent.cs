using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopoly_V4.Interfaces
{
    public interface IVariableRent : IRealStateProperty
    {
        void ChangeRentTemporarily(Func<int, int> tempRentFunction);
    }
}
