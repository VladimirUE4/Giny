using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class CharactersListWithRemodelingMessage : CharactersListMessage  
    { 
        public  const ushort Id = 3532;
        public override ushort MessageId => Id;

        public CharacterToRemodelInformations[] charactersToRemodel;

        public CharactersListWithRemodelingMessage()
        {
        }
        public CharactersListWithRemodelingMessage(CharacterToRemodelInformations[] charactersToRemodel)
        {
            this.charactersToRemodel = charactersToRemodel;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteShort((short)charactersToRemodel.Length);
            for (uint _i1 = 0;_i1 < charactersToRemodel.Length;_i1++)
            {
                (charactersToRemodel[_i1] as CharacterToRemodelInformations).Serialize(writer);
            }

        }
        public override void Deserialize(IDataReader reader)
        {
            CharacterToRemodelInformations _item1 = null;
            base.Deserialize(reader);
            uint _charactersToRemodelLen = (uint)reader.ReadUShort();
            for (uint _i1 = 0;_i1 < _charactersToRemodelLen;_i1++)
            {
                _item1 = new CharacterToRemodelInformations();
                _item1.Deserialize(reader);
                charactersToRemodel[_i1] = _item1;
            }

        }


    }
}








