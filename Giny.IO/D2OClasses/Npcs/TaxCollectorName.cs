using System;
using Giny.Core.IO.Interfaces;
using Giny.IO.D2O;
using Giny.IO.D2OTypes;
using System.Collections.Generic;

namespace Giny.IO.D2OClasses
{
    [D2OClass("TaxCollectorName", "com.ankamagames.dofus.datacenter.npcs")]
    public class TaxCollectorName : IDataObject , IIndexedData
    {
        public const string MODULE = "TaxCollectorNames";

        public int Id => (int)id;

        public int id;
        public uint nameId;

        [D2OIgnore]
        public int Id_
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
            }
        }
        [D2OIgnore]
        public uint NameId
        {
            get
            {
                return nameId;
            }
            set
            {
                nameId = value;
            }
        }

    }
}






