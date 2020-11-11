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
                Effects = this.CloneEffects(),
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
                Effects = this.CloneEffects(),
                GId = GId,
                Look = Look,
                Position = this.Position,
                Quantity = this.Quantity,
                UId = UId,
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

        public static int GetLastItemUID()
        {
            return (int)MerchantItems.Keys.OrderByDescending(x => x).FirstOrDefault();
        }

        public static IEnumerable<MerchantItemRecord> GetMerchantItems(long characterId)
        {
            return MerchantItems.Values.Where(x => x.CharacterId == characterId);
        }

        public override void Initialize()
        {

        }

        public static void RemoveMerchantItems(long id)
        {
            GetMerchantItems(id).RemoveInstantElements(typeof(MerchantItemRecord));
        }
    }
}
