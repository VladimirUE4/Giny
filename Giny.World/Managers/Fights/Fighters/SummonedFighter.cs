using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Giny.Core.DesignPattern;
using Giny.Protocol.Types;
using Giny.World.Managers.Fights.Cast;
using Giny.World.Managers.Fights.Stats;
using Giny.World.Managers.Monsters;
using Giny.World.Records.Maps;
using Giny.World.Records.Monsters;
using Giny.World.Records.Spells;

namespace Giny.World.Managers.Fights.Fighters
{
    public abstract class SummonedFighter : AIFighter
    {
        public Fighter Summoner
        {
            get;
            set;
        }
        public Fighter Controller
        {
            get;
            set;
        }

        public bool Controled => Controller != null;

        private SpellEffectHandler SummoningEffect
        {
            get;
            set;
        }

        public override bool Sex => false;

        public SummonedFighter(Fighter owner, SpellEffectHandler summoningEffect, CellRecord cell) :
            base(owner.Team, null)
        {
            this.Cell = cell;
            this.SummoningEffect = summoningEffect;
            this.Summoner = owner;
        }

        public override FightTeamMemberInformations GetFightTeamMemberInformations()
        {
            throw new NotImplementedException();
        }
        public override SpellEffectHandler GetSummoningEffect()
        {
            return SummoningEffect;
        }
        public override void Kick(Fighter source)
        {
            throw new NotImplementedException();
        }
        public override void OnTurnBegin()
        {
            base.OnTurnBegin();
        }
        [WIP]
        public void SetController(Fighter controller)
        {
            this.Controller = controller;
        }
        public void RemoveController()
        {
            if (Controller != null)
            {
                this.Controller = null;
                //
            }
        }
        public override bool IsSummoned()
        {
            return true;
        }
        public override Fighter GetSummoner()
        {
            return Summoner;
        }
    }
}
