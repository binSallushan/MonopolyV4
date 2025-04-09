using Monopoly_V4.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopoly_V4
{
    public class VariableRentChangerDouble : IVariableRentChanger
    {
        public void ChangeVariableRent(IVariableRent property)
        {
            ArgumentNullException.ThrowIfNull(property, nameof(property));
            property.ChangeRentTemporarily(x => x * 2);
        }
    }
}
