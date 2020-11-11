using System;
using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class GameRolePlayNpcWithQuestInformations : GameRolePlayNpcInformations  
    { 
        public const ushort Id = 784;
        public override ushort TypeId => Id;

        public GameRolePlayNpcQuestFlag questFlag;

        public GameRolePlayNpcWithQuestInformations()
        {
        }
        public GameRolePlayNpcWithQuestInformations(GameRolePlayNpcQuestFlag questFlag)
        {
            this.questFlag = questFlag;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            questFlag.Serialize(writer);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            questFlag = new GameRolePlayNpcQuestFlag();
            questFlag.Deserialize(reader);
        }


    }
}








