using System;
using Giny.Core.IO.Interfaces;
using Giny.IO.D2O;
using Giny.IO.D2OTypes;
using System.Collections.Generic;

namespace Giny.IO.D2OClasses
{
    [D2OClass("Hint", "com.ankamagames.dofus.datacenter.world")]
    public class Hint : IDataObject , IIndexedData
    {
        public const string MODULE = "Hints";

        public int Id => (int)id;

        public int id;
        public uint gfx;
        public uint nameId;
        public double mapId;
        public double realMapId;
        public int x;
        public int y;
        public bool outdoor;
        public int subareaId;
        public int worldMapId;
        public int categoryId;
        public uint level;

        [D2OIgnore]
        public int Id_
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
            }
        }
        [D2OIgnore]
        public uint Gfx
        {
            get
            {
                return gfx;
            }
            set
            {
                gfx = value;
            }
        }
        [D2OIgnore]
        public uint NameId
        {
            get
            {
                return nameId;
            }
            set
            {
                nameId = value;
            }
        }
        [D2OIgnore]
        public double MapId
        {
            get
            {
                return mapId;
            }
            set
            {
                mapId = value;
            }
        }
        [D2OIgnore]
        public double RealMapId
        {
            get
            {
                return realMapId;
            }
            set
            {
                realMapId = value;
            }
        }
        [D2OIgnore]
        public int X
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
            }
        }
        [D2OIgnore]
        public int Y
        {
            get
            {
                return y;
            }
            set
            {
                y = value;
            }
        }
        [D2OIgnore]
        public bool Outdoor
        {
            get
            {
                return outdoor;
            }
            set
            {
                outdoor = value;
            }
        }
        [D2OIgnore]
        public int SubareaId
        {
            get
            {
                return subareaId;
            }
            set
            {
                subareaId = value;
            }
        }
        [D2OIgnore]
        public int WorldMapId
        {
            get
            {
                return worldMapId;
            }
            set
            {
                worldMapId = value;
            }
        }
        [D2OIgnore]
        public uint Level
        {
            get
            {
                return level;
            }
            set
            {
                level = value;
            }
        }

    }
}






