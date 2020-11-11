using Giny.Protocol.Custom.Enums;
using Giny.Protocol.Types;
using Giny.World.Managers.Effects;
using Giny.World.Managers.Fights.Cast;
using Giny.World.Managers.Fights.Fighters;
using Giny.World.Managers.Maps;
using Giny.World.Managers.Maps.Shapes;
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
    public abstract class Mark
    {
        public abstract bool StopMovement
        {
            get;
        }
        public abstract GameActionMarkTypeEnum Type
        {
            get;
        }
        private bool Active
        {
            get;
            set;
        }
        public Fighter Source
        {
            get;
            private set;
        }
        public int Id
        {
            get;
            private set;
        }
        protected CellRecord CenterCell
        {
            get;
            set;
        }
        protected Spell MarkSpell
        {
            get;
            private set;
        }
        protected Spell TriggerSpell
        {
            get;
            private set;
        }
        private Color Color
        {
            get;
            set;
        }
        private List<MarkShape> Shapes
        {
            get;
            set;
        }
        private CellRecord[] Cells
        {
            get;
            set;
        }
        public MarkTriggerType Triggers
        {
            get;
            private set;
        }
        public Mark(int id, EffectDice effect, Zone zone, MarkTriggerType triggers, Color color, Fighter source, CellRecord centerCell, SpellRecord spellRecord, SpellLevelRecord spellLevel)
        {
            this.Id = id;
            this.Color = color;
            this.Triggers = triggers;
            this.Source = source;
            this.CenterCell = centerCell;
            this.MarkSpell = new Spell(spellRecord, spellLevel);
            var triggerSpellRecord = SpellRecord.GetSpellRecord((short)effect.Min);
            this.TriggerSpell = new Spell(triggerSpellRecord, triggerSpellRecord.GetLevel(MarkSpell.Level.Grade));
            this.Active = true;
            this.BuildShapes(zone);
        }
        protected void UpdateColor(Color color)
        {
            this.Color = color;

            foreach (var shape in Shapes)
            {
                shape.Color = color;
            }
        }
        private void BuildShapes(Zone zone)
        {
            this.Cells = zone.GetCells(CenterCell, Source.Fight.Map);

            Shapes = new List<MarkShape>();

            for (int i = 0; i < Cells.Length; i++)
            {
                Shapes.Add(new MarkShape(Source.Fight, Cells[i], Color));
            }
        }
        public GameActionMark GetGameActionMark()
        {
            return new GameActionMark()
            {
                active = Active,
                markAuthorId = Source.Id,
                cells = Shapes.Select(x => x.GetGameActionMarkedCell()).ToArray(),
                markId = (short)Id,
                markimpactCell = CenterCell.Id,
                markSpellId = MarkSpell.Record.Id,
                markSpellLevel = MarkSpell.Level.Grade,
                markTeamId = (byte)Source.Team.TeamId,
                markType = (byte)Type,
            };
        }
        public GameActionMark GetHiddenGameActionMark()
        {
            return new GameActionMark()
            {
                active = Active,
                markAuthorId = Source.Id,
                cells = Shapes.Select(x => x.GetGameActionMarkedCell()).ToArray(),
                markId = (short)Id,
                markimpactCell = CenterCell.Id,
                markSpellId = MarkSpell.Record.Id,
                markSpellLevel = MarkSpell.Level.Grade,
                markTeamId = (byte)Source.Team.TeamId,
                markType = (byte)Type,
            };
        }

        public abstract bool IsVisibleFor(CharacterFighter fighter);

        public bool ContainsCell(short cellId)
        {
            return Cells.Any(x => x.Id == cellId);
        }

        public abstract void Trigger(Fighter target, MarkTriggerType triggerType);

    }
}
