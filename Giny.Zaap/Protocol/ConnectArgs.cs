﻿using Giny.Core.IO;
using Giny.Zaap.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.Zaap.Protocol
{
    public class ConnectArgs : ZaapMessage
    {
        public enum TFieldId
        {
            GAMENAME = 1,
            RELEASENAME = 2,
            INSTANCEID = 3,
            HASH = 4,
        }

        private string GameName;

        private string ReleaseName;

        private int InstanceId;

        private string Hash;

        public ConnectArgs(TMessage tMessage) : base(tMessage)
        {

        }

        public override void Serialize(TProtocol protocol, BigEndianWriter writer)
        {
            throw new NotImplementedException();
        }

        public override void Deserialize(TProtocol protocol, BigEndianReader reader)
        {
            while (true)
            {
                var field = protocol.ReadFieldBegin(reader);

                if (field.Type == TType.STOP)
                {
                    break;
                }
                switch ((TFieldId)field.Id)
                {
                    case TFieldId.GAMENAME:
                        this.GameName = reader.ReadUTF7BitLength();
                        break;
                    case TFieldId.RELEASENAME:
                        this.ReleaseName = reader.ReadUTF7BitLength();
                        break;
                    case TFieldId.INSTANCEID:
                        this.InstanceId = reader.ReadInt();
                        break;
                    case TFieldId.HASH:
                        this.Hash = reader.ReadUTF7BitLength();
                        break;
                    default:
                        break;
                }
            }
        }
        public override string ToString()
        {
            return GameName + "," + ReleaseName;
        }

    }
}
