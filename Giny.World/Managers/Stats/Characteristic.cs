using Giny.Protocol.Types;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Stats
{
    [ProtoContract]
    public class Characteristic
    {
        [ProtoMember(1)]
        public virtual short Base
        {
            get;
            set;
        }
        [ProtoMember(2)]
        public virtual short Additional
        {
            get;
            set;
        }
        [ProtoMember(3)]
        public virtual short Objects
        {
            get;
            set;
        }
        // ignore
        public virtual short Context
        {
            get;
            set;
        }
        /// <summary>
        /// We dont clone context.
        /// </summary>
        public virtual Characteristic Clone()
        {
            return new Characteristic()
            {
                Additional = Additional,
                Base = Base,
                Objects = Objects
            };
        }
        public static Characteristic Zero()
        {
            return New(0);
        }
        public static Characteristic New(short @base)
        {
            return new Characteristic()
            {
                Base = @base,
                Additional = 0,
                Context = 0,
                Objects = 0
            };
        }
        public virtual CharacterBaseCharacteristic GetBaseCharacteristic()
        {
            return new CharacterBaseCharacteristic(Base, Additional, Objects, Context, Context);
        }
        public virtual short Total()
        {
            return (short)(Base + Additional + Objects);
        }
        public virtual short TotalInContext()
        {
            return (short)(Total() + Context);
        }
        public override string ToString()
        {
            return "Total Context : " + TotalInContext();
        }
    }
}
