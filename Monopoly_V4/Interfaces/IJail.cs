using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopoly_V4.Interfaces
{
    public interface IJail : ISpace
    {
        void ArrestPlayer(Player player);
        void ReleasePlayer(Player player);
        IEnumerable<Player> GetArrestedPlayers();
    }
}
