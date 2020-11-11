using System;
using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class BasicNamedAllianceInformations : BasicAllianceInformations  
    { 
        public const ushort Id = 3494;
        public override ushort TypeId => Id;

        public string allianceName;

        public BasicNamedAllianceInformations()
        {
        }
        public BasicNamedAllianceInformations(string allianceName)
        {
            this.allianceName = allianceName;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteUTF((string)allianceName);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            allianceName = (string)reader.ReadUTF();
        }


    }
}








