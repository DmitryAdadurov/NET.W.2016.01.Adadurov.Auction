using DAL.Interface.Entities;
using ORM.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Concrete.Mappers.PropertyDictionaries
{
    public static class PropertyMapperDictionaries
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
                    { "Expired", "ExpirationDate" }
                };
            }
        }

        public static Dictionary<string, string> FromDalBidToBid
        {
            get
            {
                return new Dictionary<string, string>()
                {
                    { "DateOfBid", "BidDate" }
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
                default:
                    mapper = null;
                    return false;
            }
        }
    }
}
