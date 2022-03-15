using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class CharacterReplayWithRemodelRequestMessage : CharacterReplayRequestMessage  
    { 
        public  const ushort Id = 3832;
        public override ushort MessageId => Id;

        public RemodelingInformation remodel;

        public CharacterReplayWithRemodelRequestMessage()
        {
        }
        public CharacterReplayWithRemodelRequestMessage(RemodelingInformation remodel)
        {
            this.remodel = remodel;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            remodel.Serialize(writer);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            remodel = new RemodelingInformation();
            remodel.Deserialize(reader);
        }


    }
}








