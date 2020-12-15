using System;
using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class ActorExtendedAlignmentInformations : ActorAlignmentInformations  
    { 
        public const ushort Id = 6118;
        public override ushort TypeId => Id;

        public short honor;
        public short honorGradeFloor;
        public short honorNextGradeFloor;
        public byte aggressable;

        public ActorExtendedAlignmentInformations()
        {
        }
        public ActorExtendedAlignmentInformations(short honor,short honorGradeFloor,short honorNextGradeFloor,byte aggressable)
        {
            this.honor = honor;
            this.honorGradeFloor = honorGradeFloor;
            this.honorNextGradeFloor = honorNextGradeFloor;
            this.aggressable = aggressable;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            if (honor < 0 || honor > 20000)
            {
                throw new Exception("Forbidden value (" + honor + ") on element honor.");
            }

            writer.WriteVarShort((short)honor);
            if (honorGradeFloor < 0 || honorGradeFloor > 20000)
            {
                throw new Exception("Forbidden value (" + honorGradeFloor + ") on element honorGradeFloor.");
            }

            writer.WriteVarShort((short)honorGradeFloor);
            if (honorNextGradeFloor < 0 || honorNextGradeFloor > 20000)
            {
                throw new Exception("Forbidden value (" + honorNextGradeFloor + ") on element honorNextGradeFloor.");
            }

            writer.WriteVarShort((short)honorNextGradeFloor);
            writer.WriteByte((byte)aggressable);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            honor = (short)reader.ReadVarUhShort();
            if (honor < 0 || honor > 20000)
            {
                throw new Exception("Forbidden value (" + honor + ") on element of ActorExtendedAlignmentInformations.honor.");
            }

            honorGradeFloor = (short)reader.ReadVarUhShort();
            if (honorGradeFloor < 0 || honorGradeFloor > 20000)
            {
                throw new Exception("Forbidden value (" + honorGradeFloor + ") on element of ActorExtendedAlignmentInformations.honorGradeFloor.");
            }

            honorNextGradeFloor = (short)reader.ReadVarUhShort();
            if (honorNextGradeFloor < 0 || honorNextGradeFloor > 20000)
            {
                throw new Exception("Forbidden value (" + honorNextGradeFloor + ") on element of ActorExtendedAlignmentInformations.honorNextGradeFloor.");
            }

            aggressable = (byte)reader.ReadByte();
            if (aggressable < 0)
            {
                throw new Exception("Forbidden value (" + aggressable + ") on element of ActorExtendedAlignmentInformations.aggressable.");
            }

        }


    }
}








