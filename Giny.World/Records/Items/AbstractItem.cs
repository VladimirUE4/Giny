using Giny.ORM.Attributes;
using Giny.Protocol.Custom.Enums;
using Giny.Protocol.Enums;
using Giny.Protocol.Types;
using Giny.World.Managers.Effects;
using Giny.World.Managers.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Records.Items
{
    public abstract class AbstractItem
    {
        [Ignore]
        private ItemRecord m_record;

        [Ignore]
        public ItemRecord Record
        {
            get
            {
                if (m_record == null)
                {
                    m_record = ItemRecord.GetItem(GId);
                    return m_record;
                }
                else
                {
                    return m_record;
                }
            }
        }

        [Primary]
        public int UId
        {
            get;
            set;
        }

        public short GId
        {
            get;
            set;
        }

        [Update]
        public byte Position
        {
            get;
            set;
        }

        [Ignore]
        public CharacterInventoryPositionEnum PositionEnum
        {
            get
            {
                return (CharacterInventoryPositionEnum)Position;
            }
            set
            {
                Position = (byte)value;
            }
        }

        [Update]
        public int Quantity
        {
            get;
            set;
        }

        [ProtoSerialize, Update]
        public List<Effect> Effects
        {
            get;
            set;
        }

        [Update]
        public short AppearanceId
        {
            get;
            set;
        }
        [Update]
        public string Look
        {
            get;
            set;
        }

        [Ignore]
        public bool IsAssociated
        {
            get
            {
                return HasEffect(EffectsEnum.Effect_LivingObjectId) || HasEffect(EffectsEnum.Effect_Apparence_Wrapper)
                    || HasEffect(EffectsEnum.Effect_Appearance);
            }
        }

        public ObjectItem GetObjectItem()
        {
            return new ObjectItem(Position, GId, GetObjectEffects(), UId, Quantity);
        }
        public ObjectItemQuantity GetObjectItemQuantity()
        {
            return new ObjectItemQuantity(UId, Quantity);
        }
        public ObjectEffect[] GetObjectEffects()
        {
            ObjectEffect[] effects = new ObjectEffect[Effects.Count];
            for (int i = 0; i < Effects.Count; i++)
            {
                effects[i] = Effects[i].GetObjectEffect();
            }
            return effects;
        }

        public T GetFirstEffect<T>() where T : Effect
        {
            return Effects.OfType<T>().FirstOrDefault();
        }
        public T GetEffect<T>(Predicate<Effect> predicate) where T : Effect
        {
            return (T)Effects.Find(predicate);
        }
        public T GetEffect<T>(EffectsEnum effectEnum) where T : Effect
        {
            return (T)Effects.FirstOrDefault(x => x.EffectEnum == effectEnum);
        }
        public IEnumerable<T> GetEffects<T>() where T : Effect
        {
            return Effects.OfType<T>();
        }
        public void AddEffect(Effect effect) 
        {
            Effects.Add(effect);
        }
        public bool HasEffect(EffectsEnum effect)
        {
            return Effects.Any(x => x.EffectEnum == effect);
        }
        public bool HasEffect<T>() where T : Effect
        {
            return Effects.OfType<T>().Count() > 0;
        }
        public void RemoveEffects()
        {
            Effects.Clear();
        }
        public void RemoveEffects(EffectsEnum effectsEnum)
        {
            Effects.RemoveAll(x => x.EffectEnum == effectsEnum);
        }
        protected List<Effect> CloneEffects()
        {
            List<Effect> results = new List<Effect>();

            foreach (var effect in this.Effects)
            {
                results.Add((Effect)effect.Clone());
            }
            return results;
        }
       

        public abstract AbstractItem CloneWithUID();

        public abstract AbstractItem CloneWithoutUID();

        public abstract void Initialize();

        public MerchantItemRecord ToMerchantItemRecord(long characterId, long price, int quantity)
        {
            return new MerchantItemRecord()
            {
                CharacterId = characterId,
                UId = UId,
                AppearanceId = AppearanceId,
                Effects = new List<Effect>(Effects), /* Clone each effects */
                GId = GId,
                Look = Look,
                Position = Position,
                Quantity = quantity,
                Price = price,
                Sold = false,
            };
        }
        public CharacterItemRecord ToCharacterItemRecord(long characterId)
        {
            return new CharacterItemRecord()
            {
                CharacterId = characterId,
                UId = UId,
                AppearanceId = AppearanceId,
                Effects = new List<Effect>(Effects), /* Clone each effects */
                GId = GId,
                Look = Look,
                Position = Position,
                Quantity = Quantity,
            };
        }
        public BankItemRecord ToBankItemRecord(int accountId)
        {
            return new BankItemRecord()
            {
                AccountId = accountId,
                UId = UId,
                AppearanceId = AppearanceId,
                Effects = new List<Effect>(Effects),
                GId = GId,
                Look = Look,
                Position = Position,
                Quantity = Quantity,
            };
        }
        public BidShopItemRecord ToBidShopItemRecord(long bidshopId, int accountId, long price)
        {
            return new BidShopItemRecord()
            {
                AccountId = accountId,
                AppearanceId = AppearanceId,
                BidShopId = bidshopId,
                Effects = new List<Effect>(Effects),
                GId = GId,
                Look = Look,
                Position = Position,
                Price = price,
                Quantity = Quantity,
                UId = ItemManager.Instance.PopItemUID(),
                Sold = false,

            };
        }

    }
}
