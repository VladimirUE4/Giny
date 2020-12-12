using Giny.World.Managers.Fights.Buffs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Fights.Triggers
{
    public class Trigger
    {
        public TriggerType Type
        {
            get;
            private set;
        }

        public short? Value
        {
            get;
            private set;
        }

        public Trigger(TriggerType type, short? value = null)
        {
            this.Type = type;
            this.Value = value;
        }

        public override string ToString()
        {
            return Type.ToString();
        }

        public static IEnumerable<Trigger> Singleton(TriggerType type)
        {
            return new Trigger[]
            {
                new Trigger(type),
            };
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Trigger))
            {
                return false;
            }
            return Equals((Trigger)obj);
        }
        public bool Equals(Trigger other)
        {
            return this.Type == other.Type && this.Value == other.Value;
        }
        public static bool IsInstant(IEnumerable<Trigger> triggers)
        {
            return triggers.Count() == 1 && triggers.ElementAt(0).Type == TriggerType.Instant;
        }

        public static bool SequenceEquals(IEnumerable<Trigger> t1, IEnumerable<Trigger> t2)
        {
            foreach (var v1 in t1)
            {
                foreach (var v2 in t2)
                {
                    if (!v1.Equals(v2))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
