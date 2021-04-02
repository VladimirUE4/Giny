using Giny.Core.DesignPattern;
using Giny.Core.Pool;
using Giny.Core.Time;
using Giny.ORM;
using Giny.Protocol.Custom.Enums;
using Giny.Protocol.Enums;
using Giny.World.Managers.Criterias;
using Giny.World.Managers.Effects;
using Giny.World.Managers.Entities.Characters;
using Giny.World.Records.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Items
{
    public class ItemManager : Singleton<ItemManager>
    {
        private static EffectsEnum[] IGNORED_EFFECTS = new EffectsEnum[]
        {
            EffectsEnum.Effect_Exchangeable,
        };



        private UniqueIdProvider m_idprovider;

        private Dictionary<ItemUsageHandlerAttribute, MethodInfo> m_usageHandlers = new Dictionary<ItemUsageHandlerAttribute, MethodInfo>();

        [StartupInvoke("Items Manager", StartupInvokePriority.SixthPath)]
        public void Initialize()
        {
            int lastUID = CharacterItemRecord.GetLastItemUID();
            lastUID = Math.Max(lastUID, BankItemRecord.GetLastItemUID());
            lastUID = Math.Max(lastUID, BidShopItemRecord.GetLastItemUID());
            lastUID = Math.Max(lastUID, MerchantItemRecord.GetLastItemUID());

            m_idprovider = new UniqueIdProvider(lastUID);

            foreach (var method in typeof(ItemUses).GetMethods())
            {
                var attribute = method.GetCustomAttribute<ItemUsageHandlerAttribute>();

                if (attribute != null)
                {
                    m_usageHandlers.Add(attribute, method);
                }
            }

            foreach (var item in ItemRecord.GetItems())
            {
                item.Effects = new EffectCollection(item.Effects.Where(x => !IGNORED_EFFECTS.Contains(x.EffectEnum)));
            }
        }

        public int PopItemUID()
        {
            return m_idprovider.Pop();
        }

        public CharacterItemRecord CutItem(CharacterItemRecord item, int quantity, CharacterInventoryPositionEnum newItempos)
        {
            CharacterItemRecord newItem = (CharacterItemRecord)item.CloneWithoutUID();

            item.PositionEnum = newItempos;
            item.Quantity = quantity;
            newItem.Quantity -= quantity;

            item.UpdateElement();

            return newItem;
        }

        public CharacterItemRecord CreateCharacterItem(ItemRecord record, long characterId, int quantity, bool perfect = false)
        {
            return new CharacterItemRecord(characterId, 0, (short)record.Id, (byte)CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED, quantity, record.Effects.Generate(perfect),
                record.AppearenceId, record.Look);
        }


        public bool UseItem(Character character, CharacterItemRecord item)
        {
            if (!CriteriaManager.Instance.EvaluateCriterias(character.Client, item.Record.Criteria))
            {
                character.TextInformation(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 34);
                return false;
            }

            var function = m_usageHandlers.FirstOrDefault(x => x.Key.GId == item.GId);

            if (function.Value != null)
            {
                return (bool)function.Value.Invoke(null, new object[] { character, item });
            }
            else
            {
                function = m_usageHandlers.FirstOrDefault(x => x.Key.ItemType == item.Record.TypeEnum);
                if (function.Value != null)
                {
                    return (bool)function.Value.Invoke(null, new object[] { character, item });

                }
                foreach (var effect in item.Effects.OfType<Effect>())
                {
                    function = m_usageHandlers.FirstOrDefault(x => x.Key.Effect == effect.EffectEnum);
                    if (function.Value != null)
                    {
                        try
                        {
                            return (bool)function.Value.Invoke(null, new object[] { character, effect });
                        }
                        catch (Exception ex)
                        {
                            character.ReplyError(ex.ToString());
                            return false;
                        }


                    }
                    else
                    {
                        return false;

                    }
                }
                return false;

            }
        }
    }
    public class ItemUsageHandlerAttribute : Attribute
    {
        public EffectsEnum? Effect
        {
            get;
            set;
        }
        public short? GId
        {
            get;
            set;
        }
        public ItemTypeEnum? ItemType
        {
            get;
            set;
        }
        public ItemUsageHandlerAttribute(EffectsEnum effect)
        {
            this.Effect = effect;
        }
        public ItemUsageHandlerAttribute(short gid)
        {
            this.GId = gid;
        }
        public ItemUsageHandlerAttribute(ItemTypeEnum type)
        {
            this.ItemType = type;
        }
    }
}
