using Giny.ORM.Attributes;
using Giny.ORM.Interfaces;
using Giny.Protocol.Custom.Enums;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Records.Npcs
{
    [Table("npcsactions")]
    public class NpcActionRecord : ITable
    {
        private static ConcurrentDictionary<long, NpcActionRecord> NpcsActions = new ConcurrentDictionary<long, NpcActionRecord>();

        [Primary]
        public long Id
        {
            get;
            set;
        }
        public long NpcSpawnId
        {
            get;
            set;
        }
        public NpcActionsEnum Action
        {
            get;
            set;
        }
        [TypeOverride("mediumtext")]
        public string Param1
        {
            get;
            set;
        }
        [TypeOverride("mediumtext")]
        public string Param2
        {
            get;
            set;
        }
        [TypeOverride("mediumtext")]
        public string Param3
        {
            get;
            set;
        }

        public static NpcActionRecord[] GetNpcActions(long npcSpawnId)
        {
            return NpcsActions.Values.Where(x => x.NpcSpawnId == npcSpawnId).ToArray();
        }

        public static long PopNextId()
        {
            return NpcsActions.Keys.OrderByDescending(x => x).First() + 1;
        }

        public override string ToString()
        {
            return Action.ToString();
        }
    }
}
