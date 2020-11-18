using Giny.Protocol.Types;
using Giny.World.Managers.Effects;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.Pokefus.Effects
{
    [ProtoContract]
    public class EffectPokefus : EffectCustom
    {
        private const string Description = "Contrôle du monstre {0} ({1})";

        [ProtoMember(1)]
        public override short EffectId
        {
            get => base.EffectId;
            set => base.EffectId = value;
        }
        [ProtoMember(2)]
        public short MonsterId
        {
            get;
            set;
        }
        [ProtoMember(3)]
        public byte GradeId
        {
            get;
            set;
        }
        [ProtoMember(4)]
        public string MonsterName
        {
            get;
            set;
        }
        public EffectPokefus()
        {

        }
        public EffectPokefus(short monsterId, string monsterName, byte gradeId)
        {
            this.MonsterId = monsterId;
            this.MonsterName = monsterName;
            this.GradeId = gradeId;
        }

        public override object Clone()
        {
            return new EffectPokefus()
            {
                EffectId = EffectId,
                MonsterId = MonsterId,
                GradeId = GradeId,
                MonsterName = MonsterName,
            };
        }

        public override bool Equals(object obj)
        {
            return obj is EffectPokefus ? Equals((EffectPokefus)obj) : false;
        }
        private bool Equals(EffectPokefus effect)
        {
            return MonsterId == effect.MonsterId && GradeId == effect.GradeId && MonsterName == effect.MonsterName;
        }
        public override string GetEffectDescription()
        {
            return string.Format(Description, MonsterName, GradeId);
        }
    }
}
