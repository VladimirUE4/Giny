using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Giny.World.Managers.Fights.AI;
using Giny.World.Managers.Fights.Cast;
using Giny.World.Managers.Fights.Stats;
using Giny.World.Managers.Monsters;
using Giny.World.Records.Maps;
using Giny.World.Records.Monsters;

namespace Giny.World.Managers.Fights.Fighters
{
    public abstract class AIFighter : Fighter, IMonster
    {
        public MonsterRecord Record
        {
            get;
            private set;
        }
        protected MonsterGrade Grade
        {
            get;
            private set;
        }
        protected MonsterBrain Brain
        {
            get;
            private set;
        }
        public short MonsterId => (short)Record.Id;

        public AIFighter(FightTeam team, CellRecord roleplayCell, MonsterRecord record, MonsterGrade grade) : base(team, roleplayCell)
        {
            this.Record = record;
            this.Grade = grade;
        }
        public override void Initialize()
        {
            this.Id = Fight.PopNextContextualId();
            this.Stats = new FighterStats(Grade);
            this.Brain = new MonsterBrain(this);
            base.Initialize();
            this.IsReady = true;
        }
        public override void OnTurnBegin()
        {
            this.Brain.Play();
            PassTurn();
        }
        public override bool HasSpell(short spellId)
        {
            return Record.Spells.Contains(spellId);
        }

        public override Spell GetSpell(short spellId)
        {
            var record = Record.SpellRecords[spellId];
            var level = record.GetLevel(Grade.GradeId);
            return new Spell(record, level);
        }
    }
}
