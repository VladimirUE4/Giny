using System;
using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class FriendInformations : AbstractContactInformations  
    { 
        public const ushort Id = 1820;
        public override ushort TypeId => Id;

        public byte playerState;
        public short lastConnection;
        public int achievementPoints;
        public short leagueId;
        public int ladderPosition;

        public FriendInformations()
        {
        }
        public FriendInformations(byte playerState,short lastConnection,int achievementPoints,short leagueId,int ladderPosition)
        {
            this.playerState = playerState;
            this.lastConnection = lastConnection;
            this.achievementPoints = achievementPoints;
            this.leagueId = leagueId;
            this.ladderPosition = ladderPosition;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteByte((byte)playerState);
            if (lastConnection < 0)
            {
                throw new Exception("Forbidden value (" + lastConnection + ") on element lastConnection.");
            }

            writer.WriteVarShort((short)lastConnection);
            writer.WriteInt((int)achievementPoints);
            writer.WriteVarShort((short)leagueId);
            writer.WriteInt((int)ladderPosition);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            playerState = (byte)reader.ReadByte();
            if (playerState < 0)
            {
                throw new Exception("Forbidden value (" + playerState + ") on element of FriendInformations.playerState.");
            }

            lastConnection = (short)reader.ReadVarUhShort();
            if (lastConnection < 0)
            {
                throw new Exception("Forbidden value (" + lastConnection + ") on element of FriendInformations.lastConnection.");
            }

            achievementPoints = (int)reader.ReadInt();
            leagueId = (short)reader.ReadVarShort();
            ladderPosition = (int)reader.ReadInt();
        }


    }
}








