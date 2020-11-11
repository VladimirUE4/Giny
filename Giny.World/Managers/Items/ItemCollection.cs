using Giny.Protocol.Enums;
using Giny.Protocol.Types;
using Giny.World.Managers.Effects;
using Giny.World.Records.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Items
{
    public class ItemCollection<T> where T : AbstractItem
    {
        private List<T> m_items = new List<T>(); /* Should be Dictionary<UId,Item> */

        public int Count
        {
            get
            {
                return m_items.Count;
            }
        }
        public ItemCollection(IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                m_items.Add(item);
            }
        }
        public ItemCollection()
        {

        }
        public T[] GetItems()
        {
            return this.m_items.ToArray();
        }
        public T[] GetItems(Func<T, bool> predicate)
        {
            return this.m_items.Where(predicate).ToArray();
        }

        public event Action<T> OnItemAdded;

        public event Action<T, int> OnItemStacked;

        public event Action<T> OnItemRemoved;

        public event Action<T, int> OnItemUnstacked;

        public event Action<IEnumerable<T>> OnItemsAdded;

        public event Action<IEnumerable<T>> OnItemsStackeds;

        public event Action<IEnumerable<T>> OnItemsRemoved;

        public event Action<IEnumerable<T>> OnItemsUnstackeds;

        public event Action<T, int> OnItemQuantityChanged;

        public event Action<IEnumerable<T>> OnItemsQuantityChanged;

        public virtual void AddItems(IEnumerable<T> items)
        {
            List<T> addedItems = new List<T>();
            List<T> stackedItems = new List<T>();

            foreach (var item in items)
            {
                item.Initialize();

                T sameItem = GetSameItem(item.GId, item.Effects);

                if (sameItem != null)
                {
                    sameItem.Quantity += item.Quantity;
                    stackedItems.Add(item);

                }
                else
                {
                    m_items.Add(item);
                    addedItems.Add(item);
                }
            }

            if (OnItemAdded != null)
                OnItemsAdded(addedItems);

            if (OnItemStacked != null)
                OnItemsStackeds(stackedItems);

            if (OnItemsQuantityChanged != null)
                OnItemsQuantityChanged(stackedItems);

        }
        public virtual void RemoveItems(Dictionary<int, int> pairs)
        {
            List<T> removedItems = new List<T>();
            List<T> unstackedItems = new List<T>();

            foreach (var info in pairs)
            {
                T item = GetItem(info.Key);

                if (item != null)
                {
                    if (item.Quantity == info.Value)
                    {
                        m_items.Remove(item);
                        removedItems.Add(item);
                    }
                    else
                    {
                        item.Quantity -= info.Value;
                        unstackedItems.Add(item);
                    }
                }
            }

            if (OnItemsRemoved != null)
                OnItemsRemoved(removedItems);

            if (OnItemsUnstackeds != null)
                OnItemsUnstackeds(unstackedItems);

            if (OnItemsQuantityChanged != null)
                OnItemsQuantityChanged(unstackedItems);

        }
        public virtual void AddItem(T item)
        {
            item.Initialize();

            T sameItem = GetSameItem(item.GId, item.Effects);

            if (sameItem != null)
            {
                sameItem.Quantity += item.Quantity;
                if (OnItemStacked != null)
                    OnItemStacked(sameItem, item.Quantity);

                if (OnItemQuantityChanged != null)
                    OnItemQuantityChanged(sameItem, item.Quantity);
            }
            else
            {
                m_items.Add(item);
                if (OnItemAdded != null)
                    OnItemAdded(item);
            }
        }

        public virtual void AddItem(T item, int quantity)
        {
            item.Initialize();

            T sameItem = GetSameItem(item.GId, item.Effects);

            if (sameItem != null)
            {
                sameItem.Quantity += quantity;
                if (OnItemStacked != null)
                    OnItemStacked(sameItem, quantity);

                if (OnItemQuantityChanged != null)
                    OnItemQuantityChanged(sameItem, quantity);
            }
            else
            {
                item = (T)item.CloneWithUID();
                item.Quantity = quantity;
                m_items.Add(item);

                if (OnItemAdded != null)
                    OnItemAdded(item);
            }
        }
        public virtual void RemoveItem(T item, int quantity)
        {
            if (item != null)
            {
                if (item.PositionEnum != CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED)
                    return;

                if (item.Quantity >= quantity)
                {
                    if (item.Quantity == quantity)
                    {
                        m_items.Remove(item);
                        if (OnItemRemoved != null)
                            OnItemRemoved(item);
                    }
                    else
                    {
                        item.Quantity -= quantity;
                        if (OnItemUnstacked != null)
                            OnItemUnstacked(item, quantity);

                        if (OnItemQuantityChanged != null)
                            OnItemQuantityChanged(item, quantity);
                    }
                }
            }

        }
        public void RemoveItem(int uid)
        {
            T item = GetItem(uid);
            RemoveItem(item, item.Quantity);
        }
        public void RemoveItem(int uid, int quantity)
        {
            T item = GetItem(uid);
            RemoveItem(item, quantity);
        }
        public bool Contains(T item)
        {
            return m_items.Contains(item);
        }
        protected virtual T GetSameItem(short gid, List<Effect> effects)
        {
            return GetItems().FirstOrDefault(x => x.GId == gid && SameEffects(effects, x.Effects));
        }
        public T GetItem(int uid)
        {
            return m_items.FirstOrDefault(x => x.UId == uid); // erf
        }
        public T GetItem(short gid, List<Effect> effects)
        {
            return GetSameItem(gid, effects);
        }
        public static bool SameEffects(List<Effect> e1, List<Effect> e2)
        {
            return e1.SequenceEqual(e2);
        }
        public ObjectItem[] GetObjectsItems()
        {
            var array = Array.ConvertAll<T, ObjectItem>(this.GetItems(), x => x.GetObjectItem());
            return array;
        }
        public bool Exist(short gid, int minimumQuantity)
        {
            return m_items.FirstOrDefault(x => x.GId == gid && x.Quantity >= minimumQuantity) != null;
        }
        public bool Exist(short gId)
        {
            return m_items.FirstOrDefault(x => x.GId == gId) != null;
        }
        public static Dictionary<List<T>, List<Effect>> SortByEffects(IEnumerable<T> items)
        {
            Dictionary<List<T>, List<Effect>> results = new Dictionary<List<T>, List<Effect>>();
            foreach (var item in items)
            {
                var same = results.FirstOrDefault(x => SameEffects(x.Value, item.Effects));

                if (same.Key == null)
                    results.Add(new List<T>() { item }, item.Effects);
                else
                    same.Key.Add(item);
            }
            return results;
        }
    }
}
