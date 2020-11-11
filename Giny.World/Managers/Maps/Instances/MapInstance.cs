﻿using Giny.Core.Network.Messages;
using Giny.Core.Pool;
using Giny.Protocol.Messages;
using Giny.Protocol.Types;
using Giny.World.Managers.Entities;
using Giny.World.Managers.Entities.Characters;
using Giny.World.Managers.Fights;
using Giny.World.Managers.Maps.Elements;
using Giny.World.Managers.Monsters;
using Giny.World.Managers.Skills;
using Giny.World.Network;
using Giny.World.Records;
using Giny.World.Records.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Giny.World.Managers.Generic;
using Giny.World.Managers.Entities.Merchants;
using Giny.Core.Time;

namespace Giny.World.Managers.Maps.Instances
{
    public abstract class MapInstance
    {
        public int MonsterGroupCount
        {
            get
            {
                return GetEntities<MonsterGroup>().Count();
            }
        }

        public int CharactersCount
        {
            get
            {
                return GetEntities<Character>().Count();
            }
        }

        private List<MapElement> m_elements = new List<MapElement>();

        private Dictionary<long, Entity> m_entities = new Dictionary<long, Entity>();

        private ActionTimer m_monsterSpawner;

        private Dictionary<int, Fight> m_fights = new Dictionary<int, Fight>();

        //private List<DropItem> m_droppedItems = new List<DropItem>();

        private ReversedUniqueIdProvider m_npIdPopper = new ReversedUniqueIdProvider(0);

        private UniqueIdProvider m_dropItemIdPopper = new UniqueIdProvider(0);

        public int PopNextDropItemId()
        {
            return m_dropItemIdPopper.Pop();
        }
        public MapRecord Record
        {
            get;
            private set;
        }

        public bool Mute = false;

        public MapInstance(MapRecord record)
        {
            this.Record = record;
            this.m_elements = Record.Elements.Where(x => x.Skill != null).Select(x => x.GetMapElement(this)).ToList();
            this.m_monsterSpawner = new ActionTimer(MonstersManager.MonsterSpawningPoolInterval, SpawnMonsterGroup, true);
        }

        private void SpawnMonsterGroup()
        {
            if (this.MonsterGroupCount < MonstersManager.MAX_MONSTER_GROUP_PER_MAP)
            {
                AsyncRandom rd = new AsyncRandom();

                if (Record.Subarea.Monsters.Length > 0)
                    MonstersManager.Instance.SpawnMonsterGroup(this.Record, rd);
            }
        }

        public bool MonsterGroupExists(MonsterGroup monsterGroup)
        {
            foreach (var group in GetEntities<MonsterGroup>())
            {
                if (group.GetMonsters().All(x => monsterGroup.GetMonsters().Select(monster => monster.Record.Id).Contains(x.Record.Id)))
                {
                    return true;
                }
            }
            return false;
        }

        public void Reload()
        {
            this.m_elements = Record.Elements.Where(x => x.Skill != null).Select(x => x.GetMapElement(this)).ToList();

            foreach (var character in GetEntities<Character>())
            {
                character.Client.Send(GetMapComplementaryInformationsDataMessage(character));
            }

        }

        public void AddEntity(Entity entity)
        {
            lock (this)
            {
                if (!m_entities.ContainsKey(entity.Id))
                {
                    var informations = entity.GetActorInformations();
                    Send(new GameRolePlayShowActorMessage(informations));

                    m_entities.Add(entity.Id, entity);
                    OnEntitiesUpdated();
                }
            }
        }
        private void OnEntitiesUpdated()
        {
            if (Record.CanSpawnMonsters)
            {
                if (CharactersCount == 0)
                    this.m_monsterSpawner.Pause();
                else if (!m_monsterSpawner.Started)
                    this.m_monsterSpawner.Start();

                m_monsterSpawner.Interval = MonstersManager.MonsterSpawningPoolInterval * (MonsterGroupCount + 1);
            }
        }
        public void RemoveEntity(long entityId)
        {
            lock (this)
            {
                m_entities.Remove(entityId);
                this.Send(new GameContextRemoveElementMessage(entityId));
                OnEntitiesUpdated();
            }
        }
        public Entity[] GetEntities()
        {
            return m_entities.Values.ToArray();
        }
        public T[] GetEntities<T>() where T : Entity
        {
            return m_entities.Values.OfType<T>().ToArray();
        }
        public T GetEntity<T>(long id) where T : Entity
        {
            if (!m_entities.ContainsKey(id))
            {
                return null;
            }
            return m_entities[id] as T;
        }

        public void AddFight(Fight fight)
        {
            m_fights.Add(fight.Id, fight);

            if (fight.ShowBlades)
                Send(new GameRolePlayShowChallengeMessage(fight.GetFightCommonInformations()));

            SendMapFightCount();
        }

        public Fight GetFight(short fightId)
        {
            Fight result = null;
            m_fights.TryGetValue(fightId, out result);
            return result;
        }

        public void RemoveFight(Fight fight)
        {
            m_fights.Remove(fight.Id);

            RemoveBlades(fight);

            SendMapFightCount();
        }
        public void RemoveBlades(Fight fight)
        {
            if (fight.ShowBlades)
                Send(new GameRolePlayRemoveChallengeMessage((short)fight.Id));
        }
        public void SendMapFightCount()
        {
            Send(new MapFightCountMessage((short)m_fights.Count));
        }


        public bool IsCellFree(short cellId)
        {
            foreach (var entity in GetEntities<Entity>())
            {
                if (entity.CellId == cellId)
                    return false;
            }
            return true;
        }
        public bool IsCellFree(short cellId, short exclude)
        {
            foreach (var entity in GetEntities<Entity>())
            {
                if (entity.CellId == exclude)
                {
                    continue;
                }
                if (entity.CellId == cellId)
                    return false;
            }
            return true;
        }

        public T GetEntity<T>(Func<T, bool> predicate) where T : Entity
        {
            return (T)m_entities.Values.OfType<T>().FirstOrDefault(predicate);
        }
        public Entity GetEntity(long id)
        {
            return GetEntity<Entity>(id);
        }

        public void EntityTalk(Entity entity, string message)
        {
            Send(new EntityTalkMessage((double)entity.Id, 4, new string[] { message }));
        }

        public void SendMapComplementary(WorldClient client)
        {
            client.Send(GetMapComplementaryInformationsDataMessage(client.Character));
        }
        protected GameRolePlayActorInformations[] GetGameRolePlayActorsInformations()
        {
            return m_entities.Values.Select(x => x.GetActorInformations()).ToArray();
        }
        protected MapObstacle[] GetMapObstacles()
        {
            return new MapObstacle[0];
        }
        protected HouseInformations[] GetHousesInformations()
        {
            return new HouseInformations[0];
        }
        protected bool HasAgressiveMonsters()
        {
            return false;
        }
        public abstract MapComplementaryInformationsDataMessage GetMapComplementaryInformationsDataMessage(Character character);

        public long PopNextNPEntityId()
        {
            return (long)m_npIdPopper.Pop();
        }

        public void Send(NetworkMessage message)
        {
            lock (this)
            {
                foreach (var character in GetEntities<Character>())
                {
                    character.Client.Send(message);
                }
            }
        }
        public void ToggleMute()
        {
            Mute = !Mute;
        }
        public MapElement GetElement<T>(int identifier)
        {
            return (MapElement)m_elements.FirstOrDefault(x => x.Record.Identifier == identifier);
        }
        public IEnumerable<T> GetElements<T>() where T : MapElement
        {
            return m_elements.OfType<T>();
        }

        protected InteractiveElement[] GetInteractiveElements(Character character)
        {
            return GetElements<MapInteractiveElement>().Select(x => x.GetInteractiveElement(character)).ToArray(); // todo create mapinteractiveelement and mapstatedelement
        }
        protected StatedElement[] GetStatedElements()
        {
            return GetElements<MapStatedElement>().Select(x => x.GetStatedElement()).ToArray();
        }
        protected FightCommonInformations[] GetFightsCommonInformations()
        {
            return m_fights.Values.Where(x => !x.Started).Select(x => x.GetFightCommonInformations()).ToArray();
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (var entity in m_entities.Values)
            {
                sb.Append(entity.ToString() + "\n");
            }
            return sb.ToString();
        }
        /// <summary>
        /// yet, i dont think more then one skill by element is effective for a private server
        /// </summary>
        public void UseInteractive(Character character, int elemId, int skillInstanceUid)
        {
            var element = this.GetElement<MapElement>(elemId);

            if (element != null && element.CanUse(character) && !character.Busy)
            {
                if (element != null)
                {
                    if (GenericActionsManager.Instance.IsHandled(element))
                    {
                        bool canMove = element.Record.Skill.SkillRecord.ParentJobId == 1; /* Should be working */

                        short duration = canMove ? (short)0 : SkillsManager.SKILL_DURATION; /* Duration should be related to job level (its not a const) */

                        character.SendMap(new InteractiveUsedMessage(character.Id, elemId, (short)element.Record.Skill.SkillId, duration, canMove));
                        GenericActionsManager.Instance.Handle(character, element);
                    }
                    else
                    {
                        character.Client.Send(new InteractiveUseErrorMessage(elemId, skillInstanceUid));
                    }
                }
                else
                {
                    character.Client.Send(new InteractiveUseErrorMessage(elemId, skillInstanceUid));
                }
            }
            else
            {
                character.Client.Send(new InteractiveUseErrorMessage(elemId, skillInstanceUid));
            }
        }

        public bool IsMerchantLimitReached()
        {
            return GetEntities<CharacterMerchant>().Count() >= ConfigFile.Instance.MaxMerchantPerMap;
        }

        /* public void RemoveDropItem(DropItem dropItem)
         {
             m_droppedItems.Remove(dropItem);
             m_dropItemIdPopper.Push(dropItem.Id);
             this.Send(new ObjectGroundRemovedMessage(dropItem.CellId));

         }

         public DropItem GetDroppedItem(ushort cellId)
         {
             return m_droppedItems.FirstOrDefault(x => x.CellId == cellId);
         }

         public DropItem[] GetDroppedItems()
         {
             return m_droppedItems.ToArray();
         }

         public void OnElementsUpdated()
         {
             foreach (var character in GetEntities<Character>())
             {
                 var elements = Array.ConvertAll(m_interactiveElements.ToArray(), x => x.GetInteractiveElement(character));

                 foreach (var element in elements)
                 {
                     character.Client.Send(new InteractiveElementUpdatedMessage(element));
                 }
             }
         } */
    }
}
