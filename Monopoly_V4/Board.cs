using Monopoly_V4.Enums;
using Monopoly_V4.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopoly_V4
{
    public class Board
    {
        public int TotalSpaces { get; }
        public IReadOnlyDictionary<ISpace, int> AddressBySpace { get => addressBySpace; }

        private readonly Dictionary<ISpace, int> addressBySpace;
        private readonly Dictionary<PlayerToken, int> addressOfPlayerToken;
        private readonly Dictionary<int, IPassable> passableByAddress;
        private readonly Dictionary<IJail, int> addressOfJail;

        public Board(ISpace[] spaces, PlayerToken[] playerTokens)
        {
            ArgumentNullException.ThrowIfNull(spaces, nameof(spaces));
            ArgumentNullException.ThrowIfNull(playerTokens, nameof(playerTokens));            

            TotalSpaces = spaces.Length;
            addressBySpace = [];
            addressOfPlayerToken = [];
            passableByAddress = [];
            addressOfJail = [];

            for (int i = 1; i <= spaces.Length; i++)
            {
                if (spaces[i] is IPassable p)
                    passableByAddress.Add(i, p);
                else if (spaces[i] is IJail j)
                    addressOfJail.Add(j, i);
                else
                    addressBySpace.Add(spaces[i], i);                
            }

            foreach (var token in playerTokens)
            {
                addressOfPlayerToken.Add(token, 1);
            }
        }

        public Dictionary<PlayerToken, int> GetAddressesOfPlayers()
        {
            var dict = new Dictionary<PlayerToken, int>();
            foreach (var token in addressOfPlayerToken.Keys)
            {
                foreach (var jail in addressOfJail.Keys)
                {
                    if (jail.GetArrestedPlayers().Any(x => x.PlayerToken == token))
                    {
                        ChangePlayerTokenAddress(token, addressOfJail[jail]);
                    }
                }

                dict.Add(token, addressOfPlayerToken[token]);
            }


            return dict;            
        }
        public int GetAddressOfPlayer(PlayerToken playerToken)
        {
            ArgumentNullException.ThrowIfNull(playerToken, nameof(playerToken));
            if (!addressOfPlayerToken.ContainsKey(playerToken)) throw new ArgumentException("Player Token not found");

            foreach (var jail in addressOfJail.Keys)
            {
                if (jail.GetArrestedPlayers().Any(x => x.PlayerToken == playerToken))
                {
                    ChangePlayerTokenAddress(playerToken, addressOfJail[jail]);
                }
            }

            return addressOfPlayerToken[playerToken];
        }
        
        public ISpace? GetSpaceByAddress(int address)
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(address, 1, nameof(address));
            return AddressBySpace.Where(x => x.Value == address).Select(x => x.Key).FirstOrDefault();
        }

        public void AdvancePlayer(Player player, ISpace space)
        {
            ArgumentNullException.ThrowIfNull(player, nameof(player));
            ArgumentNullException.ThrowIfNull(space, nameof(space));            

            var playerToken = player.PlayerToken;
            var playerOrigin = GetAddressOfPlayer(playerToken);            
            var spaceAddress = addressBySpace[space];

            if (spaceAddress < playerOrigin)
            {
                // Space is behind player, needs to cover almost the whole board to reach
                var difference = playerOrigin - spaceAddress;
                var distanceToCover = TotalSpaces - difference;
                AdvancePlayer(player, distanceToCover);
            }
            else
                AdvancePlayer(player, spaceAddress - playerOrigin);
        }
        public void AdvancePlayer(Player player, int distance)
        {
            ArgumentNullException.ThrowIfNull(player, nameof(player));
            ArgumentOutOfRangeException.ThrowIfNegative(distance, nameof(distance));
            var playerToken = player.PlayerToken;

            if (distance == 0)
            {
                GetSpaceByAddress(addressOfPlayerToken[playerToken])!.PlayerLanded(player);
            }
            var rounds = distance / TotalSpaces; // Get the number of rounds player has to go around board                  
            for (int i = 0; i < rounds; i++)
            {
                foreach (var passable in passableByAddress.Values)               
                    passable.PlayerPassed(player); // Player will pass passables if it takes 1 or more rounds around the board.
                
            }
            
            var playerOrigin = GetAddressOfPlayer(playerToken);
            var destinationAddress = (distance % TotalSpaces) + playerOrigin; // Get the destination address
            List<IPassable> passablesPassedBetweenOriginAndDestination = [];

            if (destinationAddress > TotalSpaces)            
                destinationAddress -= TotalSpaces;

            if (destinationAddress < playerOrigin)
            {
                // Space is behind player, needs to cover almost the whole board to reach
                passablesPassedBetweenOriginAndDestination.AddRange(passableByAddress.Where(x => x.Key >= playerOrigin && x.Key <= TotalSpaces).Select(x => x.Value)); // Passables between player and end of board
                passablesPassedBetweenOriginAndDestination.AddRange(passableByAddress.Where(x => x.Key >= 1 && x.Key <= destinationAddress).Select(x => x.Value)); // Passables between the first space and destination
            }
            else
                passablesPassedBetweenOriginAndDestination.AddRange(passableByAddress.Where(x => x.Key >= playerOrigin && x.Key <= destinationAddress).Select(x => x.Value));
            
            foreach (var passable in passablesPassedBetweenOriginAndDestination) 
            {
                passable.PlayerPassed(player);
            }

            // Final movement, all passables have been passed.            
            MovePlayer(player, destinationAddress);
        }
        public void MovePlayer(Player player, int destinationAddress)
        {
            ArgumentNullException.ThrowIfNull(player, nameof(player));
            ArgumentOutOfRangeException.ThrowIfLessThan(destinationAddress, 1, nameof(destinationAddress));
            if (!addressBySpace.ContainsValue(destinationAddress)) throw new ArgumentException("Destination Address not found.");

            ChangePlayerTokenAddress(player.PlayerToken, destinationAddress);
            GetSpaceByAddress(destinationAddress)!.PlayerLanded(player);
        }

        private void ChangePlayerTokenAddress(PlayerToken playerToken, int address) 
        {
            ArgumentNullException.ThrowIfNull(playerToken, nameof(playerToken));
            ArgumentOutOfRangeException.ThrowIfLessThan(address, 1, nameof(address));
            if (!addressOfPlayerToken.ContainsKey(playerToken)) throw new ArgumentException("Player Token not found");

            addressOfPlayerToken[playerToken] = address;
        }                
    }
}
