using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopoly_V4.Interfaces
{
    public interface IRealStateProperty : ISpace, IValuable
    {
        public int Price { get; }
        public bool IsMortgaged { get; }        
        public void MortgageProperty();
        public void UnMortgageProperty();
        public int CalculateRent();
        public int CalculateMortgageAmount();
        public int CalculateUnMortgageAmount();        
    }
}
