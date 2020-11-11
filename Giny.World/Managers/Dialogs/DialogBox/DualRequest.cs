using Giny.Protocol.Messages;
using Giny.World.Managers.Entities.Characters;
using Giny.World.Managers.Fights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Dialogs.DialogBox
{
    public class DualRequest : RequestBox
    {
        public DualRequest(Character source, Character target)
            : base(source, target)
        {

        }
        protected override void OnOpen()
        {
            Source.Client.Send(new GameRolePlayPlayerFightFriendlyRequestedMessage()
            {
                targetId = Target.Id,
                fightId = 0,
                sourceId = Source.Id
            });

            Target.Client.Send(new GameRolePlayPlayerFightFriendlyRequestedMessage()
            {
                targetId = Target.Id,
                fightId = 0,
                sourceId = Source.Id,
            });
        }
        protected override void OnAccept()
        {
            Source.Client.Send(new GameRolePlayPlayerFightFriendlyAnsweredMessage()
            {
                accept = true,
                fightId = 0,
                sourceId = Source.Id,
                targetId = Target.Id,

            });

            FightDual fight = FightManager.Instance.CreateFightDual(Source, Target, Source.GetCell());

            fight.RedTeam.AddFighter(Target.CreateFighter(fight.RedTeam));

            fight.BlueTeam.AddFighter(Source.CreateFighter(fight.BlueTeam));

            fight.StartPlacement();
        }
        protected override void OnDeny()
        {
            Source.Client.Send(new GameRolePlayPlayerFightFriendlyAnsweredMessage()
            {
                accept = false,
                fightId = 0,
                sourceId = Source.Id,
                targetId = Target.Id,
            });
        }
        protected override void OnCancel()
        {
            Target.Client.Send(new GameRolePlayPlayerFightFriendlyAnsweredMessage()
            {
                accept = false,
                fightId = 0,
                sourceId = Source.Id,
                targetId = Target.Id
            });

        }
    }
}
