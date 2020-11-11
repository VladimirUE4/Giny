using Giny.IO.DLM;
using Giny.IO.DLM.Elements;
using Giny.Core;
using Giny.IO.D2P;
using Giny.IO.DLM;
using Giny.IO.ELE;
using Giny.IO.ELE.Repertory;
using Giny.ORM;
using Giny.World.Records;
using Giny.World.Records.Maps;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.DatabaseSynchronizer
{
    class MapSynchronizer
    {
        public const string MAP_ENCRYPTION_KEY = "649ae451ca33ec53bbcbcc33becf15f4";

        public const string MAPS_PATH = "content/maps/";

        static Dictionary<int, EleGraphicalData> Elements;

        public static void Synchronize()
        {
            Logger.Write("Building Maps...");

            foreach (var file in Directory.GetFiles(Path.Combine(ConfigFile.Instance.ClientPath, MAPS_PATH)))
            {
                if (Path.GetExtension(file) == ".ele")
                {
                    var reader = new EleReader(file);
                    Elements = reader.ReadElements();
                }
                if (Path.GetExtension(file).ToLower() == ".d2p")
                {
                    Logger.Write(Path.GetFileName(file) + "...");
                    D2PFastMapFile d2p = new D2PFastMapFile(file);

                    foreach (var compressedMap in d2p.CompressedMaps)
                    {
                        DlmMap map = new DlmMap(compressedMap.Value);
                        if (map.Cells == null)
                            continue;

                        MapRecord record = new MapRecord();
                        record.Version = map.MapVersion;
                        record.Id = map.Id;
                        record.SubareaId = map.SubareaId;
                        record.RightMap = map.RightNeighbourId;
                        record.LeftMap = map.LeftNeighbourId;
                        record.TopMap = map.TopNeighbourId;
                        record.BottomMap = map.BottomNeighbourId;
                        record.Cells = new CellRecord[560];

                        for (short i = 0; i < record.Cells.Length; i++)
                        {


                            var cell = map.Cells[i];

                            record.Cells[i] = new CellRecord()
                            {
                                Blue = cell.Blue,
                                Red = cell.Red,
                                Id = i,
                                LosMov = cell.Losmov,
                                MapChangeData = cell.MapChangeData,
                            };
                        }

                        var layers = map.Layers;

                        List<InteractiveElementRecord> elements = new List<InteractiveElementRecord>();

                        foreach (var layer in layers)
                        {
                            foreach (DlmCell layerCell in layer.Cells)
                            {
                                foreach (var graphicalElement in layerCell.Elements.OfType<GraphicalElement>())
                                {
                                    if (graphicalElement.Identifier != 0)
                                    {
                                        var gfxElement = Elements[(int)graphicalElement.ElementId];

                                        if (gfxElement.Type != EleGraphicalElementTypes.ENTITY)
                                        {
                                            NormalGraphicalElementData normalElement = gfxElement as NormalGraphicalElementData;
                                            InteractiveElementRecord interactiveRecord = new InteractiveElementRecord();

                                            interactiveRecord.Identifier = (int)graphicalElement.Identifier;
                                            interactiveRecord.CellId = layerCell.CellId;

                                            if (normalElement != null)
                                                interactiveRecord.GfxId = normalElement.Gfx;
                                         
                                            interactiveRecord.BonesId = -1;
                                            elements.Add(interactiveRecord);

                                        }
                                        else
                                        {
                                            EntityGraphicalElementData entityElement = gfxElement as EntityGraphicalElementData;
                                            InteractiveElementRecord interactiveTable = new InteractiveElementRecord();
                                            interactiveTable.Identifier = (int)graphicalElement.Identifier;
                                            interactiveTable.CellId = layerCell.CellId;
                                            interactiveTable.BonesId = ushort.Parse(entityElement.EntityLook.Replace("{", "").Replace("}", ""));
                                            interactiveTable.GfxId = -1;
                                            elements.Add(interactiveTable);

                                        }

                                    }
                                }
                            }
                        }
                        record.Elements = elements.ToArray();
                        record.AddInstantElement();
                    }
                }
            }
            // parse .ele
        }
    }
}
