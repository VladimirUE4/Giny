using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class TrustStatusMessage : NetworkMessage  
    { 
        public new const ushort Id = 2030;
        public override ushort MessageId => Id;

        public bool trusted;
        public bool certified;

        public TrustStatusMessage()
        {
        }
        public TrustStatusMessage(bool trusted,bool certified)
        {
            this.trusted = trusted;
            this.certified = certified;
        }
        public override void Serialize(IDataWriter writer)
        {
            byte _box0 = 0;
            _box0 = BooleanByteWrapper.SetFlag(_box0,0,trusted);
            _box0 = BooleanByteWrapper.SetFlag(_box0,1,certified);
            writer.WriteByte((byte)_box0);
        }
        public override void Deserialize(IDataReader reader)
        {
            byte _box0 = reader.ReadByte();
            trusted = BooleanByteWrapper.GetFlag(_box0,0);
            certified = BooleanByteWrapper.GetFlag(_box0,1);
        }


    }
}








