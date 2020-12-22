using Giny.ORM;
using Giny.World.Records.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Items
{
    public class MerchantSellerBag : ItemCollection<MerchantItemRecord>
    {
        public MerchantSellerBag(IEnumerable<MerchantItemRecord> items) : base(items)
        {
            this.OnItemRemoved += ItemRemoved;
            this.OnItemQuantityChanged += ItemQuantityChanged;
        }

        private void ItemQuantityChanged(MerchantItemRecord item, int quantity)
        {
            item.UpdateElement();
        }

        private void ItemRemoved(MerchantItemRecord item)
        {
            item.Sold = true;
            item.UpdateElement(); 
        }
    }
}
