﻿using Giny.Protocol.Custom.Enums;
using Giny.World.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Criterias
{
    [Criteria("PO")]
    public class HasItemCriteria : Criteria
    {
        public override bool Eval(WorldClient client)
        {
            var criteria = CriteriaFull.Remove(0, 3).Split(',');
            int quantity = 1;
            short gid = short.Parse(criteria[0]);

            if (criteria.Length > 1)
            {
                quantity = int.Parse(criteria[1]);
            }

            return client.Character.Inventory.Exist(gid, quantity);
        }
    }
    [Criteria("PT")]
    public class HasItemOfTypeCriteria : Criteria
    {

        public override bool Eval(WorldClient client)
        {
            var type = (ItemTypeEnum)int.Parse(CriteriaValue);
            var item = client.Character.Inventory.GetFirstItem(type);
            return item != null;
        }
    }
}
