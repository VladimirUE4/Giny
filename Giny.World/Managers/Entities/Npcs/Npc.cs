using Giny.ORM;
using Giny.Protocol.Custom.Enums;
using Giny.Protocol.Types;
using Giny.World.Managers.Entities.Characters;
using Giny.World.Managers.Entities.Look;
using Giny.World.Managers.Maps.Npcs;
using Giny.World.Records.Maps;
using Giny.World.Records.Npcs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Entities.Npcs
{
    public class Npc : Entity
    {
        public override string Name
        {
            get
            {
                return Template.Name;
            }
        }
        public NpcSpawnRecord SpawnRecord
        {
            get;
            set;
        }
        public NpcRecord Template
        {
            get
            {
                return SpawnRecord.Template;
            }
        }

        //s public List<NpcActionRecord> ActionsRecord = new List<NpcActionRecord>();

        private long m_Id;

        public override long Id
        {
            get
            {
                return m_Id;
            }
        }

        public override short CellId
        {
            get
            {
                return SpawnRecord.CellId;
            }
            set
            {
                SpawnRecord.CellId = value;
                SpawnRecord.UpdateInstantElement();
            }
        }

        public override DirectionsEnum Direction
        {
            get
            {
                return SpawnRecord.Direction;
            }
            set
            {
                SpawnRecord.Direction = value;
                SpawnRecord.UpdateInstantElement();
            }
        }

        public override ServerEntityLook Look
        {
            get
            {
                return Template.Look;
            }
            set
            {
                throw new NotSupportedException("not authorized.");
            }
        }

        public Npc(NpcSpawnRecord spawnRecord, MapRecord map) : base(map)
        {
            this.SpawnRecord = spawnRecord;
            this.m_Id = this.Map.Instance.PopNextNPEntityId();
        }

        public void InteractWith(Character character, NpcActionsEnum actionType)
        {
            if (character.Busy)
                return;

            NpcActionRecord action = GetAction(actionType);

            if (action != null)
            {
                NpcsManager.Instance.HandleNpcAction(character, this, action);
            }
            else if (character.Client.Account.Role > ServerRoleEnum.Player)
            {
                character.ReplyWarning("No (" + actionType + ") action linked to this npc...(" + SpawnRecord.Id + ")");
            }
        }
        public override GameRolePlayActorInformations GetActorInformations()
        {
            return new GameRolePlayNpcInformations()
            {
                contextualId = Id,
                disposition = new EntityDispositionInformations(CellId, (byte)Direction),
                look = Look.ToEntityLook(),
                npcId = (short)Template.Id,
                sex = Template.Sex,
                specialArtworkId = 0
            };

        }
        public void SetId(long id)
        {
            m_Id = id;
        }
        private NpcActionRecord GetAction(NpcActionsEnum actionsEnum)
        {
            return SpawnRecord.Actions.FirstOrDefault(x => x.Action == actionsEnum);
        }
        public override string ToString()
        {
            return "Npc (" + Name + ") (Id:" + SpawnRecord.Id + " Record Id:" + Template.Id + ") (CellId:" + CellId + ")";
        }
    }
}
