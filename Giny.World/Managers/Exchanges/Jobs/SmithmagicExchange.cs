using Giny.Core.DesignPattern;
using Giny.Core.Extensions;
using Giny.Core.Time;
using Giny.ORM;
using Giny.Protocol.Custom.Enums;
using Giny.Protocol.Enums;
using Giny.Protocol.Messages;
using Giny.World.Managers.Effects;
using Giny.World.Managers.Entities.Characters;
using Giny.World.Managers.Items;
using Giny.World.Records.Characters;
using Giny.World.Records.Items;
using Giny.World.Records.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Exchanges.Jobs
{
    public enum SmithMagicResultEnum
    {
        Critical,
        Decreased,
        Neutral,
    }
    public class SmithmagicExchange : JobExchange
    {
        private const double MaxWeigthPerLine = 101;

        private const ItemTypeEnum RuneType = ItemTypeEnum.SMITHMAGIC_RUNE;

        public SmithmagicExchange(Character character, SkillRecord skill) : base(character, skill)
        {

        }

        private CharacterItemRecord Rune
        {
            get;
            set;
        }

        private CharacterItemRecord TargetItem
        {
            get;
            set;
        }

        private double Pool
        {
            get;
            set;
        } = 0;

        public override void Ready(bool ready, short step)
        {
            if (Items.Count != 2)
            {
                return;
            }

            var probability = GetRuneSuccessProbability();

            var runeEffect = Rune.Effects.GetFirst<Effect>();

            AsyncRandom random = new AsyncRandom();

            var delta = random.NextDouble();

            SmithMagicResultEnum result = 0;

            double runeWeigth = GetRuneWeigth();

            double poolDelta = 0;

            if (delta < probability)
            {
                result = SmithMagicResultEnum.Critical;
            }

            else if (delta <= 0.5)
            {
                result = SmithMagicResultEnum.Neutral;
            }
            else
            {
                result = SmithMagicResultEnum.Decreased;
            }

            if (result == SmithMagicResultEnum.Neutral)
            {
                if (runeEffect.EffectEnum == EffectsEnum.Effect_AddAP_111 ||
                    runeEffect.EffectEnum == EffectsEnum.Effect_AddMP_128 ||
                    runeEffect.EffectEnum == EffectsEnum.Effect_AddRange)
                {
                    result = SmithMagicResultEnum.Decreased;
                }
            }

            bool effectAdded = false;

            if (result == SmithMagicResultEnum.Decreased)
            {
                if (Pool > runeWeigth)
                {
                    poolDelta = -runeWeigth;
                }
                else
                {
                    poolDelta = DecreaseItem() - runeWeigth;
                }
            }
            else if (result == SmithMagicResultEnum.Neutral)
            {
                if (Pool > runeWeigth)
                {
                    poolDelta = -runeWeigth;
                }
                else
                {
                    poolDelta = DecreaseItem() - runeWeigth;
                }

                if (CanAddEffect())
                {
                    AddRuneEffects(Rune, TargetItem);
                    effectAdded = true;
                }
            }
            else if (result == SmithMagicResultEnum.Critical)
            {
                if (CanAddEffect())
                {
                    AddRuneEffects(Rune, TargetItem);
                    effectAdded = true;
                }
            }

            if (effectAdded)
            {
                double n = Math.Abs(TargetItem.Record.Level / (double)CharacterJob.Level);
                double n2 = GetRuneWeigth();
                long exp = (long)(n * n2 * ConfigFile.Instance.JobRate);
                Character.AddJobExp(CharacterJob.JobId, exp);
            }

            Character.Inventory.RemoveItem(TargetItem.UId, 1);
            this.TargetItem = Character.Inventory.AddItem(TargetItem, 1);


            Character.Inventory.RemoveItem(Rune.UId, 1);
            Items.RemoveItem(Rune.UId, 1);

            MagicPoolStatusEnum poolStatus = 0;

            if (poolDelta > 0)
            {
                poolStatus = MagicPoolStatusEnum.INCREASED;
            }
            else if (poolDelta < 0)
            {
                poolStatus = MagicPoolStatusEnum.DECREASED;
            }
            else if (poolDelta == 0)
            {
                poolStatus = MagicPoolStatusEnum.UNMODIFIED;
            }

            Pool += poolDelta;

            if (Pool < 0)
            {
                Pool = 0;
                poolStatus = MagicPoolStatusEnum.UNMODIFIED;
            }

            OnResulted(TargetItem, poolStatus);
        }
        public override void MoveItem(int uid, int quantity)
        {
            base.MoveItem(uid, quantity);

            this.Rune = GetRune();
            this.TargetItem = GetTargetItem();
        }
        private double GetRuneWeigth()
        {
            return ItemsManager.Instance.GetRuneWeight(Rune.Record).Value;
        }
        private double DecreaseItem()
        {
            double runeWeigth = GetRuneWeigth();

            var runeEffect = Rune.Effects.GetFirst<Effect>();

            if (TargetItem.Effects.Count() == 0)
            {
                return 0;
            }

            double totalLoss = 0;

            var effects = TargetItem.Effects.Select(x => (EffectInteger)x).Shuffle().Take(3).ToList();

            if (effects.Any(x => x.EffectEnum == runeEffect.EffectEnum) && effects.Count() > 1)
            {
                effects.RemoveAll(x => x.EffectEnum == runeEffect.EffectEnum);
            }

            while (runeWeigth > 0)
            {
                foreach (EffectInteger effect in effects)
                {
                    if (effect.Value > 0)
                    {
                        var weigth = ItemsManager.Instance.GetEffectWeight(effect.EffectEnum);

                        if (weigth.HasValue)
                        {
                            runeWeigth -= weigth.Value;
                            effect.Value -= 1;
                            totalLoss += weigth.Value;

                            if (runeWeigth <= 0)
                            {
                                break;
                            }
                        }
                    }
                }

                if (effects.All(x => x.Value == 0))
                {
                    break;
                }
            }

            VerifyItemIntegrity();

            return totalLoss;
        }

        private void VerifyItemIntegrity()
        {
            foreach (var effect in TargetItem.Effects.OfType<EffectInteger>().ToArray())
            {
                if (effect.Value <= 0)
                {
                    TargetItem.Effects.Remove(effect);
                }
            }
        }
        private double GetRuneSuccessProbability()
        {
            var runeEffect = Rune.Effects.GetFirst<Effect>();

            if (runeEffect.EffectEnum == EffectsEnum.Effect_AddAP_111 ||
                runeEffect.EffectEnum == EffectsEnum.Effect_AddMP)
            {
                return 0.20d;
            }
            double ratio = CharacterJob.Level / (double)TargetItem.Record.Level;


            if (ratio < 0.1d)
            {
                ratio = 0.1d;
            }
            if (ratio > 0.80d)
            {
                ratio = 0.80d;
            }

            var itemEffect = TargetItem.Effects.GetFirst<EffectInteger>(runeEffect.EffectEnum);


            if (itemEffect != null)
            {
                var maxEffect = TargetItem.Record.Effects.GetFirst<EffectDice>().Max;

                double n = itemEffect.Value - maxEffect;

                if (n > 0)
                {
                    ratio -= n / maxEffect;
                }
            }


            return ratio;
        }

        private bool CanAddEffect()
        {
            EffectInteger runeEffect = Rune.Effects.GetFirst<EffectInteger>();

            EffectInteger itemEffect = TargetItem.Effects.GetFirst<EffectInteger>(runeEffect.EffectEnum);

            if (itemEffect == null)
            {
                return true;
            }
            double? weight = ItemsManager.Instance.GetEffectWeight(runeEffect.EffectEnum);

            double effective = itemEffect.Value * weight.Value;

            return effective + (runeEffect.Value * weight.Value) < MaxWeigthPerLine;
        }

        public void AddRuneEffects(CharacterItemRecord rune, CharacterItemRecord targetItem)
        {
            var addedEffect = rune.Effects.GetFirst<EffectInteger>();

            EffectInteger targetEffect = targetItem.Effects.GetFirst<EffectInteger>(addedEffect.EffectEnum);

            if (targetEffect != null)
            {
                targetEffect.Value += addedEffect.Value;
            }
            else
            {
                targetItem.Effects.Add((Effect)addedEffect.Clone());
            }

            targetItem.UpdateElement();

        }
        public override void SetCount(int count)
        {
            this.Count = count;
        }

        private CharacterItemRecord GetRune()
        {
            return Items.GetItems().FirstOrDefault(x => x.Record.TypeEnum == RuneType);
        }

        private CharacterItemRecord GetTargetItem()
        {
            return Items.GetItems().FirstOrDefault(x => x.Record.TypeEnum != RuneType);
        }

        private void OnResulted(CharacterItemRecord item, MagicPoolStatusEnum magicPoolStatus)
        {
            Character.Client.Send(new ExchangeCraftResultMagicWithObjectDescMessage()
            {
                craftResult = (byte)CraftResultEnum.CRAFT_SUCCESS,
                magicPoolStatus = (byte)magicPoolStatus,
                objectInfo = item.GetObjectItemNotInContainer(),
            });
        }
        public override void Close()
        {
            base.Close();
        }
    }
}
