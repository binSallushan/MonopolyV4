using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Monopoly_V4.Interfaces
{
    public interface IAuction
    {
        Player? HighestBidder { get; }
        int BidAmount { get; }
        bool AuctionFinished { get; }
        void Bid(Player transactable, int amount);
        void RemoveBidder(Player transactable);
        void FinishAuction();
    }
}
