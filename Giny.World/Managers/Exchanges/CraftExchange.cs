using Giny.Protocol.Custom.Enums;
using Giny.Protocol.Enums;
using Giny.Protocol.Messages;
using Giny.World.Managers.Entities.Characters;
using Giny.World.Records.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Exchanges
{
    public class CraftExchange : Exchange
    {
        public override ExchangeTypeEnum ExchangeType => ExchangeTypeEnum.CRAFT;

        private SkillRecord Skill
        {
            get;
            set;
        }
        
        public CraftExchange(Character character, SkillRecord skillRecord) : base(character)
        {
            this.Skill = skillRecord;
        }

        public override void ModifyItemPriced(int objectUID, int quantity, long price)
        {
            throw new NotImplementedException();
        }

        public override void MoveItem(int uid, int quantity)
        {
            throw new NotImplementedException();
        }

        public override void MoveItemPriced(int objectUID, int quantity, long price)
        {
            throw new NotImplementedException();
        }

        public override void MoveKamas(long quantity)
        {
            throw new NotImplementedException();
        }

        public override void OnNpcGenericAction(NpcActionsEnum action)
        {
            throw new NotImplementedException();
        }

        public override void Open()
        {
            Character.Client.Send(new ExchangeStartOkCraftWithInformationMessage()
            {
                skillId = (short)Skill.Id,
            });
        }

        public override void Ready(bool ready, short step)
        {
            throw new NotImplementedException();
        }
    }
}
