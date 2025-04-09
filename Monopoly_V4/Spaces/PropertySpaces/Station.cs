using Monopoly_V4.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopoly_V4.Spaces.PropertySpaces
{
    public class Station : RealStateProperty<Station>, IVariableRent
    {
        public int BaseRent { get; } // Rent if only one is owned as Station's rent changes depening on the number of stations owned.
        private Func<int, int>? tempRentCalculator; // Function to calculate the rent if it is changed temporarily, due to some action cards.
        public Station(string name, int price, int baseRent, Bank bank, Player? owner, IRealStatePropertyGroup<Station> group) : base(name, price, bank, owner, group)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(baseRent, nameof(baseRent));
            BaseRent = baseRent;
        }

        public override int CalculateRent()
        {
            if (Owner == null) throw new InvalidOperationException("Station does not have an owner.");

            var stationsOwned = PropertyGroup.GetNumberOfPropertiesOwned(Owner);
            var rent = BaseRent * (int)Math.Pow(2, stationsOwned - 1);

            if (tempRentCalculator != null)
            {
                rent = tempRentCalculator(rent);
                tempRentCalculator = null;
            }

            return rent;
        }

        public void ChangeRentTemporarily(Func<int, int> tempRentFunction)
        {
            ArgumentNullException.ThrowIfNull(tempRentFunction, nameof(tempRentFunction));
            tempRentCalculator = tempRentFunction;
        }        
    }
}
