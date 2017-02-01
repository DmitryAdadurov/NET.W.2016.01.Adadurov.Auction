using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Expressions.PropertyDictionaries
{
    public static partial class PropertyMapperDictionaries
    {
        public static Dictionary<string, string> FromBllBidToDalBid
        {
            get
            {
                return new Dictionary<string, string>()
                {
                    { "BidDate", "DateOfBid" },
                };
            }
        }

        public static Dictionary<string, string> FromBllLotToDalLot
        {
            get
            {
                return new Dictionary<string, string>()
                {
                    { "CreationDate", "DateOfCreation" },
                    { "LastUpdateDate", "LastUpdated" },
                    { "AuctionEndDate", "Expired" },
                    { "CurrentPrice", "Price" }
                };
            }
        }

        public static Dictionary<string, string> FromBllUserToDalUser
        {
            get
            {
                return new Dictionary<string, string>()
                {
                    { "Login", "UserName" }
                };
            }
        }
    }
}
