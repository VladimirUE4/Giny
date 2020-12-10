using Giny.Core.DesignPattern;
using Giny.Protocol.Custom.Enums;
using Giny.World.Managers.Effects;
using Giny.World.Managers.Fights.Cast;
using Giny.World.Managers.Fights.Fighters;
using Giny.World.Managers.Fights.Sequences;
using Giny.World.Managers.Maps.Shapes;
using Giny.World.Managers.Spells;
using Giny.World.Records.Maps;
using Giny.World.Records.Spells;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Fights.Marks
{
    public class GlyphAura : Glyph
    {
        private List<Fighter> AffectedFighters
        {
            get;
            set;
        }
        public GlyphAura(int id, EffectDice effect, Zone zone, MarkTriggerType triggers, Color color, Fighter source, CellRecord centerCell, SpellRecord spellRecord, SpellLevelRecord spellLevel) : base(id, effect, zone, triggers, color, source, centerCell, spellRecord, spellLevel)
        {
            this.AffectedFighters = new List<Fighter>();
        }

        public override bool StopMovement => true;

        public override GameActionMarkTypeEnum Type => GameActionMarkTypeEnum.GLYPH;

        public override bool IsVisibleFor(CharacterFighter fighter)
        {
            return true;
        }

        public override void Trigger(Fighter target, MarkTriggerType triggerType)
        {
            if (!AffectedFighters.Contains(target))
            {
                AffectedFighters.Add(target);
                Enter(target);
            }
        }
        public override void OnAdded()
        {
            CastTriggerSpell();
            AffectedFighters = Source.Fight.GetFighters<Fighter>(x => ContainsCell(x.Cell.Id)).ToList();
        }
        public override void OnRemoved()
        {
            foreach (var fighter in AffectedFighters.ToArray())
            {
                Leave(fighter);
            }
        }

        [WIP("2 times sames aura glyph wont work")]
        private void Leave(Fighter fighter)
        {
            fighter.Moved -= OnFighterMove;

            using (fighter.Fight.SequenceManager.StartSequence(SequenceTypeEnum.SEQUENCE_GLYPH_TRAP))
            {
                var buffs = fighter.GetBuffs().Where(x => x.Cast.SpellId == this.TriggerSpell.Record.Id);

                foreach (var buff in buffs.ToArray())
                {
                    fighter.RemoveAndDispellBuff(buff);
                }

                AffectedFighters.Remove(fighter);
            }

        }
        private void Enter(Fighter fighter)
        {
            fighter.Moved += OnFighterMove;
            var handler = SpellManager.Instance.GetSpellCastHandler(CreateSpellCast());

            if (!handler.Initialize())
            {
                return;
            }

            handler.Execute(new Fighter[] { fighter });

        }

        private void OnFighterMove(Fighter fighter)
        {
            if (!ContainsCell(fighter.Cell.Id))
            {
                Leave(fighter);
            }
        }
    }
}
