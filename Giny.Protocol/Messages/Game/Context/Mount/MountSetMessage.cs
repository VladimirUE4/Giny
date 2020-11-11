using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class MountSetMessage : NetworkMessage  
    { 
        public new const ushort Id = 8199;
        public override ushort MessageId => Id;

        public MountClientData mountData;

        public MountSetMessage()
        {
        }
        public MountSetMessage(MountClientData mountData)
        {
            this.mountData = mountData;
        }
        public override void Serialize(IDataWriter writer)
        {
            mountData.Serialize(writer);
        }
        public override void Deserialize(IDataReader reader)
        {
            mountData = new MountClientData();
            mountData.Deserialize(reader);
        }


    }
}








