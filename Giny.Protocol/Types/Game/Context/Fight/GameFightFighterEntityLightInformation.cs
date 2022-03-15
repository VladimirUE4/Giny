using System;
using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class GameFightFighterEntityLightInformation : GameFightFighterLightInformations  
    { 
        public const ushort Id = 4268;
        public override ushort TypeId => Id;

        public byte entityModelId;
        public double masterId;

        public GameFightFighterEntityLightInformation()
        {
        }
        public GameFightFighterEntityLightInformation(byte entityModelId,double masterId)
        {
            this.entityModelId = entityModelId;
            this.masterId = masterId;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            if (entityModelId < 0)
            {
                throw new Exception("Forbidden value (" + entityModelId + ") on element entityModelId.");
            }

            writer.WriteByte((byte)entityModelId);
            if (masterId < -9.00719925474099E+15 || masterId > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + masterId + ") on element masterId.");
            }

            writer.WriteDouble((double)masterId);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            entityModelId = (byte)reader.ReadByte();
            if (entityModelId < 0)
            {
                throw new Exception("Forbidden value (" + entityModelId + ") on element of GameFightFighterEntityLightInformation.entityModelId.");
            }

            masterId = (double)reader.ReadDouble();
            if (masterId < -9.00719925474099E+15 || masterId > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + masterId + ") on element of GameFightFighterEntityLightInformation.masterId.");
            }

        }


    }
}








