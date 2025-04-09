using Monopoly_V4.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopoly_V4.Interfaces
{
    public interface ICard
    {
        public bool IsResolved { get; }
        public CardType CardType { get; }
        void Resolve(Player player);
    }    
}
