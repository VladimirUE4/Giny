using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class ServerOptionalFeaturesMessage : NetworkMessage  
    { 
        public new const ushort Id = 2305;
        public override ushort MessageId => Id;

        public byte[] features;

        public ServerOptionalFeaturesMessage()
        {
        }
        public ServerOptionalFeaturesMessage(byte[] features)
        {
            this.features = features;
        }
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteShort((short)features.Length);
            for (uint _i1 = 0;_i1 < features.Length;_i1++)
            {
                if (features[_i1] < 0)
                {
                    throw new Exception("Forbidden value (" + features[_i1] + ") on element 1 (starting at 1) of features.");
                }

                writer.WriteByte((byte)features[_i1]);
            }

        }
        public override void Deserialize(IDataReader reader)
        {
            uint _val1 = 0;
            uint _featuresLen = (uint)reader.ReadUShort();
            features = new byte[_featuresLen];
            for (uint _i1 = 0;_i1 < _featuresLen;_i1++)
            {
                _val1 = (uint)reader.ReadByte();
                if (_val1 < 0)
                {
                    throw new Exception("Forbidden value (" + _val1 + ") on elements of features.");
                }

                features[_i1] = (byte)_val1;
            }

        }


    }
}








