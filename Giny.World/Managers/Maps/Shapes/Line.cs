using Giny.Protocol.Custom.Enums;
using Giny.World.Records.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Maps.Shapes
{
    public class Line : IShape
    {
        public Line(byte radius, bool opposedDirection)
        {
            Radius = radius;
            Direction = DirectionsEnum.DIRECTION_SOUTH_EAST;
            OpposedDirection = opposedDirection;
        }

        #region IShape Members

        public uint Surface => (uint)Radius + 1;

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

        public bool OpposedDirection
        {
            get;
            set;
        }

        public CellRecord[] GetCells(CellRecord centerCell, MapRecord map)
        {
            if (OpposedDirection)
                Direction = Direction.GetOpposedDirection();

            var centerPoint = new MapPoint(centerCell.Id);
            var result = new List<CellRecord>();

            for (int i = MinRadius; i <= Radius; i++)
            {
                switch (Direction)
                {
                    case DirectionsEnum.DIRECTION_WEST:
                        AddCellIfValid(centerPoint.X - i, centerPoint.Y - i, map, result);
                        break;
                    case DirectionsEnum.DIRECTION_NORTH:
                        AddCellIfValid(centerPoint.X - i, centerPoint.Y + i, map, result);
                        break;
                    case DirectionsEnum.DIRECTION_EAST:
                        AddCellIfValid(centerPoint.X + i, centerPoint.Y + i, map, result);
                        break;
                    case DirectionsEnum.DIRECTION_SOUTH:
                        AddCellIfValid(centerPoint.X + i, centerPoint.Y - i, map, result);
                        break;
                    case DirectionsEnum.DIRECTION_NORTH_WEST:
                        AddCellIfValid(centerPoint.X - i, centerPoint.Y, map, result);
                        break;
                    case DirectionsEnum.DIRECTION_SOUTH_WEST:
                        AddCellIfValid(centerPoint.X, centerPoint.Y - i, map, result);
                        break;
                    case DirectionsEnum.DIRECTION_SOUTH_EAST:
                        AddCellIfValid(centerPoint.X + i, centerPoint.Y, map, result);
                        break;
                    case DirectionsEnum.DIRECTION_NORTH_EAST:
                        AddCellIfValid(centerPoint.X, centerPoint.Y + i, map, result);
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
        #endregion
    }
}
