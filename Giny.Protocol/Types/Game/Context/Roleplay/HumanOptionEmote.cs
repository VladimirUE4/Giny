using System;
using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class HumanOptionEmote : HumanOption  
    { 
        public const ushort Id = 4201;
        public override ushort TypeId => Id;

        public byte emoteId;
        public double emoteStartTime;

        public HumanOptionEmote()
        {
        }
        public HumanOptionEmote(byte emoteId,double emoteStartTime)
        {
            this.emoteId = emoteId;
            this.emoteStartTime = emoteStartTime;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            if (emoteId < 0 || emoteId > 255)
            {
                throw new Exception("Forbidden value (" + emoteId + ") on element emoteId.");
            }

            writer.WriteByte((byte)emoteId);
            if (emoteStartTime < -9.00719925474099E+15 || emoteStartTime > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + emoteStartTime + ") on element emoteStartTime.");
            }

            writer.WriteDouble((double)emoteStartTime);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            emoteId = (byte)reader.ReadSByte();
            if (emoteId < 0 || emoteId > 255)
            {
                throw new Exception("Forbidden value (" + emoteId + ") on element of HumanOptionEmote.emoteId.");
            }

            emoteStartTime = (double)reader.ReadDouble();
            if (emoteStartTime < -9.00719925474099E+15 || emoteStartTime > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + emoteStartTime + ") on element of HumanOptionEmote.emoteStartTime.");
            }

        }


    }
}








