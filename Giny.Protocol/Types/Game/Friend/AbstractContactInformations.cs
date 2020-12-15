using System;
using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class AbstractContactInformations  
    { 
        public const ushort Id = 3761;
        public virtual ushort TypeId => Id;

        public int accountId;
        public string accountName;

        public AbstractContactInformations()
        {
        }
        public AbstractContactInformations(int accountId,string accountName)
        {
            this.accountId = accountId;
            this.accountName = accountName;
        }
        public virtual void Serialize(IDataWriter writer)
        {
            if (accountId < 0)
            {
                throw new Exception("Forbidden value (" + accountId + ") on element accountId.");
            }

            writer.WriteInt((int)accountId);
            writer.WriteUTF((string)accountName);
        }
        public virtual void Deserialize(IDataReader reader)
        {
            accountId = (int)reader.ReadInt();
            if (accountId < 0)
            {
                throw new Exception("Forbidden value (" + accountId + ") on element of AbstractContactInformations.accountId.");
            }

            accountName = (string)reader.ReadUTF();
        }


    }
}








