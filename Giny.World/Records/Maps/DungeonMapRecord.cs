using Giny.Core.DesignPattern;
using Giny.ORM.Attributes;
using Giny.ORM.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Records.Maps
{
    [Table("dungeons")]
    public class DungeonMapRecord : ITable
    {
        private static Dictionary<long, DungeonMapRecord> Dungeons = new Dictionary<long, DungeonMapRecord>();

        [Primary]
        public long Id
        {
            get;
            set;
        }

        [Ignore]
        public long MapId => Id;

        public long NextMapId
        {
            get;
            set;
        }

        public List<short> Monsters
        {
            get;
            set;
        }
        /// <summary>
        /// Seconds
        /// </summary>
        public float RespawnDelay
        {
            get;
            set;
        }

        public int GetRespawnInterval()
        {
            return (int)(RespawnDelay * 1000);
        }
        public static DungeonMapRecord GetDungeonMap(long id)
        {
            DungeonMapRecord result = null;

            if (Dungeons.TryGetValue(id,out result))
            {
                return result;
            }
            else
            {
                return null;
            }
        }
    }
}
