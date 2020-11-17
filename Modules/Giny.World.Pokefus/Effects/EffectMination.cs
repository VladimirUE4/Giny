using Giny.Protocol.Types;
using Giny.World.Managers.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Pokefus.Effects
{
    public class EffectMination : Effect
    {
        public const short DummyTextEffectId = 990;

        public const string EffectMessage = "Contrôle du monstre {0} ({1})";

        public short MonsterId
        {
            get;
            set;
        }
        public sbyte GradeId
        {
            get;
            set;
        }
        public string MonsterName
        {
            get;
            set;
        }
        public EffectMination()
        {

        }
        public EffectMination(short monsterId, string monsterName, sbyte gradeId)
            : base(DummyTextEffectId)
        {
            this.MonsterId = monsterId;
            this.MonsterName = monsterName;
            this.GradeId = gradeId;
        }

        public override ObjectEffect GetObjectEffect()
        {
            return new ObjectEffectString()
            {
                actionId = EffectId,
                value = string.Format(EffectMessage, MonsterName, GradeId),
            };
        }

        public override object Clone()
        {
            return null;
        }
    }
}
