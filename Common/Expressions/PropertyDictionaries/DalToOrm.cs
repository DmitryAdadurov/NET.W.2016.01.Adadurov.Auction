using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Expressions.PropertyDictionaries
{
    public static partial class PropertyMapperDictionaries
    {
        public static Dictionary<string, string> FromDalLotToLot
        {
            get
            {
                return new Dictionary<string, string>()
                {
                    { "DateOfCreation", "CreationDate" },
                    { "LastUpdated", "LastUpdatedDate" },
                    { "Price", "CurrentPrice" },
                    { "Expired", "ExpirationDate" },
                    { "Categorie", "CtegorieId" },
                    { "Seller", "SellerId" }
                };
            }
        }

        public static Dictionary<string, string> FromDalBidToBid
        {
            get
            {
                return new Dictionary<string, string>()
                {
                    { "DateOfBid", "BidDate" },
                    { "Lot", "LotId" },
                    { "User", "UserId" }
                };
            }
        }

        public static Dictionary<string, string> FromDalCommentaryToCommentary
        {
            get
            {
                return new Dictionary<string, string>()
                {
                    { "Lot", "LotId" },
                    { "User", "UserId" }
                };
            }
        }

        public static bool TryGetMapperDictionary(Type type, out IDictionary<string, string> mapper)
        {
            switch (type.Name)
            {
                case "DalLot":
                    mapper = FromDalLotToLot;
                    return true;
                case "DalBid":
                    mapper = FromDalBidToBid;
                    return true;
                case "DalCommentary":
                    mapper = FromDalCommentaryToCommentary;
                    return true;
                case "BllUser":
                    mapper = FromBllUserToDalUser;
                    return true;
                case "BllLot":
                    mapper = FromBllLotToDalLot;
                    return true;
                case "BllBid":
                    mapper = FromBllBidToDalBid;
                    return true;
                default:
                    mapper = null;
                    return false;
            }
        }
    }
}
