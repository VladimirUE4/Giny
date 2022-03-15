using System;
using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class PaddockItem : ObjectItemInRolePlay  
    { 
        public const ushort Id = 5575;
        public override ushort TypeId => Id;

        public ItemDurability durability;

        public PaddockItem()
        {
        }
        public PaddockItem(ItemDurability durability)
        {
            this.durability = durability;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            durability.Serialize(writer);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            durability = new ItemDurability();
            durability.Deserialize(reader);
        }


    }
}








