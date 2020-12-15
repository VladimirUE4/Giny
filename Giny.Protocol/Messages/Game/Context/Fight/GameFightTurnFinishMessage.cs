using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class GameFightTurnFinishMessage : NetworkMessage  
    { 
        public new const ushort Id = 9394;
        public override ushort MessageId => Id;

        public bool isAfk;

        public GameFightTurnFinishMessage()
        {
        }
        public GameFightTurnFinishMessage(bool isAfk)
        {
            this.isAfk = isAfk;
        }
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteBoolean((bool)isAfk);
        }
        public override void Deserialize(IDataReader reader)
        {
            isAfk = (bool)reader.ReadBoolean();
        }


    }
}








