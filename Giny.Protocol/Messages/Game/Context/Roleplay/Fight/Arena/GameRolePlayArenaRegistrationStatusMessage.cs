using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class GameRolePlayArenaRegistrationStatusMessage : NetworkMessage  
    { 
        public new const ushort Id = 7549;
        public override ushort MessageId => Id;

        public bool registered;
        public byte step;
        public int battleMode;

        public GameRolePlayArenaRegistrationStatusMessage()
        {
        }
        public GameRolePlayArenaRegistrationStatusMessage(bool registered,byte step,int battleMode)
        {
            this.registered = registered;
            this.step = step;
            this.battleMode = battleMode;
        }
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteBoolean((bool)registered);
            writer.WriteByte((byte)step);
            writer.WriteInt((int)battleMode);
        }
        public override void Deserialize(IDataReader reader)
        {
            registered = (bool)reader.ReadBoolean();
            step = (byte)reader.ReadByte();
            if (step < 0)
            {
                throw new Exception("Forbidden value (" + step + ") on element of GameRolePlayArenaRegistrationStatusMessage.step.");
            }

            battleMode = (int)reader.ReadInt();
            if (battleMode < 0)
            {
                throw new Exception("Forbidden value (" + battleMode + ") on element of GameRolePlayArenaRegistrationStatusMessage.battleMode.");
            }

        }


    }
}








