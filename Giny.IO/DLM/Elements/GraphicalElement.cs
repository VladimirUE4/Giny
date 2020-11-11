using Giny.Core.IO;

namespace Giny.IO.DLM.Elements
{
    public class GraphicalElement : BasicElement
    {
        public const uint CELL_HALF_WIDTH = 43;
        public const float CELL_HALF_HEIGHT = 21.5F;

        public int Hue1
        {
            get;
            set;
        }

        public int Hue2
        {
            get;
            set;
        }

        public int Hue3
        {
            get;
            set;
        }

        public int Shadow1
        {
            get;
            set;
        }

        public int Shadow2
        {
            get;
            set;
        }

        public int Shadow3
        {
            get;
            set;
        }

        public int OffsetX
        {
            get;
            set;
        }

        public int OffsetY
        {
            get;
            set;
        }

        public int PixelOffsetX
        {
            get;
            set;
        }

        public int PixelOffsetY
        {
            get;
            set;
        }

        public int Altitude
        {
            get;
            set;
        }

        public uint Identifier
        {
            get;
            set;
        }
        public GraphicalElement()
        {

        }
        public GraphicalElement(BigEndianReader _reader, sbyte mapVersion)
        {
            ElementId = _reader.ReadUInt();
            Hue1 = _reader.ReadSByte();
            Hue2 = _reader.ReadSByte();
            Hue3 = _reader.ReadSByte();
            Shadow1 = _reader.ReadSByte();
            Shadow2 = _reader.ReadSByte();
            Shadow3 = _reader.ReadSByte();

            if (mapVersion <= 4)
            {
                OffsetX = _reader.ReadSByte();
                OffsetY = _reader.ReadSByte();

                PixelOffsetX = (int)(OffsetX * CELL_HALF_WIDTH);
                PixelOffsetY = (int)(OffsetY * CELL_HALF_HEIGHT);
            }

            else
            {
                PixelOffsetX = _reader.ReadShort();
                PixelOffsetY = _reader.ReadShort();

                OffsetX = (int)(PixelOffsetX / CELL_HALF_WIDTH);
                OffsetY = (int)(PixelOffsetY / CELL_HALF_HEIGHT);
            }

            Altitude = _reader.ReadSByte();
            Identifier = _reader.ReadUInt();

        }

        public override void Serialize(BigEndianWriter writer, sbyte mapVersion)
        {
            writer.WriteUInt(ElementId);

            writer.WriteSByte((sbyte)Hue1);
            writer.WriteSByte((sbyte)Hue2);
            writer.WriteSByte((sbyte)Hue3);
            writer.WriteSByte((sbyte)Shadow1);
            writer.WriteSByte((sbyte)Shadow2);
            writer.WriteSByte((sbyte)Shadow3);

            if (mapVersion <= 4)
            {
                writer.WriteSByte((sbyte)OffsetX);
                writer.WriteSByte((sbyte)OffsetY);
            }
            else
            {
                writer.WriteShort((short)PixelOffsetX);
                writer.WriteShort((short)PixelOffsetY);
            }

            writer.WriteSByte((sbyte)Altitude);
            writer.WriteUInt((uint)Identifier);

        }
    }
}
