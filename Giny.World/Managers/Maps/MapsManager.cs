using Giny.Core;
using Giny.Core.DesignPattern;
using Giny.Core.Extensions;
using Giny.Core.Time;
using Giny.Protocol.Custom.Enums;
using Giny.World.Handlers.Roleplay.Maps.Paths;
using Giny.World.Managers.Maps.Instances;
using Giny.World.Managers.Monsters;
using Giny.World.Records;
using Giny.World.Records.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Giny.World.Managers.Maps
{
    public class MapsManager : Singleton<MapsManager>
    {
        public const string MAP_KEY = "649ae451ca33ec53bbcbcc33becf15f4";

        [StartupInvoke("Map Instances", StartupInvokePriority.FourthPass)]
        public void CreateInstances()
        {
            UpdateLogger updateLogger = new UpdateLogger();

            var maps = MapRecord.GetMaps().ToArray();

            double n = 0;
            foreach (var record in maps)
            {
                record.Instance = new ClassicMapInstance(record);

                AsyncRandom random = new AsyncRandom();

                if (record.IsDungeonMap)
                {
                    MonstersManager.Instance.SpawnDungeonGroup(record);
                }
                else if (record.CanSpawnMonsters)
                {
                    MonstersManager.Instance.SpawnMonsterGroups(record, random);
                }

                double ratio = (n / maps.Length) * 100;

                updateLogger.Update((int)ratio);
                n++;
            }

        }

        public int GetOverrideNeighbourMapId(int mapId, MapScrollEnum scrollType)
        {
            var scrollAction = MapScrollActionRecord.GetMapScrollAction(mapId);

            if (scrollAction != null)
            {
                switch (scrollType)
                {
                    case MapScrollEnum.TOP:
                        return scrollAction.TopMapId;
                    case MapScrollEnum.LEFT:
                        return scrollAction.LeftMapId;
                    case MapScrollEnum.BOTTOM:
                        return scrollAction.BottomMapId;
                    case MapScrollEnum.RIGHT:
                        return scrollAction.RightMapId;
                }
                return -1;

            }
            else
            {
                return -1;
            }
        }

        public short GetNeighbourCellId(short cellId, MapScrollEnum scrollType)
        {
            switch (scrollType)
            {
                case MapScrollEnum.TOP:
                    return (short)(cellId + 532);
                case MapScrollEnum.LEFT:
                    return (short)(cellId + 27);
                case MapScrollEnum.BOTTOM:
                    return (short)(cellId - 532);
                case MapScrollEnum.RIGHT:
                    return (short)(cellId - 27);
                default:
                    return 0;
            }
        }

        public DirectionsEnum GetScrollDirection(MapScrollEnum scrollType)
        {
            switch (scrollType)
            {
                case MapScrollEnum.TOP:
                    return DirectionsEnum.DIRECTION_NORTH;

                case MapScrollEnum.LEFT:
                    return DirectionsEnum.DIRECTION_WEST;

                case MapScrollEnum.BOTTOM:
                    return DirectionsEnum.DIRECTION_SOUTH;

                case MapScrollEnum.RIGHT:
                    return DirectionsEnum.DIRECTION_EAST;

                default:
                    return DirectionsEnum.DIRECTION_EAST;
            }
        }

        private MapScrollEnum InvertScrollType(MapScrollEnum scroll)
        {
            switch (scroll)
            {
                case MapScrollEnum.TOP:
                    return MapScrollEnum.BOTTOM;
                case MapScrollEnum.BOTTOM:
                    return MapScrollEnum.TOP;
                case MapScrollEnum.LEFT:
                    return MapScrollEnum.RIGHT;
                case MapScrollEnum.RIGHT:
                    return MapScrollEnum.LEFT;
            }
            return MapScrollEnum.UNDEFINED;
        }
        public short FindNearMapBorder(MapRecord destinationMap, MapScrollEnum scrollType, short cellId)
        {
            var invertedScrollType = InvertScrollType(scrollType);
            var cells = destinationMap.GetMapChangeCells(invertedScrollType);

            if (cells.Length == 0)
            {
                return destinationMap.RandomWalkableCell().Id;
            }

            return cells.Aggregate((previous, next) => previous.Point.ManhattanDistanceTo(new MapPoint(cellId)) < next.Point.ManhattanDistanceTo(new MapPoint(cellId)) ? previous : next).Id;
        }
        /*
         * If cell is free & walkable, return cell
         * else return near free cell.
         * if no free cell is available return random walkable cell
         */
        public CellRecord SecureRoleplayCell(MapRecord map, CellRecord roleplayCell)
        {
            if (roleplayCell.Walkable && map.Instance.IsCellFree(roleplayCell.Id))
            {
                return roleplayCell;
            }
            else
            {
                CellRecord freeCell = GetNearFreeCell(map, roleplayCell);

                if (freeCell != null)
                {
                    return freeCell;
                }
                else
                {
                    return map.RandomWalkableCell();
                }
            }
        }
        public CellRecord GetNearFreeCell(MapRecord map, CellRecord roleplayCell)
        {
            MapPoint[] points = roleplayCell.Point.GetNearPoints().Where(x => map.IsCellWalkable(x.CellId) && map.Instance.IsCellFree(x.CellId) == true).ToArray();

            if (points.Length > 0)
            {
                return map.GetCell(points.Random());
            }
            else
            {
                return null;
            }
        }
    }
}
