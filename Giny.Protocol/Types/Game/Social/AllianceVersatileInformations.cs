using System.Collections.Generic;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Types
{ 
    public class AllianceVersatileInformations  
    { 
        public const ushort Id = 1207;
        public virtual ushort TypeId => Id;

        public int allianceId;
        public short nbGuilds;
        public short nbMembers;
        public short nbSubarea;

        public AllianceVersatileInformations()
        {
        }
        public AllianceVersatileInformations(int allianceId,short nbGuilds,short nbMembers,short nbSubarea)
        {
            this.allianceId = allianceId;
            this.nbGuilds = nbGuilds;
            this.nbMembers = nbMembers;
            this.nbSubarea = nbSubarea;
        }
        public virtual void Serialize(IDataWriter writer)
        {
            if (allianceId < 0)
            {
                throw new System.Exception("Forbidden value (" + allianceId + ") on element allianceId.");
            }

            writer.WriteVarInt((int)allianceId);
            if (nbGuilds < 0)
            {
                throw new System.Exception("Forbidden value (" + nbGuilds + ") on element nbGuilds.");
            }

            writer.WriteVarShort((short)nbGuilds);
            if (nbMembers < 0)
            {
                throw new System.Exception("Forbidden value (" + nbMembers + ") on element nbMembers.");
            }

            writer.WriteVarShort((short)nbMembers);
            if (nbSubarea < 0)
            {
                throw new System.Exception("Forbidden value (" + nbSubarea + ") on element nbSubarea.");
            }

            writer.WriteVarShort((short)nbSubarea);
        }
        public virtual void Deserialize(IDataReader reader)
        {
            allianceId = (int)reader.ReadVarUhInt();
            if (allianceId < 0)
            {
                throw new System.Exception("Forbidden value (" + allianceId + ") on element of AllianceVersatileInformations.allianceId.");
            }

            nbGuilds = (short)reader.ReadVarUhShort();
            if (nbGuilds < 0)
            {
                throw new System.Exception("Forbidden value (" + nbGuilds + ") on element of AllianceVersatileInformations.nbGuilds.");
            }

            nbMembers = (short)reader.ReadVarUhShort();
            if (nbMembers < 0)
            {
                throw new System.Exception("Forbidden value (" + nbMembers + ") on element of AllianceVersatileInformations.nbMembers.");
            }

            nbSubarea = (short)reader.ReadVarUhShort();
            if (nbSubarea < 0)
            {
                throw new System.Exception("Forbidden value (" + nbSubarea + ") on element of AllianceVersatileInformations.nbSubarea.");
            }

        }


    }
}








