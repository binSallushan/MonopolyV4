using Monopoly_V4.Enums;
using Monopoly_V4.Interfaces;
using Monopoly_V4.Spaces.PropertySpaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopoly_V4.ActionCards
{
    public class RepairCard(CardType cardType, int moneyPerHouse, int moneyPerHotel, Street[] streets, Bank bank) : ICard
    {
        public bool IsResolved => true;

        public CardType CardType => cardType;

        public void Resolve(Player player)
        {
            ArgumentNullException.ThrowIfNull(player, nameof(player));

            var streetsOwned = streets.Where(x => x.Owner == player && x.BuildingCount > 0);
            var houseFee = streetsOwned.Where(x => x.BuildingType == BuildingType.House).Select(x => x.BuildingCount).Sum() * moneyPerHouse;
            var hotelFee = streetsOwned.Where(x => x.BuildingType == BuildingType.Hotel).Select(x => x.BuildingCount).Sum() * moneyPerHotel;
            // Pay bank
        }
    }
}
