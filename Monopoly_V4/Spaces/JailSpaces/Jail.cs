using Monopoly_V4.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopoly_V4.Spaces.JailSpaces
{
    public class Jail : IJail
    {
        public string Name => "Jail";

        private readonly Dictionary<Player, int> arrestedTurnNumberByPlayer = [];
        public IReadOnlyDictionary<Player, int> ArrestedTurnNumberByPlayer { get => arrestedTurnNumberByPlayer; }

        public IEnumerable<Player> GetArrestedPlayers()
        {
            return arrestedTurnNumberByPlayer.Keys;
        }

        public void PlayerLanded(Player player)
        {
            ArgumentNullException.ThrowIfNull(player, nameof(player));
            if (!arrestedTurnNumberByPlayer.ContainsKey(player)) throw new ArgumentException("Player is not arrested.");

            arrestedTurnNumberByPlayer.Remove(player);
        }

        public void ArrestPlayer(Player player)
        {
            ArgumentNullException.ThrowIfNull(player, nameof(player));
            if (arrestedTurnNumberByPlayer.ContainsKey(player)) throw new ArgumentException("Player already arrested.");

            arrestedTurnNumberByPlayer.Add(player, player.TurnNumber);            
        }
        public void ReleasePlayer(Player player)
        {
            throw new NotImplementedException();
        }                
    }
}
