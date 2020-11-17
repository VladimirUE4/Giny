using Giny.Protocol.Types;
using Giny.World.Managers.Fights.Effects.Buffs;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Stats
{
    [ProtoContract]
    public class RelativeCharacteristic : Characteristic
    {
        private Characteristic Relative
        {
            get;
            set;
        }
        public short RelativeDelta
        {
            get
            {
                return (short)(Relative.Total() / 10);
            }
        }
        [ProtoMember(1)]
        public override short Base
        {
            get => base.Base;
            set => base.Base = value;
        }
        [ProtoMember(2)]
        public override short Additional
        {
            get => base.Additional;
            set => base.Additional = value;
        }
        [ProtoMember(3)]
        public override short Objects
        {
            get => base.Objects;
            set => base.Objects = value;
        }

        public RelativeCharacteristic()
        {

        }
        public void Bind(Characteristic relative)
        {
            this.Relative = relative;
        }
        public override CharacterBaseCharacteristic GetBaseCharacteristic()
        {
            return new CharacterBaseCharacteristic((short)(Base + RelativeDelta), Additional, Objects, Context, Context);
        }
        public static new RelativeCharacteristic Zero()
        {
            return new RelativeCharacteristic();
        }
        public static new RelativeCharacteristic New(short delta)
        {
            return new RelativeCharacteristic()
            {
                Base = delta,
                Additional = 0,
                Context = 0,
                Objects = 0,
                Relative = null
            };
        }
        public override Characteristic Clone()
        {
            return new RelativeCharacteristic()
            {
                Base = base.Base,
                Additional = Additional,
                Context = Context,
                Objects = Objects,
                Relative = Relative,
            };
        }
        public override short Total()
        {
            return (short)(RelativeDelta + Base + Additional + Objects);
        }
    }
}
