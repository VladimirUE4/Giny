using Giny.Core.Extensions;
using Giny.Protocol.Custom.Enums;
using Giny.Protocol.Messages;
using Giny.Protocol.Types;
using Giny.World.Managers.Entities;
using Giny.World.Managers.Entities.Look;
using Giny.World.Managers.Fights;
using Giny.World.Managers.Fights.Fighters;
using Giny.World.Records.Maps;
using Giny.World.Records.Monsters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Monsters
{
    public class MonsterGroup : Entity
    {
        private List<Monster> Monsters = new List<Monster>();

        public int MonsterCount
        {
            get
            {
                return Monsters.Count;
            }
        }
        public MonsterGroup(MapRecord map, short cellId)
        {
            this.Map = map;
            this.CellId = cellId;
            this.m_UId = Map.Instance.PopNextNPEntityId();
            this.CreationDate = DateTime.Now;
        }

        public override string Name
        {
            get
            {
                return Leader.Record.Name;
            }
        }

        private long m_UId;

        public override long Id
        {
            get
            {
                return m_UId;
            }
        }

        public override short CellId
        {
            get;
            set;
        }

        public DateTime CreationDate
        {
            get;
            set;
        }
        public override DirectionsEnum Direction
        {
            get;
            set;
        }

        public override ServerEntityLook Look
        {
            get { return Leader.Look; }
            set { return; }
        }

        public Monster Leader
        {
            get;
            private set;
        }

        public void AddMonster(Monster monster)
        {
            Monsters.Add(monster);

            if (Monsters.Count == 1)
                this.Leader = monster;
        }
        public IEnumerable<Monster> GetMonsters()
        {
            return Monsters;
        }
        private void Move(List<short> keys)
        {
            this.Map.Instance.Send(new GameMapMovementMessage(keys.ToArray(), 0, Id));
            this.CellId = keys.Last();
        }


        /*    public void RandomMapMove()
            {
                Lozenge lozenge = new Lozenge(1, 4);
                short cellId = lozenge.GetCells((short)this.CellId, Map).Where((short entry) => Map.IsCellWalkable(entry)).Random();

                if (cellId != 0)
                {
                    Pathfinder pathfinder = new Pathfinder(Map, (short)this.CellId, cellId);
                    var cells = pathfinder.FindPath();

                    if (cells != null && cells.Count > 0)
                    {
                        cells.Insert(0, (short)this.CellId);
                        this.Move(cells);
                    }
                }
            }
            public IEnumerable<Fighter> CreateFighters(FightTeam team)
            {
                foreach (var monster in Monsters)
                {
                    yield return monster.CreateFighter(team);
                }
            }
             */
        public MonsterInGroupInformations[] GetMonsterInGroupInformations()
        {
            return Monsters.FindAll(x => x != Leader).ConvertAll<MonsterInGroupInformations>(x => x.GetMonsterInGroupInformations()).ToArray();
        }

        public GroupMonsterStaticInformations GetGroupMonsterStaticInformations()
        {
            return new GroupMonsterStaticInformations()
            {
                mainCreatureLightInfos = Leader.GetMonsterInGroupLightInformations(),
                underlings = GetMonsterInGroupInformations(),
            };
        }

        public override GameRolePlayActorInformations GetActorInformations()
        {
            return new GameRolePlayGroupMonsterInformations()
            {
                contextualId = Id,
                alignmentSide = 0,
                disposition = new EntityDispositionInformations(CellId, (byte)Direction),
                hasAVARewardToken = false,
                hasHardcoreDrop = false,
                keyRingBonus = false,
                look = Look.ToEntityLook(),
                lootShare = 0,
                staticInfos = GetGroupMonsterStaticInformations(),
            };
        }
        public override string ToString()
        {
            return "Monsters (" + Name + "' group)";
        }
    }
}
