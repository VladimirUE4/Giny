using Giny.Protocol.Enums;
using Giny.World.Managers.Fights.Fighters;
using Giny.World.Records.Maps;
using Giny.World.Records.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Fights.Cast
{
    public class SpellCast
    {
        public Fighter Source
        {
            get;
            private set;
        }
        public short SpellId
        {
            get
            {
                return Spell.Record.Id;
            }
        }
        public Spell Spell
        {
            get;
            set;
        }
        public CellRecord TargetCell
        {
            get;
            set;
        }
        public bool Force
        {
            get;
            set;
        }
        public CellRecord CastCell
        {
            get;
            set;
        }
        public SpellCastResult[] BypassedConditions
        {
            get;
            set;
        }
        public FightSpellCastCriticalEnum Critical
        {
            get;
            set;
        }
        public bool IsCriticalHit => Critical == FightSpellCastCriticalEnum.CRITICAL_HIT;

        public bool ApFree => Force;

        public bool Silent
        {
            get;
            set;
        }
        public bool Weapon
        {
            get;
            set;
        }
        public ITriggerToken Token
        {
            get;
            set;
        }
        public bool SilentNetwork
        {
            get;
            set;
        }

        public SpellCast(Fighter source, Spell spell, CellRecord targetCell)
        {
            this.Source = source;
            this.Spell = spell;
            this.CastCell = source.Cell;
            this.TargetCell = targetCell;
            this.Force = false;
            this.Silent = false;
            this.SilentNetwork = false;
            this.Weapon = false;
        }

        public bool IsConditionBypassed(SpellCastResult result) => BypassedConditions != null && (BypassedConditions.Contains(result) || BypassedConditions.Contains(SpellCastResult.OK));

    }
}
