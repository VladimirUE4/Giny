using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class ChatClientMultiWithObjectMessage : ChatClientMultiMessage  
    { 
        public  const ushort Id = 1682;
        public override ushort MessageId => Id;

        public ObjectItem[] objects;

        public ChatClientMultiWithObjectMessage()
        {
        }
        public ChatClientMultiWithObjectMessage(ObjectItem[] objects,string content,byte channel)
        {
            this.objects = objects;
            this.content = content;
            this.channel = channel;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteShort((short)objects.Length);
            for (uint _i1 = 0;_i1 < objects.Length;_i1++)
            {
                (objects[_i1] as ObjectItem).Serialize(writer);
            }

        }
        public override void Deserialize(IDataReader reader)
        {
            ObjectItem _item1 = null;
            base.Deserialize(reader);
            uint _objectsLen = (uint)reader.ReadUShort();
            for (uint _i1 = 0;_i1 < _objectsLen;_i1++)
            {
                _item1 = new ObjectItem();
                _item1.Deserialize(reader);
                objects[_i1] = _item1;
            }

        }


    }
}








