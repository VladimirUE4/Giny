using System;
using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class TrustCertificate  
    { 
        public const ushort Id = 8671;
        public virtual ushort TypeId => Id;

        public int id;
        public string hash;

        public TrustCertificate()
        {
        }
        public TrustCertificate(int id,string hash)
        {
            this.id = id;
            this.hash = hash;
        }
        public virtual void Serialize(IDataWriter writer)
        {
            if (id < 0)
            {
                throw new Exception("Forbidden value (" + id + ") on element id.");
            }

            writer.WriteInt((int)id);
            writer.WriteUTF((string)hash);
        }
        public virtual void Deserialize(IDataReader reader)
        {
            id = (int)reader.ReadInt();
            if (id < 0)
            {
                throw new Exception("Forbidden value (" + id + ") on element of TrustCertificate.id.");
            }

            hash = (string)reader.ReadUTF();
        }


    }
}








