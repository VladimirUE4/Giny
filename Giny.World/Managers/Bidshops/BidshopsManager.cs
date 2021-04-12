using Giny.Core.DesignPattern;
using Giny.ORM;
using Giny.World.Managers.Entities.Characters;
using Giny.World.Records.Bidshops;
using Giny.World.Records.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Bidshops
{
    public class BidshopsManager : Singleton<BidshopsManager> // create BidShop instances, containing dialogs.
    {
        private Dictionary<long, Dictionary<long, BidShopItemRecord>> m_bidshopItems = new Dictionary<long, Dictionary<long, BidShopItemRecord>>();

        [StartupInvoke("Bidshops", StartupInvokePriority.SixthPath)]
        public void Initialize()
        {
            foreach (var bidshop in BidShopRecord.GetBidShops())
            {
                m_bidshopItems.Add(bidshop.Id, new Dictionary<long, BidShopItemRecord>());
            }

            foreach (var item in BidShopItemRecord.GetItems())
            {
                m_bidshopItems[item.BidShopId].Add(item.UId, item);
            }
        }
        public void AddItem(long bidshopId, BidShopItemRecord item)
        {
            lock (m_bidshopItems)
            {
                m_bidshopItems[bidshopId].Add(item.UId, item);
                item.AddElement();
            }
        }
        public void RemoveItem(long bidshopId, BidShopItemRecord item)
        {
            lock (m_bidshopItems)
            {
                m_bidshopItems[bidshopId].Remove(item.UId);
                item.RemoveElement();
            }
        }
        public IEnumerable<BidShopItemRecord> GetItems(long bidshopId)
        {
            lock (m_bidshopItems)
            {
                return m_bidshopItems[bidshopId].Values.Where(x => !x.Sold);
            }
        }
        public BidShopItemRecord GetItem(long bidshopId, int uid)
        {
            lock (m_bidshopItems)
            {
                BidShopItemRecord result = null;
                m_bidshopItems[bidshopId].TryGetValue(uid, out result);
                return result;
            }
        }

        public IEnumerable<BidShopItemRecord> GetSellerItems(long bidshopId, int accountId)
        {
            lock (m_bidshopItems)
            {
                return m_bidshopItems[bidshopId].Values.Where(x => x.AccountId == accountId);
            }
        }

        public IEnumerable<BidShopItemRecord> GetSoldItem(Character character)
        {
            return BidShopItemRecord.GetItems().Where(x => x.Sold && x.AccountId == character.Client.Account.Id);
        }
    }
}
