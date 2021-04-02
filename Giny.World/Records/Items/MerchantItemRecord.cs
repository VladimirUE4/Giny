using Giny.Core.DesignPattern;
using Giny.ORM;
using Giny.ORM.Attributes;
using Giny.ORM.Interfaces;
using Giny.Protocol.Types;
using Giny.World.Managers.Items;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Records.Items
{
    [Table("merchantitems")]
    public class MerchantItemRecord : AbstractItem, ITable
    {
        private static readonly ConcurrentDictionary<long, MerchantItemRecord> MerchantItems = new ConcurrentDictionary<long, MerchantItemRecord>();

        [Ignore]
        public long Id => UId;

        public long CharacterId
        {
            get;
            set;
        }
        public long Price
        {
            get;
            set;
        }

        [WIP] // what quantity was solded ?
        public bool Sold
        {
            get;
            set;
        }

        public override AbstractItem CloneWithoutUID()
        {
            return new MerchantItemRecord()
            {
                CharacterId = CharacterId,
                AppearanceId = this.AppearanceId,
                Effects = this.Effects.Clone(),
                GId = GId,
                Look = Look,
                Position = this.Position,
                Quantity = this.Quantity,
                UId = ItemManager.Instance.PopItemUID(),
            };
        }

        public override AbstractItem CloneWithUID()
        {
            return new MerchantItemRecord()
            {
                CharacterId = CharacterId,
                AppearanceId = this.AppearanceId,
                Effects = this.Effects.Clone(),
                GId = GId,
                Look = Look,
                Position = this.Position,
                Quantity = this.Quantity,
                UId = UId,
            };
        }
        public ObjectItemToSellInHumanVendorShop GetObjectItemToSellInHumanVendorShop()
        {
            return new ObjectItemToSellInHumanVendorShop()
            {
                effects = Effects.Select(x => x.GetObjectEffect()).ToArray(),
                objectGID = GId,
                objectPrice = Price,
                objectUID = UId,
                publicPrice = Price,
                quantity = Quantity,
            };
        }

        public ObjectItemToSell GetObjectItemToSell()
        {
            return new ObjectItemToSell()
            {
                effects = Effects.Select(x => x.GetObjectEffect()).ToArray(),
                objectGID = GId,
                objectPrice = Price,
                objectUID = UId,
                quantity = Quantity,
            };
        }
        public ObjectItemQuantityPriceDateEffects GetObjectItemQuantityPriceDateEffects()
        {
            return new ObjectItemQuantityPriceDateEffects()
            {
                date = 0,
                effects = new ObjectEffects(Effects.Select(x => x.GetObjectEffect()).ToArray()),
                objectGID = GId,
                price = Price,
                quantity = Quantity,
            };
        }
        public static int GetLastItemUID()
        {
            return (int)MerchantItems.Keys.OrderByDescending(x => x).FirstOrDefault();
        }

        public static IEnumerable<MerchantItemRecord> GetMerchantItems(long characterId, bool solded)
        {
            return GetAllMerchantItems(characterId).Where(x => x.Sold == solded);
        }
        public static IEnumerable<MerchantItemRecord> GetAllMerchantItems(long characterId)
        {
            return MerchantItems.Values.Where(x => x.CharacterId == characterId);
        }

        public override void Initialize()
        {

        }

        public static void RemoveMerchantItems(long id)
        {
            GetAllMerchantItems(id).RemoveInstantElements(typeof(MerchantItemRecord));
        }


    }
}
