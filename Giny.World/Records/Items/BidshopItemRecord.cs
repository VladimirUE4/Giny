﻿using Giny.ORM.Attributes;
using Giny.ORM.Interfaces;
using Giny.Protocol.Types;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Records.Items
{
    [Table("BidshopItems")]
    public class BidShopItemRecord : AbstractItem, ITable
    {
        private static readonly ConcurrentDictionary<long, BidShopItemRecord> BidshopItems = new ConcurrentDictionary<long, BidShopItemRecord>();

        [Ignore]
        public long Id => UId;

        public long BidShopId
        {
            get;
            set;
        }
        public int AccountId
        {
            get;
            set;
        }
        [Update]
        public long Price
        {
            get;
            set;
        }
        [Update]
        public bool Sold
        {
            get;
            set;
        }
        public override AbstractItem CloneWithoutUID()
        {
            return new BidShopItemRecord()
            {
                BidShopId = BidShopId,
                AccountId = this.AccountId,
                Price = Price,
                AppearanceId = this.AppearanceId,
                Effects = this.Effects.Clone(),
                GId = GId,
                Look = Look,
                Position = this.Position,
                Quantity = this.Quantity,
                UId = this.UId,
            };
        }

        public override AbstractItem CloneWithUID()
        {
            return new BidShopItemRecord()
            {
                BidShopId = BidShopId,
                AccountId = this.AccountId,
                Price = Price,
                AppearanceId = this.AppearanceId,
                Effects = this.Effects.Clone(),
                GId = GId,
                Look = Look,
                Position = this.Position,
                Quantity = this.Quantity,
                UId = this.UId,
            };

        }

        public static int GetLastItemUID()
        {
            return (int)BidshopItems.Keys.OrderByDescending(x => x).FirstOrDefault();
        }

        public BidExchangerObjectInfo GetBidExchangerObjectInfo(long[] prices)
        {
            return new BidExchangerObjectInfo()
            {
                effects = Effects.Select(x => x.GetObjectEffect()).ToArray(),
                objectUID = UId,
                prices = prices,
                objectGID = GId,
                objectType = (int)Record.TypeEnum,
            };
        }
        public ObjectItemQuantityPriceDateEffects GetObjectItemQuantityPriceDateEffects()
        {
            return new ObjectItemQuantityPriceDateEffects()
            {
                objectGID = GId,
                price = Price,
                quantity = Quantity,
                date = 19999,
                effects = new ObjectEffects(Effects.Select(x => x.GetObjectEffect()).ToArray()),
            };
        }
        public static IEnumerable<BidShopItemRecord> GetItems()
        {
            return BidshopItems.Values;
        }

        public ObjectItemToSellInBid GetObjectItemToSellInBid()
        {
            return new ObjectItemToSellInBid()
            {
                effects = Effects.Select(x => x.GetObjectEffect()).ToArray(),
                objectGID = GId,
                objectPrice = Price,
                objectUID = UId,
                quantity = Quantity,
                unsoldDelay = 0, /* ??? */
            };
        }

        public override void Initialize()
        {

        }
    }
}
