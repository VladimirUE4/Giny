using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class IgnoredDeleteResultMessage : NetworkMessage  
    { 
        public new const ushort Id = 7050;
        public override ushort MessageId => Id;

        public bool success;
        public string name;
        public bool session;

        public IgnoredDeleteResultMessage()
        {
        }
        public IgnoredDeleteResultMessage(bool success,string name,bool session)
        {
            this.success = success;
            this.name = name;
            this.session = session;
        }
        public override void Serialize(IDataWriter writer)
        {
            byte _box0 = 0;
            _box0 = BooleanByteWrapper.SetFlag(_box0,0,success);
            _box0 = BooleanByteWrapper.SetFlag(_box0,1,session);
            writer.WriteByte((byte)_box0);
            writer.WriteUTF((string)name);
        }
        public override void Deserialize(IDataReader reader)
        {
            byte _box0 = reader.ReadByte();
            success = BooleanByteWrapper.GetFlag(_box0,0);
            session = BooleanByteWrapper.GetFlag(_box0,1);
            name = (string)reader.ReadUTF();
        }


    }
}








