using Giny.Core.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.IO.SWL
{
    public class SwlFile
    {
        private string FileName
        {
            get;
            set;
        }
        private byte[] Swf
        {
            get;
            set;
        }

        public SwlFile(string path)
        {
            BigEndianReader reader = new BigEndianReader(File.ReadAllBytes(path));
            FileName = Path.GetFileNameWithoutExtension(path);
            Deserialize(reader);

        }
        private void Deserialize(BigEndianReader reader)
        {
            byte header = reader.ReadByte();
            if (header != 76)
            {
                throw new Exception("Malformated library file (wrong header).");
            }
            byte version = reader.ReadByte();
            uint frameRate = reader.ReadUInt();
            int classesCount = reader.ReadInt();

            var classes = new List<string>();
            for (int i = 0; i < classesCount; i++)
            {
                classes.Add(reader.ReadUTF());
            }

            Swf = reader.ReadBytes((int)reader.BytesAvailable);
        }

        public void ExtractSwf(string output)
        {
            File.WriteAllBytes(output, Swf);
        }
    }
}
