using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class HouseSellingUpdateMessage : NetworkMessage  
    { 
        public new const ushort Id = 5119;
        public override ushort MessageId => Id;

        public int houseId;
        public int instanceId;
        public bool secondHand;
        public long realPrice;
        public string buyerName;

        public HouseSellingUpdateMessage()
        {
        }
        public HouseSellingUpdateMessage(int houseId,int instanceId,bool secondHand,long realPrice,string buyerName)
        {
            this.houseId = houseId;
            this.instanceId = instanceId;
            this.secondHand = secondHand;
            this.realPrice = realPrice;
            this.buyerName = buyerName;
        }
        public override void Serialize(IDataWriter writer)
        {
            if (houseId < 0)
            {
                throw new Exception("Forbidden value (" + houseId + ") on element houseId.");
            }

            writer.WriteVarInt((int)houseId);
            if (instanceId < 0)
            {
                throw new Exception("Forbidden value (" + instanceId + ") on element instanceId.");
            }

            writer.WriteInt((int)instanceId);
            writer.WriteBoolean((bool)secondHand);
            if (realPrice < 0 || realPrice > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + realPrice + ") on element realPrice.");
            }

            writer.WriteVarLong((long)realPrice);
            writer.WriteUTF((string)buyerName);
        }
        public override void Deserialize(IDataReader reader)
        {
            houseId = (int)reader.ReadVarUhInt();
            if (houseId < 0)
            {
                throw new Exception("Forbidden value (" + houseId + ") on element of HouseSellingUpdateMessage.houseId.");
            }

            instanceId = (int)reader.ReadInt();
            if (instanceId < 0)
            {
                throw new Exception("Forbidden value (" + instanceId + ") on element of HouseSellingUpdateMessage.instanceId.");
            }

            secondHand = (bool)reader.ReadBoolean();
            realPrice = (long)reader.ReadVarUhLong();
            if (realPrice < 0 || realPrice > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + realPrice + ") on element of HouseSellingUpdateMessage.realPrice.");
            }

            buyerName = (string)reader.ReadUTF();
        }


    }
}








