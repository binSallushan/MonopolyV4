using Monopoly_V4.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopoly_V4.Spaces
{
    public class FreeParking : ISpace
    {
        public string Name => "Free Parking";

        public void PlayerLanded(Player player)
        {            
        }
    }
}
