using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class InteractiveUseErrorMessage : NetworkMessage  
    { 
        public new const ushort Id = 9202;
        public override ushort MessageId => Id;

        public int elemId;
        public int skillInstanceUid;

        public InteractiveUseErrorMessage()
        {
        }
        public InteractiveUseErrorMessage(int elemId,int skillInstanceUid)
        {
            this.elemId = elemId;
            this.skillInstanceUid = skillInstanceUid;
        }
        public override void Serialize(IDataWriter writer)
        {
            if (elemId < 0)
            {
                throw new Exception("Forbidden value (" + elemId + ") on element elemId.");
            }

            writer.WriteVarInt((int)elemId);
            if (skillInstanceUid < 0)
            {
                throw new Exception("Forbidden value (" + skillInstanceUid + ") on element skillInstanceUid.");
            }

            writer.WriteVarInt((int)skillInstanceUid);
        }
        public override void Deserialize(IDataReader reader)
        {
            elemId = (int)reader.ReadVarUhInt();
            if (elemId < 0)
            {
                throw new Exception("Forbidden value (" + elemId + ") on element of InteractiveUseExceptionMessage.elemId.");
            }

            skillInstanceUid = (int)reader.ReadVarUhInt();
            if (skillInstanceUid < 0)
            {
                throw new Exception("Forbidden value (" + skillInstanceUid + ") on element of InteractiveUseExceptionMessage.skillInstanceUid.");
            }

        }


    }
}








