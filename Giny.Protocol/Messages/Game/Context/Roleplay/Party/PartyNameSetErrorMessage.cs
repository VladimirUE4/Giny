using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class PartyNameSetErrorMessage : AbstractPartyMessage  
    { 
        public new const ushort Id = 6623;
        public override ushort MessageId => Id;

        public byte result;

        public PartyNameSetErrorMessage()
        {
        }
        public PartyNameSetErrorMessage(byte result)
        {
            this.result = result;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteByte((byte)result);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            result = (byte)reader.ReadByte();
            if (result < 0)
            {
                throw new Exception("Forbidden value (" + result + ") on element of PartyNameSetExceptionMessage.result.");
            }

        }


    }
}








