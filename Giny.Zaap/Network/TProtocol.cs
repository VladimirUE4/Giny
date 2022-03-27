using Giny.Core.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.Zaap.Network
{
    public class TProtocol
    {
        private int VERSION_MASK;

        private int VERSION_1;

        private bool StrictRead
        {
            get;
            set;
        }
        private bool StrictWrite
        {
            get;
            set;
        }

        public TProtocol(bool strictRead = false, bool strictWrite = true)
        {
            unchecked
            {
                VERSION_MASK = (int)4294901760;
                VERSION_1 = (int)2147549184;
            }

            StrictRead = strictRead;
            StrictWrite = strictWrite;
        }

        public TMessage ReadMessageBegin(BigEndianReader reader)
        {
            TMessage result = new TMessage();

            var val1 = reader.ReadInt();

            if ((val1 & VERSION_MASK) != VERSION_1)
            {
                throw new Exception("Bad version in read message begin.");
            }

            result.Type = val1 & 255;
            result.Name = reader.ReadUTF7BitLength();
            result.SequenceId = reader.ReadInt();
            return result;
        }
    }
}
