using Giny.Protocol.Custom.Enums;
using Giny.World.Records.Maps;
using System;
using System.Collections.Generic;

namespace Giny.World.Managers.Maps.Shapes
{
    public class HalfLozenge : IShape
    {
        public HalfLozenge(byte minRadius, byte radius)
        {
            MinRadius = minRadius;
            Radius = radius;

            Direction = DirectionsEnum.DIRECTION_NORTH;
        }

        public uint Surface
        {
            get
            {
                return (uint)Radius * 2 + 1;
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

        public CellRecord[] GetCells(CellRecord centerCell, MapRecord map)
        {
            var centerPoint = centerCell.Point;
            var result = new List<CellRecord>();

            if (MinRadius == 0)
                result.Add(centerCell);

            for (int i = 1; i <= Radius; i++)
            {
                switch (Direction)
                {
                    case DirectionsEnum.DIRECTION_NORTH_WEST:
                        AddCellIfValid(centerPoint.X + i, centerPoint.Y + i, map, result);
                        AddCellIfValid(centerPoint.X + i, centerPoint.Y - i, map, result);
                        break;

                    case DirectionsEnum.DIRECTION_NORTH_EAST:
                        AddCellIfValid(centerPoint.X - i, centerPoint.Y - i, map, result);
                        AddCellIfValid(centerPoint.X + i, centerPoint.Y - i, map, result);
                        break;

                    case DirectionsEnum.DIRECTION_SOUTH_EAST:
                        AddCellIfValid(centerPoint.X - i, centerPoint.Y + i, map, result);
                        AddCellIfValid(centerPoint.X - i, centerPoint.Y - i, map, result);
                        break;

                    case DirectionsEnum.DIRECTION_SOUTH_WEST:
                        AddCellIfValid(centerPoint.X - i, centerPoint.Y + i, map, result);
                        AddCellIfValid(centerPoint.X + i, centerPoint.Y + i, map, result);
                        break;
                }
            }

            return result.ToArray();
        }

        private static void AddCellIfValid(int x, int y, MapRecord map, IList<CellRecord> container)
        {
            if (!MapPoint.IsInMap(x, y))
                return;

            container.Add(map.Cells[MapPoint.CoordToCellId(x, y)]);
        }
    }
}
