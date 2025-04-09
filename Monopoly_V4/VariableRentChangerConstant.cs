using Monopoly_V4.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopoly_V4
{
    public class VariableRentChangerConstant(int value) : IVariableRentChanger
    {
        public void ChangeVariableRent(IVariableRent property)
        {
            ArgumentNullException.ThrowIfNull(property, nameof(property));
            property.ChangeRentTemporarily(x => value);
        }
    }
}
