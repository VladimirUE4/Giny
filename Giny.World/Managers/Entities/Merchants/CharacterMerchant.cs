using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Giny.Protocol.Custom.Enums;
using Giny.Protocol.Enums;
using Giny.Protocol.Types;
using Giny.World.Managers.Entities.Look;
using Giny.World.Records.Characters;

namespace Giny.World.Managers.Entities.Merchants
{
    public class CharacterMerchant : Entity
    {
        public const short BAG_SKIN = 237;

        private MerchantRecord Record
        {
            get;
            set;
        }

        public CharacterMerchant(MerchantRecord record)
        {
            this.Record = record;

            ServerEntityLook bagLook = new ServerEntityLook();
            bagLook.SetBones(BAG_SKIN);
            Look.RemoveAura();
            Look.SubEntities.Add(new ServerSubentityLook(SubEntityBindingPointCategoryEnum.HOOK_POINT_CATEGORY_MERCHANT_BAG, 0, bagLook));
        }

        public override long Id => Record.CharacterId;

        public override string Name => Record.Name;

        public override short CellId
        {
            get => Record.CellId;
            set => throw new NotImplementedException();
        }
        public override DirectionsEnum Direction
        {
            get => Record.Direction;
            set => throw new NotImplementedException();
        }
        public override ServerEntityLook Look
        {
            get => Record.Look;
            set => throw new NotImplementedException();
        }

        public override GameRolePlayActorInformations GetActorInformations()
        {
            return new GameRolePlayMerchantInformations()
            {
                contextualId = Id,
                sellType = 0,
                disposition = new EntityDispositionInformations(CellId, (byte)Direction),
                look = Look.ToEntityLook(),
                name = Name,
                options = new HumanOption[0],
            };
        }
    }
}
