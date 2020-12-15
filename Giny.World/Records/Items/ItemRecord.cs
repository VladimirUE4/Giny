﻿using Giny.Core.DesignPattern;
using Giny.IO.D2O;
using Giny.ORM.Attributes;
using Giny.ORM.Interfaces;
using Giny.Protocol.Custom.Enums;
using Giny.Protocol.Enums;
using Giny.Protocol.Types;
using Giny.World.Managers.Effects;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Records.Items
{
    [D2OClass("Item")]
    [Table("items")]
    public class ItemRecord : ITable
    {
        private static readonly ConcurrentDictionary<long, ItemRecord> Items = new ConcurrentDictionary<long, ItemRecord>();

        [D2OField("id")]
        [Primary]
        public long Id
        {
            get;
            set;
        }
        [I18NField]
        [D2OField("nameId")]
        public string Name
        {
            get;
            set;
        }
        [D2OField("typeId")]
        public short TypeId
        {
            get;
            set;
        }
        [D2OField("level")]
        public short Level
        {
            get;
            set;
        }
        [D2OField("realWeight")]
        public int RealWeight
        {
            get;
            set;
        }
        [D2OField("cursed")]
        public bool Cursed
        {
            get;
            set;
        }
        [D2OField("usable")]
        public bool Usable
        {
            get;
            set;
        }
        [D2OField("exchangeable")]
        public bool Exchangeable
        {
            get;
            set;
        }

        [D2OField("price")]
        public double Price
        {
            get;
            set;
        }
        [D2OField("etheral")]
        public bool Etheral
        {
            get;
            set;
        }
        [D2OField("itemSetId")]
        public int ItemSetId
        {
            get;
            set;
        }
        [D2OField("criteria")]
        public string Criteria
        {
            get;
            set;
        }
        [Update]
        [D2OField("appearanceId")]
        public short AppearenceId
        {
            get;
            set;
        }
        [ProtoSerialize]
        [D2OField("dropMonsterIds")]
        public short[] DropMonsterIds
        {
            get;
            set;
        }
        [D2OField("recipeSlots")]
        public int RecipeSlots
        {
            get;
            set;
        }
        [D2OField("recipeIds")]
        [ProtoSerialize]
        public uint[] RecipeIds
        {
            get;
            set;
        }
        [ProtoSerialize]
        [D2OField("possibleEffects")]
        public Effect[] Effects
        {
            get;
            set;
        }
        [D2OField("craftXpRatio")]
        public int CraftXpRatio
        {
            get;
            set;
        }
        [D2OField("isSaleable")]
        public bool IsSaleable
        {
            get;
            set;
        }
        [Update]
        public string Look
        {
            get;
            set;
        }
        [Ignore]
        public ItemTypeEnum TypeEnum
        {
            get
            {
                return (ItemTypeEnum)TypeId;
            }
        }

        public ObjectItemToSellInNpcShop GetObjectItemToSellInNpcShop()
        {
            return new ObjectItemToSellInNpcShop()
            {
                buyCriterion = string.Empty,
                effects = Effects.Select(x => x.GetObjectEffect()).ToArray(),
                objectGID = (short)Id,
                objectPrice = (long)Price,
            };
        }
       
        public static IEnumerable<ItemRecord> GetItems()
        {
            return Items.Values;
        }
        public static void Add(ItemRecord item)
        {
            if (!Items.ContainsKey(item.Id))
            {
                Items[item.Id] = item;
            }
        }
        public static bool ItemExists(int gid)
        {
            return Items.ContainsKey(gid);
        }
        public static ItemRecord GetItem(int gid)
        {
            return Items[gid];
        }

        public override string ToString()
        {
            return "(" + Id + ") " + Name;
        }

    }
}
