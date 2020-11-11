using Giny.Protocol.Custom.Enums;
using Giny.World.Records.Maps;

namespace Giny.World.Managers.Maps.Shapes
{
    public interface IShape
    {
        uint Surface
        {
            get;
        }
        byte MinRadius
        {
            get;
            set;
        }
        DirectionsEnum Direction
        {
            get;
            set;
        }
        byte Radius
        {
            get;
            set;
        }
        CellRecord[] GetCells(CellRecord centerCell, MapRecord map);
    }
}
