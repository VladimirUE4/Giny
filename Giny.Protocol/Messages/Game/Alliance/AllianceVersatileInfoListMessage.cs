using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class AllianceVersatileInfoListMessage : NetworkMessage  
    { 
        public new const ushort Id = 1735;
        public override ushort MessageId => Id;

        public AllianceVersatileInformations[] alliances;

        public AllianceVersatileInfoListMessage()
        {
        }
        public AllianceVersatileInfoListMessage(AllianceVersatileInformations[] alliances)
        {
            this.alliances = alliances;
        }
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteShort((short)alliances.Length);
            for (uint _i1 = 0;_i1 < alliances.Length;_i1++)
            {
                (alliances[_i1] as AllianceVersatileInformations).Serialize(writer);
            }

        }
        public override void Deserialize(IDataReader reader)
        {
            AllianceVersatileInformations _item1 = null;
            uint _alliancesLen = (uint)reader.ReadUShort();
            for (uint _i1 = 0;_i1 < _alliancesLen;_i1++)
            {
                _item1 = new AllianceVersatileInformations();
                _item1.Deserialize(reader);
                alliances[_i1] = _item1;
            }

        }


    }
}








