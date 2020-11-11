using Giny.Core.Extensions;
using Giny.ORM;
using Giny.ORM.Attributes;
using Giny.ORM.Interfaces;
using Giny.Protocol.Custom.Enums;
using Giny.Protocol.Enums;
using Giny.Protocol.Types;
using Giny.World.Managers.Effects;
using Giny.World.Managers.Entities.Characters;
using Giny.World.Managers.Items;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Records.Items
{
    [Table("CharactersItems")]
    public class CharacterItemRecord : AbstractItem, ITable
    {
        private static readonly ConcurrentDictionary<long, CharacterItemRecord> CharactersItems = new ConcurrentDictionary<long, CharacterItemRecord>();

        [Update]
        public long CharacterId
        {
            get;
            set;
        }

        long ITable.Id => base.UId;

        public CharacterItemRecord(long characterId, int uid, short gid, byte position, int quantity, List<Effect> effects, short appearanceId, string look)
        {
            this.UId = uid;
            this.GId = gid;
            this.Position = position;
            this.CharacterId = characterId;
            this.Quantity = quantity;
            this.Effects = effects;
            this.AppearanceId = appearanceId;
            this.Look = look;
        }

     
        public CharacterItemRecord()
        {

        }


        public bool IsEquiped()
        {
            return PositionEnum != CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED;
        }

        public ObjectItemNotInContainer GetObjectItemNotInContainer()
        {
            return new ObjectItemNotInContainer(GId, Effects.ConvertAll<ObjectEffect>(x => x.GetObjectEffect()).ToArray(), UId, Quantity);
        }
        public static IEnumerable<CharacterItemRecord> GetCharacterItems(long characterId)
        {
            return CharactersItems.Values.Where(x => x.CharacterId == characterId);
        }

        public override string ToString()
        {
            return "(" + UId + ") " + Record.Name;
        }
        public bool CanBeExchanged()
        {
            return Record.Exchangeable;
        }

        public static int GetLastItemUID()
        {
            return (int)CharactersItems.Keys.OrderByDescending(x => x).FirstOrDefault();
        }
        public override AbstractItem CloneWithUID()
        {
            return new CharacterItemRecord(CharacterId, UId, GId, Position, Quantity, this.CloneEffects(), AppearanceId, Look); /* shouldnt we clone each effects? */
        }
        public override AbstractItem CloneWithoutUID()
        {
            return new CharacterItemRecord(CharacterId, ItemManager.Instance.PopItemUID(), GId, Position, Quantity, this.CloneEffects(), AppearanceId, Look);
        }

        public static void RemoveCharacterItems(long characterId)
        {
            CharacterItemRecord[] items = CharactersItems.Values.Where(x => x.CharacterId == characterId).ToArray();
            items.RemoveInstantElements(typeof(CharacterItemRecord));
        }

        public override void Initialize()
        {
            switch (Record.TypeEnum)
            {
                case ItemTypeEnum.LIVING_OBJECT:
                    LivingObjectManager.Instance.InitializeLivingObject(this);
                    break;
            }
        }
        public void Feed(Character character, ObjectItemQuantity[] meal)
        {
            if (this.HasEffect(EffectsEnum.Effect_LivingObjectId))
            {
                LivingObjectManager.Instance.FeedLivingObject(character, this, meal);
            }
        }
    }
}
