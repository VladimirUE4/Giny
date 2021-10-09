using Giny.Core;
using Giny.Core.Time;
using Giny.Protocol.Custom.Enums;
using Giny.Protocol.Enums;
using Giny.Protocol.Types;
using Giny.World.Api;
using Giny.World.Managers.Chat;
using Giny.World.Managers.Entities;
using Giny.World.Managers.Entities.Look;
using Giny.World.Modules;
using Giny.World.Network;
using Giny.World.Records.Characters;
using Giny.World.Records.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.InGameLadder
{
    [Module("InGameLadder")]
    public class Module : IModule
    {
        private const long TargetMapId = 211292164;

        private static short[] Cells = new short[]
        {
            132,161,190,
        };

        private MapRecord TargetMap
        {
            get;
            set;
        }

        private List<CharacterLadder> Ladder = new List<CharacterLadder>();

        public void CreateHooks()
        {

        }


        public void Initialize()
        {
            PutEntities();

            ActionTimer timer = new ActionTimer(60000, Reload, true);
            timer.Start();

        }

        private void PutEntities()
        {
            TargetMap = MapRecord.GetMap(TargetMapId);

            var characters = CharacterRecord.GetCharacterRecords().OrderByDescending(x => x.Experience).Take(Cells.Count()).ToArray();

            for (int i = 0; i < characters.Count(); i++)
            {
                if (i <= Cells.Count() -1)
                {
                    var entity = new CharacterLadder(i + 1, characters[i],
                   TargetMap, Cells[i]);

                    Ladder.Add(entity);

                    TargetMap.Instance.AddEntity(entity);
                }
            }

        }

        private void RemoveEntities()
        {
            foreach (var characterLadder in TargetMap.Instance.GetEntities<CharacterLadder>())
            {
                TargetMap.Instance.RemoveEntity(characterLadder.Id);
            }
        }
        private void Reload()
        {
            RemoveEntities();
            PutEntities();
        }


        [ChatCommand("ladder", ServerRoleEnum.Player)]
        public static void LadderCommand(WorldClient client)
        {
            client.Character.Teleport(TargetMapId);
        }

    }

    public class CharacterLadder : Entity
    {
        public int Rank
        {
            get;
            set;
        }
        public CharacterRecord Record
        {
            get;
            set;
        }

        public CharacterLadder(int rank, CharacterRecord record, MapRecord map, short cellId) : base(map)
        {
            this.Rank = rank;
            this.Record = record;
            this.m_id = map.Instance.PopNextNPEntityId();
            this.CellId = cellId;


            this.Look = Record.Look.ActorLook.Clone();
            this.Look.SetBones(1);
        }

        private long m_id;

        public override long Id => m_id;

        public override string Name => Record.Name;

        public override short CellId
        {
            get;
            set;
        }
        public override DirectionsEnum Direction
        {
            get => DirectionsEnum.DIRECTION_SOUTH_WEST;
            set => throw new NotImplementedException();
        }
        public override ServerEntityLook Look
        {
            get;
            set;
        }

        public override GameRolePlayActorInformations GetActorInformations()
        {
            short titleId = 252;

            switch (Rank)
            {
                case 1:
                    titleId = 40;
                    break;
                case 2:
                    titleId = 41;
                    break;
                case 3:
                    titleId = 42;
                    break;
            }

            var option = new HumanOptionTitle(titleId, string.Empty);

            return new GameRolePlayCharacterInformations()
            {
                accountId = 0,
                alignmentInfos = new ActorAlignmentInformations()
                {
                    alignmentGrade = 0,
                    alignmentSide = 0,
                    characterPower = 0,
                    alignmentValue = 0,
                }
                ,
                humanoidInfo = new HumanInformations(new ActorRestrictionsInformations(), Record.Sex, new HumanOption[] { option }),
                contextualId = Id,
                disposition = new EntityDispositionInformations()
                {
                    cellId = CellId,
                    direction = (byte)Direction,
                },
                look = Look.ToEntityLook(),
                name = Name,
            };
        }
    }
}
