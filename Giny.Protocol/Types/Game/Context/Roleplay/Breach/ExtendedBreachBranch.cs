using System;
using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class ExtendedBreachBranch : BreachBranch  
    { 
        public const ushort Id = 9376;
        public override ushort TypeId => Id;

        public BreachReward[] rewards;
        public int modifier;
        public int prize;

        public ExtendedBreachBranch()
        {
        }
        public ExtendedBreachBranch(BreachReward[] rewards,int modifier,int prize)
        {
            this.rewards = rewards;
            this.modifier = modifier;
            this.prize = prize;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteShort((short)rewards.Length);
            for (uint _i1 = 0;_i1 < rewards.Length;_i1++)
            {
                (rewards[_i1] as BreachReward).Serialize(writer);
            }

            writer.WriteVarInt((int)modifier);
            if (prize < 0)
            {
                throw new Exception("Forbidden value (" + prize + ") on element prize.");
            }

            writer.WriteVarInt((int)prize);
        }
        public override void Deserialize(IDataReader reader)
        {
            BreachReward _item1 = null;
            base.Deserialize(reader);
            uint _rewardsLen = (uint)reader.ReadUShort();
            for (uint _i1 = 0;_i1 < _rewardsLen;_i1++)
            {
                _item1 = new BreachReward();
                _item1.Deserialize(reader);
                rewards[_i1] = _item1;
            }

            modifier = (int)reader.ReadVarInt();
            prize = (int)reader.ReadVarUhInt();
            if (prize < 0)
            {
                throw new Exception("Forbidden value (" + prize + ") on element of ExtendedBreachBranch.prize.");
            }

        }


    }
}








