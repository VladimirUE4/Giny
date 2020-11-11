using Giny.Core.DesignPattern;
using Giny.Protocol.Custom.Enums;
using Giny.Protocol.Enums;
using Giny.Protocol.Types;
using Giny.World.Managers.Effects.Targets;
using Giny.World.Managers.Fights.Buffs;
using Giny.World.Managers.Fights.Cast;
using Giny.World.Managers.Maps.Shapes;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Effects
{
    [ProtoContract]
    [ProtoInclude(2, typeof(EffectDice))]
    [ProtoInclude(3, typeof(EffectDice))]
    [ProtoInclude(5, typeof(EffectInteger))]
    [ProtoInclude(4, typeof(EffectInteger))]
    public abstract class Effect : ICloneable
    {
        public EffectsEnum EffectEnum
        {
            get
            {
                return (EffectsEnum)EffectId;
            }
        }
        [ProtoMember(1)]
        public virtual short EffectId
        {
            get;
            set;
        }
        [ProtoMember(6)]
        public int Order
        {
            get;
            set;
        }
        [ProtoMember(7)]
        public int TargetId
        {
            get;
            set;
        }
        [ProtoMember(8)]
        public string TargetMask
        {
            get;
            set;
        }
        [ProtoMember(9)]
        public int Duration
        {
            get;
            set;
        }
        [ProtoMember(10)]
        public int Delay
        {
            get;
            set;
        }
        [ProtoMember(11)]
        public int Random
        {
            get;
            set;
        }
        [ProtoMember(12)]
        public int Group
        {
            get;
            set;
        }
        [ProtoMember(13)]
        public int Modificator
        {
            get;
            set;
        }
        [ProtoMember(14)]
        public bool Trigger
        {
            get;
            set;
        }
        [ProtoMember(15)]
        public string Triggers
        {
            get;
            set;
        }
        [ProtoMember(16)]
        public int Dispellable
        {
            get;
            set;
        }
        [ProtoMember(17)]
        public string RawZone
        {
            get;
            set;
        }

        private BuffTriggerType? m_triggersEnum;

        [WIP]
        public BuffTriggerType TriggersEnum
        {
            get
            {
                if (!m_triggersEnum.HasValue)
                {
                    m_triggersEnum = ParseTriggers();
                }

                return ParseTriggers();// m_triggersEnum.Value;
            }
        }
        public Effect()
        {

        }

        public Effect(short effectId)
        {
            this.EffectId = effectId;
        }

        public Zone GetZone()
        {
            return GetZone(0);
        }
        public Zone GetZone(DirectionsEnum direction)
        {
            SpellShapeEnum zoneShape = 0;
            int zoneSize = 0;
            int zoneMinSize = 0;

            if (string.IsNullOrEmpty(RawZone))
            {
                return null;
            }

            var shape = (SpellShapeEnum)RawZone[0];
            byte size = 0;
            byte minSize = 0;
            int zoneEfficiency = 0;
            int zoneMaxEfficiency = 0;
            int zoneEfficiencyPercent = 0;

            var data = RawZone.Remove(0, 1).Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            var hasMinSize = shape == SpellShapeEnum.C || shape == SpellShapeEnum.X || shape == SpellShapeEnum.Q || shape == SpellShapeEnum.plus || shape == SpellShapeEnum.sharp;

            if (data.Length >= 4)
            {
                size = byte.Parse(data[0]);
                minSize = byte.Parse(data[1]);
                zoneEfficiency = byte.Parse(data[2]);
                zoneMaxEfficiency = byte.Parse(data[2]);
            }
            else
            {
                if (data.Length >= 1)
                    size = byte.Parse(data[0]);

                if (data.Length >= 2)
                    if (hasMinSize)
                        minSize = byte.Parse(data[1]);
                    else
                        zoneEfficiency = byte.Parse(data[1]);

                if (data.Length >= 3)
                    if (hasMinSize)
                        zoneEfficiency = byte.Parse(data[2]);
                    else
                        zoneMaxEfficiency = byte.Parse(data[2]);
            }


            zoneShape = shape;
            zoneSize = size;
            zoneMinSize = minSize;
            zoneEfficiencyPercent = zoneEfficiency;

            return new Zone(zoneShape, (byte)zoneSize, (byte)zoneMinSize, direction, zoneEfficiency, zoneMaxEfficiency);
        }
        [WIP]
        private BuffTriggerType ParseTriggers()
        {
            switch (Triggers.ToUpper())
            {
           
                case "TE":
                    return BuffTriggerType.OnTurnEnd;
                case "TB":
                    return BuffTriggerType.OnTurnBegin;
                case "D":
                    return BuffTriggerType.OnDamaged;
                case "DR":
                    return BuffTriggerType.OnDamagedInLongRange;
                case "DS":
                    return BuffTriggerType.OnDamagedBySpell;
                case "DM":
                    return BuffTriggerType.OnDamagedInCloseRange;
                case "I":
                    return BuffTriggerType.Instant;
            }

            return BuffTriggerType.Unknown;
        }
        public IEnumerable<TargetCriterion> GetTargets()
        {
            if (string.IsNullOrEmpty(TargetMask) || TargetMask == "a,A" || TargetMask == "A,a")
            {
                return new TargetCriterion[0]; // default target = ALL
            }

            var data = TargetMask.Split(',');

            IEnumerable<TargetCriterion> result = data.Select(TargetCriterion.ParseCriterion).ToArray();

            return result;
        }

        public abstract ObjectEffect GetObjectEffect();

        public abstract object Clone();
    }
}
