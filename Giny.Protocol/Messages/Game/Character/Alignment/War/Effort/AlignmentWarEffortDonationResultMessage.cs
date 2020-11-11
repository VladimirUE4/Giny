using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class AlignmentWarEffortDonationResultMessage : NetworkMessage  
    { 
        public new const ushort Id = 6877;
        public override ushort MessageId => Id;

        public byte result;

        public AlignmentWarEffortDonationResultMessage()
        {
        }
        public AlignmentWarEffortDonationResultMessage(byte result)
        {
            this.result = result;
        }
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteByte((byte)result);
        }
        public override void Deserialize(IDataReader reader)
        {
            result = (byte)reader.ReadByte();
        }


    }
}








