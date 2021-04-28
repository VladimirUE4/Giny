using Giny.Core.DesignPattern;
using Giny.Core.Time;
using Giny.ORM;
using Giny.Pokefus.Effects;
using Giny.Pokefus.Fight.Fighters;
using Giny.Protocol.Enums;
using Giny.World.Managers.Effects;
using Giny.World.Managers.Entities.Characters;
using Giny.World.Managers.Fights;
using Giny.World.Managers.Fights.Fighters;
using Giny.World.Managers.Fights.Results;
using Giny.World.Managers.Items;
using Giny.World.Managers.Items.Collections;
using Giny.World.Records.Items;
using Giny.World.Records.Maps;
using Giny.World.Records.Monsters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.Pokefus
{
    public class PokefusManager : Singleton<PokefusManager>
    {
        private const short RegularItemId = 15710;

        private const short MiniBossItemId = 15161;

        private const short BossItemId = 15393;

        private ItemRecord m_regularItemRecord;
        private ItemRecord m_miniBossItemRecord;
        private ItemRecord m_bossItemRecord;

        public const short MaxPokefusLevel = 200;

        private const string PokefusLevelUpMessage = "Felicitation ! Votre Pokefus {0} passe niveau {1}";

        private const string DropInfoMessage = "Chance de drop pour {0} : {1} %";

        private const string UndroppableMessage = "Le monstre {0} n'a pas d'âme ...Il est impossible de drop son pokéfus !";

        private static short[] ForbiddenMonsters = new short[]
       {
            232,233,494, 1003,216, 3578, 4275, 113, 4362, 4363, 4364, 499, 2819, 3138, 588, 4373,
            424,2954,3141,1181,3760,939,4619,116, 158, 216, 229,250,384,387, 424,499,557,558,559,
            3142,560, 654, 667,670,676,682,802, 831,872,890, 1027,1088,1094, 1140, 1145,1169,
            1181,3138,1194,1195,1231,1233,2384,2424,2493, 2495,2538,2666,2762,2794,2795,2971,2977,
            2978,2979,3101,2819,2822,2827,2906,2941,2951,2954,2955, 2967,3143,3144,3165,3166,3167,
            3173,3174,3217,3293,3294,3295,3296,3297,3298,3341,3362,3398,3413,3433,3457,3461,3519,
            3528,3539,3541,3553,3556,3560,3619,3632,3644,3648,3655,3657,3668,3669, 3677,3678,3680,
            3681,3690,3692,3711,3714, 3716,3726,3735,3749, 3751,3760,3769, 3770,3773,3781,3804,3826,
            3828,3851,3854,3882,3907,3918,3923, 3943,3991,3998,4016, 4017,4020,4024,4034, 4068,4071,
            4074, 4126,4164,4172,4216,4226,4228,4235,4245,4256,4270,4298,4299,4331,4332,4362,4363,
            4364,4442,4451,4456, 4487,4499, 4505,4512, 4523,4552,4559, 4597, 4619,4662,4677,4684,
            4685, 4695,4710,2793, 1044,4139,422,3543,1145,1169, 407,839,2636,1184,1185,1186,1188,1187,
            4460,1070,2570,666,3590,3592,3589,3591,3588,3234,1072,1085,1086,1087,4359,1050,3803,3561
       };

        public void Initialize()
        {
            m_regularItemRecord = ItemRecord.GetItem(RegularItemId);
            m_miniBossItemRecord = ItemRecord.GetItem(MiniBossItemId);
            m_bossItemRecord = ItemRecord.GetItem(BossItemId);
        }

        public void OnHumanOptionsCreated(Character character)
        {
            foreach (var item in GetPokefusItems(character.Inventory))
            {
                EffectPokefus pokefusEffect = item.Effects.Get<EffectPokefus>();
                MonsterRecord monsterRecord = MonsterRecord.GetMonsterRecord(pokefusEffect.MonsterId);
                character.AddFollower(monsterRecord.Look);
            }
        }
        public void OnPlayerResultApplied(FightPlayerResult result)
        {
            AsyncRandom random = new AsyncRandom();

            if (result.Fight.Winners == result.Fighter.Team)
            {
                var monsters = result.Fighter.EnemyTeam.GetFighters<MonsterFighter>(false);

                foreach (var monster in monsters)
                {
                    if (ForbiddenMonsters.Contains((short)monster.Record.Id))
                    {
                        result.Character.Reply(string.Format(UndroppableMessage, monster.Name), Color.CornflowerBlue);
                        continue;
                    }
                    var chance = (random.Next(0, 100) + random.NextDouble());
                    var dropRate = GetDropRate(monster, result.Fighter);

                    if (!(dropRate >= chance))
                        continue;

                    CharacterItemRecord item = CreatePokefusItem(result.Character.Id, monster.Record, monster.Monster.Grade.GradeId);
                    result.Loot.AddItem(item.GId, item.Quantity);
                    result.Character.Inventory.AddItem(item);
                }

                AddPokefusExperience(result);



            }
        }
        private void AddPokefusExperience(FightPlayerResult result)
        {
            var items = result.Character.Inventory.GetEquipedItems().Where(x => x.Effects.Exists<EffectPokefus>());

            if (items.Count() == 0)
            {
                return;
            }
            var delta = result.ExperienceData.ExperienceFightDelta / items.Count();

            foreach (var item in items)
            {
                EffectPokefus effect = item.Effects.Get<EffectPokefus>();

                EffectPokefusLevel effectLevel = item.Effects.Get<EffectPokefusLevel>();

                var level = effectLevel.Level;

                effectLevel.AddExperience(delta);

                if (level != effectLevel.Level)
                {
                    result.Character.DisplayPopup(0, "Serveur", string.Format(PokefusLevelUpMessage, effect.MonsterName, effectLevel.Level));
                }

                result.Character.Inventory.OnItemModified(item);

                item.UpdateElement();
            }

        }
        private double GetDropRate(MonsterFighter monster, CharacterFighter fighter)
        {
            double probability = 0.05d;

            if (monster.Level >= 50)
            {
                probability = 0.04;
            }
            if (monster.Level >= 100)
            {
                probability = 0.03;
            }
            if (monster.Level >= 150)
            {
                probability = 0.02;
            }
            if (monster.Level >= 200)
            {
                probability = 0.01;
            }
            if (monster.Record.IsBoss)
            {
                probability = 0.008;
            }

            probability += (fighter.Level / 200) / 20;


            probability += (fighter.Stats.Prospecting.TotalInContext() / 5000);

            var percentage = Math.Round(probability * 100d, 2);

            fighter.Character.Reply(string.Format(DropInfoMessage, monster.Name, percentage), Color.CornflowerBlue);

            return percentage;
        }
        public void OnFighterJoined(Fighter fighter)
        {
            if (!(fighter is CharacterFighter))
                return;

            CharacterFighter characterFighter = (CharacterFighter)fighter;

            foreach (var pokefusItem in GetPokefusItems(characterFighter.Character.Inventory))
            {
                EffectPokefus effect = pokefusItem.Effects.Get<EffectPokefus>();
                MonsterRecord monsterRecord = MonsterRecord.GetMonsterRecord(effect.MonsterId);

                CellRecord cell = fighter.Team.GetPlacementCell();

                PokefusFighter pokefusFighter = new PokefusFighter(characterFighter, pokefusItem, monsterRecord, null, effect.GradeId, cell);

                fighter.Team.AddFighter(pokefusFighter);
            }
        }

        private IEnumerable<CharacterItemRecord> GetPokefusItems(Inventory inventory)
        {
            return inventory.GetEquipedItems().Where(x => IsPokefusItem(x));
        }
        private bool IsPokefusItem(CharacterItemRecord item)
        {
            return item.Effects.Exists<EffectPokefus>();
        }
        public CharacterItemRecord CreatePokefusItem(long characterId, MonsterRecord monster, byte monsterGradeId)
        {
            ItemRecord itemRecord;

            if (monster.IsMiniBoss)
            {
                itemRecord = m_miniBossItemRecord;
            }
            else if (monster.IsBoss)
            {
                itemRecord = m_bossItemRecord;
            }
            else
            {
                itemRecord = m_regularItemRecord;
            }

            CharacterItemRecord item = ItemManager.Instance.CreateCharacterItem(itemRecord, characterId, 1);

            item.Effects.Clear();

            item.Effects.Add(new EffectPokefus((short)monster.Id, monster.Name, monsterGradeId));
            item.Effects.Add(new EffectPokefusLevel(0));
            item.Effects.Add(new EffectInteger(EffectsEnum.Effect_Followed, (int)monster.Id));

            return item;
        }


    }
}
