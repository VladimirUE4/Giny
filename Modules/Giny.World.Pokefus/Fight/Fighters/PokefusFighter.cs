using Giny.Protocol.Types;
using Giny.World.Managers.Fights;
using Giny.World.Managers.Fights.Cast;
using Giny.World.Managers.Fights.Fighters;
using Giny.World.Managers.Fights.Stats;
using Giny.World.Managers.Monsters;
using Giny.World.Records.Maps;
using Giny.World.Records.Monsters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.Pokefus.Fight.Fighters
{
    public class PokefusFighter : SummonedMonster
    {
        public PokefusFighter(Fighter owner, MonsterRecord record, SpellEffectHandler summoningEffect, byte gradeId, CellRecord cell) : base(owner, record, summoningEffect, gradeId, cell)
        {

        }

        public override bool CanDrop => true;

        public override bool Sex => false;

        public override void Initialize()
        {
            this.Look = Record.Look.Clone();
            this.Stats = new FighterStats(Grade);
            base.Initialize();
        }

        public override GameFightFighterInformations GetFightFighterInformations(CharacterFighter target)
        {
            return new GameFightMonsterInformations()
            {
                contextualId = Id,
                creatureGenericId = (short)Record.Id,
                creatureGrade = Grade.GradeId,
                creatureLevel = Level,
                disposition = GetEntityDispositionInformations(),
                look = Look.ToEntityLook(),
                previousPositions = GetPreviousPositions(),
                stats = Stats.GetFightMinimalStats(this, target),
                wave = 0,
                spawnInfo = new GameContextBasicSpawnInformation()
                {
                    teamId = (byte)Team.TeamId,
                    alive = Alive,
                    informations = new GameContextActorPositionInformations()
                    {
                        contextualId = Id,
                        disposition = GetEntityDispositionInformations(),
                    },
                }
            };
        }

        public override FightTeamMemberInformations GetFightTeamMemberInformations()
        {
            return new FightTeamMemberMonsterInformations()
            {
                id = Id,
                grade = Grade.GradeId,
                monsterId = (int)Record.Id
            };
        }

        public override void Kick(Fighter source)
        {
            throw new NotImplementedException();
        }

    }
}
