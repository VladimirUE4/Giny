using Giny.Protocol.Custom.Enums;
using Giny.World.Records.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Maps.Shapes
{
    public class Cross : IShape
    {
        public bool Diagonal
        {
            get;
            set;
        }
        public List<DirectionsEnum> DisabledDirections
        {
            get;
            set;
        }
        public bool OnlyPerpendicular
        {
            get;
            set;
        }
        public bool AllDirections
        {
            get;
            set;
        }
        public uint Surface
        {
            get
            {
                return (uint)(this.Radius * 4 + 1);
            }
        }
        public byte MinRadius
        {
            get;
            set;
        }
        public DirectionsEnum Direction
        {
            get;
            set;
        }
        public byte Radius
        {
            get;
            set;
        }
        public Cross(byte minRadius, byte radius)
        {
            this.MinRadius = minRadius;
            this.Radius = radius;
            this.DisabledDirections = new System.Collections.Generic.List<DirectionsEnum>();
        }
        public CellRecord[] GetCells(CellRecord centerCell, MapRecord map)
        {
            List<CellRecord> list = new List<CellRecord>();
            if (this.MinRadius == 0)
            {
                list.Add(centerCell);
            }
            System.Collections.Generic.List<DirectionsEnum> list2 = this.DisabledDirections.ToList<DirectionsEnum>();
            if (this.OnlyPerpendicular)
            {
                switch (this.Direction)
                {
                    case DirectionsEnum.DIRECTION_EAST:
                    case DirectionsEnum.DIRECTION_WEST:
                        list2.Add(DirectionsEnum.DIRECTION_EAST);
                        list2.Add(DirectionsEnum.DIRECTION_WEST);
                        break;
                    case DirectionsEnum.DIRECTION_SOUTH_EAST:
                    case DirectionsEnum.DIRECTION_NORTH_WEST:
                        list2.Add(DirectionsEnum.DIRECTION_SOUTH_EAST);
                        list2.Add(DirectionsEnum.DIRECTION_NORTH_WEST);
                        break;
                    case DirectionsEnum.DIRECTION_SOUTH:
                    case DirectionsEnum.DIRECTION_NORTH:
                        list2.Add(DirectionsEnum.DIRECTION_SOUTH);
                        list2.Add(DirectionsEnum.DIRECTION_NORTH);
                        break;
                    case DirectionsEnum.DIRECTION_SOUTH_WEST:
                    case DirectionsEnum.DIRECTION_NORTH_EAST:
                        list2.Add(DirectionsEnum.DIRECTION_NORTH_EAST);
                        list2.Add(DirectionsEnum.DIRECTION_SOUTH_WEST);
                        break;
                }
            }
            for (int i = (int)this.Radius; i > 0; i--)
            {
                if (i >= (int)this.MinRadius)
                {
                    if (!this.Diagonal)
                    {
                        if (!list2.Contains(DirectionsEnum.DIRECTION_SOUTH_EAST))
                        {
                            MapPoint.AddCellIfValid(centerCell.Point.X + i, centerCell.Point.Y, map, list);
                        }
                        if (!list2.Contains(DirectionsEnum.DIRECTION_NORTH_WEST))
                        {
                            MapPoint.AddCellIfValid(centerCell.Point.X - i, centerCell.Point.Y, map, list);
                        }
                        if (!list2.Contains(DirectionsEnum.DIRECTION_NORTH_EAST))
                        {
                            MapPoint.AddCellIfValid(centerCell.Point.X, centerCell.Point.Y + i, map, list);
                        }
                        if (!list2.Contains(DirectionsEnum.DIRECTION_SOUTH_WEST))
                        {
                            MapPoint.AddCellIfValid(centerCell.Point.X, centerCell.Point.Y - i, map, list);
                        }
                    }
                    if (this.Diagonal || this.AllDirections)
                    {
                        if (!list2.Contains(DirectionsEnum.DIRECTION_SOUTH))
                        {
                            MapPoint.AddCellIfValid(centerCell.Point.X + i, centerCell.Point.Y - i, map, list);
                        }
                        if (!list2.Contains(DirectionsEnum.DIRECTION_NORTH))
                        {
                            MapPoint.AddCellIfValid(centerCell.Point.X - i, centerCell.Point.Y + i, map, list);
                        }
                        if (!list2.Contains(DirectionsEnum.DIRECTION_EAST))
                        {
                            MapPoint.AddCellIfValid(centerCell.Point.X + i, centerCell.Point.Y + i, map, list);
                        }
                        if (!list2.Contains(DirectionsEnum.DIRECTION_WEST))
                        {
                            MapPoint.AddCellIfValid(centerCell.Point.X - i, centerCell.Point.Y - i, map, list);
                        }
                    }
                }
            }
            return list.ToArray();
        }
    }
}
