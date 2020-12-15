using System;
using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class GameFightFighterNamedInformations : GameFightFighterInformations  
    { 
        public const ushort Id = 5594;
        public override ushort TypeId => Id;

        public string name;
        public PlayerStatus status;
        public short leagueId;
        public int ladderPosition;
        public bool hiddenInPrefight;

        public GameFightFighterNamedInformations()
        {
        }
        public GameFightFighterNamedInformations(string name,PlayerStatus status,short leagueId,int ladderPosition,bool hiddenInPrefight)
        {
            this.name = name;
            this.status = status;
            this.leagueId = leagueId;
            this.ladderPosition = ladderPosition;
            this.hiddenInPrefight = hiddenInPrefight;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteUTF((string)name);
            status.Serialize(writer);
            writer.WriteVarShort((short)leagueId);
            writer.WriteInt((int)ladderPosition);
            writer.WriteBoolean((bool)hiddenInPrefight);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            name = (string)reader.ReadUTF();
            status = new PlayerStatus();
            status.Deserialize(reader);
            leagueId = (short)reader.ReadVarShort();
            ladderPosition = (int)reader.ReadInt();
            hiddenInPrefight = (bool)reader.ReadBoolean();
        }


    }
}








