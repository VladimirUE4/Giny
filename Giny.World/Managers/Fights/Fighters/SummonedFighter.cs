using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Giny.Protocol.Types;
using Giny.World.Managers.Fights.Cast;
using Giny.World.Managers.Fights.Stats;
using Giny.World.Managers.Monsters;
using Giny.World.Records.Maps;
using Giny.World.Records.Monsters;
using Giny.World.Records.Spells;

namespace Giny.World.Managers.Fights.Fighters
{
    public class SummonedFighter : AIFighter, IMonster
    {
        public Fighter Summoner
        {
            get;
            set;
        }
        private SpellEffectHandler SummoningEffect
        {
            get;
            set;
        }

        public override string Name => Record.Name;

        public override short Level => Grade.Level;

        public override bool Sex => false;

        public SummonedFighter(Fighter owner, MonsterRecord record, SpellEffectHandler summoningEffect, byte grade, CellRecord cell) :
            base(owner.Team, null, record, record.GetGrade(grade))
        {
            this.Cell = cell;
            this.SummoningEffect = summoningEffect;
            this.Summoner = owner;
        }

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
                creatureLevel = Grade.Level,
                disposition = GetEntityDispositionInformations(),
                look = Look.ToEntityLook(),
                previousPositions = new short[0],
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
            throw new NotImplementedException();
        }
        public override SpellEffectHandler GetSummoningEffect()
        {
            return SummoningEffect;
        }
        public override void Kick(Fighter source)
        {
            throw new NotImplementedException();
        }
        public override bool IsSummoned()
        {
            return true;
        }
        public override Fighter GetSummoner()
        {
            return Summoner;
        }

        public override Spell GetSpell(short spellId)
        {
            var record = Record.SpellRecords[spellId];
            var level = record.GetLevel(Grade.GradeId);
            return new Spell(record, level);

        }
    }
}
