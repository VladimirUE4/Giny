using Giny.Core.DesignPattern;
using Giny.ORM.Attributes;
using Giny.ORM.Interfaces;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Records.Maps
{
    [WIP]
    public class DungeonRecord : ITable
    {
        [Primary]
        public long Id
        {
            get;
            set;
        }
        [Update]
        public string Name
        {
            get;
            set;
        }
        [Update]
        public int EntranceMapId
        {
            get;
            set;
        }
        [Update]
        public int ExitMapId
        {
            get;
            set;
        }

        [ProtoSerialize]
        [Update]
        public List<int> Maps
        {
            get;
            set;
        }

        [ProtoSerialize]
        [Update]
        public List<MonsterRoom> Monsters
        {
            get;
            set;
        }
        
    }

    [ProtoContract]
    public class MonsterRoom
    {
        [ProtoMember(1)]
        public List<short> MonsterIds
        {
            get;
            set;
        }
    }
}
