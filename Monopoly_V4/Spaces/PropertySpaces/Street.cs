using Monopoly_V4.Enums;
using Monopoly_V4.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopoly_V4.Spaces.PropertySpaces
{
    public class Street : RealStateProperty<Street>
    {
        public int BaseRent { get; } // Rent without any houses
        public int BuildingCost { get; }
        public int BuildingCount { get; private set; } // Number of buildings on the street
        public int[] HouseRents { get; }
        public int HotelRent { get; }
        public BuildingType? BuildingType { get; private set; }
        public StreetColor Color { get; set;}              
        public Street(string name, int price, int baseRent, int buildingCost, int[] houseRents, int hotelRent, Bank bank, Player? owner, 
            StreetColor color, IRealStatePropertyGroup<Street> group) : base(name, price, bank, owner, group)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(baseRent, nameof(baseRent));
            ArgumentOutOfRangeException.ThrowIfNegative(buildingCost, nameof(buildingCost));
            ArgumentOutOfRangeException.ThrowIfNegative(houseRents.Min(), nameof(houseRents)); // Validating rents doesn't include a negative
            ArgumentOutOfRangeException.ThrowIfNegative(hotelRent, nameof(hotelRent));
            if (group.Properties.DistinctBy(x => x.Color).Count() != 1 && group.Properties.First().Color != Color)
                throw new ArgumentException("Invalid Group."); // Validating group contains only one color and it matches with this street color.

            Color = color;
            HotelRent = hotelRent;
            HouseRents = houseRents ?? throw new ArgumentNullException(nameof(houseRents));
            BuildingCost = buildingCost;
            BaseRent = baseRent;
        }

        public BuildingType? GetNextBuildingType()
        {
            if (BuildingType == Enums.BuildingType.Hotel)            
                return null;

            if (BuildingCount < HouseRents.Length)
                return BuildingType; // Return house
            else
                return Enums.BuildingType.Hotel;
        }

        public void UpgradeBuilding()
        {
            if (BuildingType == Enums.BuildingType.Hotel)
                throw new InvalidOperationException("Street has reached the building limit.");

            if (BuildingCount < HouseRents.Length)
            {
                BuildingCount++;
                BuildingType = Enums.BuildingType.House;
            }
            else
            {
                BuildingCount = 1;
                BuildingType = Enums.BuildingType.Hotel;
            }
        }

        public void DowngradeBuilding()
        {
            if (BuildingCount == 0)
                throw new InvalidOperationException("Street does not have any buildings.");

            if (BuildingType == Enums.BuildingType.Hotel)
            {
                BuildingCount = HouseRents.Length;
                BuildingType = Enums.BuildingType.House;
            }
            else
            {
                BuildingCount--;
                if (BuildingCount == 0)
                    BuildingType = null;
            }
        }
        public override int CalculateRent()
        {
            if (Owner == null)
                throw new InvalidOperationException("Street does not have an Owner.");

            var rent = BaseRent;
            if (PropertyGroup.GroupOwned(Owner))
                rent *= 2;

            if (BuildingCount > 0)
                if (BuildingType == Enums.BuildingType.House)
                    rent = HouseRents[BuildingCount - 1];
                else 
                    rent = HotelRent;

            return rent;
        }

        protected override void ValidateChangeOwnership()
        {
            base.ValidateChangeOwnership();
            if (BuildingCount > 0)
                throw new InvalidOperationException("Can not change ownership due to Buildings present.");
        }
        protected override void ValidateMortgageProperty()
        {
            base.ValidateMortgageProperty();
            if (BuildingCount > 0)
                throw new InvalidOperationException("Can not mortgage due to Buildings present.");
        }        
    }
}
