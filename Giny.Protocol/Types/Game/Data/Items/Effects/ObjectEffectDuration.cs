using System;
using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class ObjectEffectDuration : ObjectEffect  
    { 
        public const ushort Id = 5502;
        public override ushort TypeId => Id;

        public short days;
        public byte hours;
        public byte minutes;

        public ObjectEffectDuration()
        {
        }
        public ObjectEffectDuration(short days,byte hours,byte minutes)
        {
            this.days = days;
            this.hours = hours;
            this.minutes = minutes;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            if (days < 0)
            {
                throw new Exception("Forbidden value (" + days + ") on element days.");
            }

            writer.WriteVarShort((short)days);
            if (hours < 0)
            {
                throw new Exception("Forbidden value (" + hours + ") on element hours.");
            }

            writer.WriteByte((byte)hours);
            if (minutes < 0)
            {
                throw new Exception("Forbidden value (" + minutes + ") on element minutes.");
            }

            writer.WriteByte((byte)minutes);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            days = (short)reader.ReadVarUhShort();
            if (days < 0)
            {
                throw new Exception("Forbidden value (" + days + ") on element of ObjectEffectDuration.days.");
            }

            hours = (byte)reader.ReadByte();
            if (hours < 0)
            {
                throw new Exception("Forbidden value (" + hours + ") on element of ObjectEffectDuration.hours.");
            }

            minutes = (byte)reader.ReadByte();
            if (minutes < 0)
            {
                throw new Exception("Forbidden value (" + minutes + ") on element of ObjectEffectDuration.minutes.");
            }

        }


    }
}








