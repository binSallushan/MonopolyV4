using Monopoly_V4.Enums;
using Monopoly_V4.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopoly_V4.ActionCards
{
    public class NearestPositionCard(CardType cardType, bool advance, ISpace[] possibleSpaces, Board board) : MovementCard(cardType, advance, board), ICard
    {
        private readonly ISpace[] possibleSpaces = possibleSpaces ?? throw new ArgumentNullException(nameof(possibleSpaces));
        
        protected Player? player;
        protected ISpace GetNearestSpaceForAdvance(int playerOrigin)
        {
            var spacesAheadPlayer = board.AddressBySpace.Where(x => x.Value > playerOrigin && possibleSpaces.Contains(x.Key));
            if (spacesAheadPlayer.Any())
                return spacesAheadPlayer.Where(x => x.Value == spacesAheadPlayer.Min(x => x.Value)).Select(x => x.Key).First(); // Return the closest in front of player

            // Closest is behind the player now or if the player has to go around the board
            IEnumerable<KeyValuePair<ISpace, int>> variableRents = board.AddressBySpace.Where(x => possibleSpaces.Contains(x.Key));
            if (!variableRents.Any())
                throw new InvalidOperationException("No appropriate space found.");

            return variableRents.Where(x => x.Value == variableRents.Min(x => x.Value)).Select(x => x.Key).First();
        }
        protected ISpace GetClosestSpaceToPlayer(int playerOrigin)
        {
            var minDifference = Math.Abs(playerOrigin - board.AddressBySpace[possibleSpaces.First()]);
            var minSpace = possibleSpaces.First();
            for (var i = 1; i < possibleSpaces.Length; i++)
            {
                var difference = playerOrigin - board.AddressBySpace[possibleSpaces[i]];
                if (difference < minDifference)
                {
                    minDifference = difference;
                    minSpace = possibleSpaces[i];
                }
            }

            return minSpace;
        }
        
        protected override ISpace GetDestinationSpace()
        {
            if (player == null) throw new InvalidOperationException("Player is null.");

            var playerOrigin = board.GetAddressOfPlayer(player.PlayerToken);
            if (advance)            
                return GetNearestSpaceForAdvance(playerOrigin);            
            else
                return GetClosestSpaceToPlayer(playerOrigin);
        }

        public override void Resolve(Player player)
        {
            this.player = player;
            base.Resolve(player);            
        }


    }
}
