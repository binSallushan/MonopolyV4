using Monopoly_V4.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopoly_V4.Spaces.PropertySpaces
{
    public class Utility : RealStateProperty<Utility>, IVariableRent
    {
        private readonly int[] rentMultiplier; // Numbers multiplied by the dice number of tenant. Length must be equal to the number of total utilities.
        private Func<int, int>? tempRentCalculator;
        private Player? tenant;

        public Utility(string name, int price, int[] multipliers, Bank bank, Player? owner, IRealStatePropertyGroup<Utility> group) : base(name, price, bank, owner, group)
        {
            this.rentMultiplier = multipliers ?? throw new ArgumentNullException(nameof(multipliers));
            if (group.Properties.Count() != multipliers.Length) throw new ArgumentException("Length of rent multipliers must be equal to the number of properties in the group.");            
        }

        public override int CalculateRent()
        {
            if (Owner == null) throw new InvalidOperationException("Utility does not have an owner.");
            if (tenant == null) 
                return 0;

            var utilitiesOwned = PropertyGroup.GetNumberOfPropertiesOwned(Owner);
            var rent = rentMultiplier[utilitiesOwned - 1] * tenant.DiceTotal;

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

        public override void PlayerLanded(Player player)
        {
            base.PlayerLanded(player);
            tenant = player;
        }        
    }
}
