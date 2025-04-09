using Monopoly_V4.Enums;
using Monopoly_V4.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopoly_V4.ActionCards
{
    public class NearestRentCard(CardType cardType, IVariableRent[] possibleSpaces, IVariableRentChanger rentChanger, Board board) : NearestPositionCard(cardType, true, possibleSpaces, board)
    {        
        private IVariableRent? destinationSpace;
        protected override ISpace GetDestinationSpace()
        {
            destinationSpace = (base.GetDestinationSpace() as IVariableRent) ?? throw new InvalidOperationException("No appropriate space found.");
            return destinationSpace;
        }
        public override void Resolve(Player player)
        {
            if (destinationSpace == null) throw new InvalidOperationException("No appropriate space found.");
            base.Resolve(player);

            if (destinationSpace.Owner != null)
                rentChanger.ChangeVariableRent(destinationSpace);            
        }
    }
}
