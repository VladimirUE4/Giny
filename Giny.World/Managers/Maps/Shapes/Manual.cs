using Giny.Protocol.Custom.Enums;
using Giny.World.Records.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Maps.Shapes
{
    public class Manual : IShape
    {
        public uint Surface => throw new InvalidOperationException();

        private IEnumerable<short> Cells
        {
            get;
            set;
        }
        public Manual(IEnumerable<short> cells)
        {
            this.Cells = cells;
        }
        public byte MinRadius
        {
            get => throw new InvalidOperationException();
            set => throw new InvalidOperationException();
        }
        public DirectionsEnum Direction
        {
            get => throw new InvalidOperationException();
            set => throw new InvalidOperationException();
        }
        public byte Radius
        {
            get => throw new InvalidOperationException();
            set => throw new InvalidOperationException();
        }

        public CellRecord[] GetCells(CellRecord centerCell, MapRecord map)
        {
            return Cells.Select(x => map.GetCell(x)).ToArray();
        }
    }
}
