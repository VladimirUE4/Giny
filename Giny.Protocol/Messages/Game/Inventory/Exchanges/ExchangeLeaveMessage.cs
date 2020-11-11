using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class ExchangeLeaveMessage : LeaveDialogMessage  
    { 
        public new const ushort Id = 279;
        public override ushort MessageId => Id;

        public bool success;

        public ExchangeLeaveMessage()
        {
        }
        public ExchangeLeaveMessage(bool success)
        {
            this.success = success;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteBoolean((bool)success);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            success = (bool)reader.ReadBoolean();
        }


    }
}








