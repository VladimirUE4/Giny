
using Giny.Protocol.Custom.Enums;
using Giny.World.Records.Maps;
using System;

namespace Giny.World.Managers.Maps.Shapes
{
	public class Single : IShape
	{
		public uint Surface
		{
			get
			{
				return 1u;
			}
		}
		public byte MinRadius
		{
			get
			{
				return 1;
			}
			set
			{
			}
		}
		public DirectionsEnum Direction
		{
			get
			{
				return DirectionsEnum.DIRECTION_NORTH;
			}
			set
			{
			}
		}
		public byte Radius
		{
			get
			{
				return 1;
			}
			set
			{
			}
		}
		public CellRecord[] GetCells(CellRecord centerCell,MapRecord map)
		{
            return new CellRecord[] { centerCell };
		}
	}
}
