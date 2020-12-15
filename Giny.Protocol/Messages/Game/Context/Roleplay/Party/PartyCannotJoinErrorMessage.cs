using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class PartyCannotJoinErrorMessage : AbstractPartyMessage  
    { 
        public new const ushort Id = 232;
        public override ushort MessageId => Id;

        public byte reason;

        public PartyCannotJoinErrorMessage()
        {
        }
        public PartyCannotJoinErrorMessage(byte reason)
        {
            this.reason = reason;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteByte((byte)reason);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            reason = (byte)reader.ReadByte();
            if (reason < 0)
            {
                throw new Exception("Forbidden value (" + reason + ") on element of PartyCannotJoinExceptionMessage.reason.");
            }

        }


    }
}








