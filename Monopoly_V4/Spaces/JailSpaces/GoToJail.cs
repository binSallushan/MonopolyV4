using Monopoly_V4.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopoly_V4.Spaces.JailSpaces
{
    public class GoToJail(IJail jail) : ISpace
    {
        public string Name => "Go To Jail";
        private readonly IJail jail = jail ?? throw new ArgumentNullException(nameof(jail));

        public void PlayerLanded(Player player)
        {
            ArgumentNullException.ThrowIfNull(player, nameof(player));
            jail.ArrestPlayer(player);
        }
    }
}
