using Giny.Protocol.Custom.Enums;
using Giny.World.Records.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Maps.Shapes
{
    public class Lozenge : IShape
    {
        public Lozenge(byte minRadius, byte radius)
        {
            MinRadius = minRadius;
            Radius = radius;
        }

        public uint Surface
        {
            get
            {
                return ((uint)Radius + 1) * ((uint)Radius + 1) + Radius * (uint)Radius;
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
            var result = new List<CellRecord>();

            if (Radius == 0)
            {
                if (MinRadius == 0)
                    result.Add(centerCell);

                return result.ToArray();
            }

            int x = (int)(centerCell.Point.X - Radius);
            int y = 0;
            int i = 0;
            int j = 1;
            while (x <= centerCell.Point.X + Radius)
            {
                y = -i;

                while (y <= i)
                {
                    if (MinRadius == 0 || Math.Abs(centerCell.Point.X - x) + Math.Abs(y) >= MinRadius)
                        AddCellIfValid(x, y + centerCell.Point.Y, map, result);

                    y++;
                }

                if (i == Radius)
                {
                    j = -j;
                }

                i = i + j;
                x++;
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
