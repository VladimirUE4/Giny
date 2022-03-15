using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class GameFightResumeWithSlavesMessage : GameFightResumeMessage  
    { 
        public  const ushort Id = 6205;
        public override ushort MessageId => Id;

        public GameFightResumeSlaveInfo[] slavesInfo;

        public GameFightResumeWithSlavesMessage()
        {
        }
        public GameFightResumeWithSlavesMessage(GameFightResumeSlaveInfo[] slavesInfo)
        {
            this.slavesInfo = slavesInfo;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteShort((short)slavesInfo.Length);
            for (uint _i1 = 0;_i1 < slavesInfo.Length;_i1++)
            {
                (slavesInfo[_i1] as GameFightResumeSlaveInfo).Serialize(writer);
            }

        }
        public override void Deserialize(IDataReader reader)
        {
            GameFightResumeSlaveInfo _item1 = null;
            base.Deserialize(reader);
            uint _slavesInfoLen = (uint)reader.ReadUShort();
            for (uint _i1 = 0;_i1 < _slavesInfoLen;_i1++)
            {
                _item1 = new GameFightResumeSlaveInfo();
                _item1.Deserialize(reader);
                slavesInfo[_i1] = _item1;
            }

        }


    }
}








