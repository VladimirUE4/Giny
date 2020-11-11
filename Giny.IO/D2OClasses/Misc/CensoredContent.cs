using System;
using Giny.Core.IO.Interfaces;
using Giny.IO.D2O;
using Giny.IO.D2OTypes;
using System.Collections.Generic;

namespace Giny.IO.D2OClasses
{
    [D2OClass("CensoredContent", "com.ankamagames.dofus.datacenter.misc")]
    public class CensoredContent : IDataObject , IIndexedData
    {
        public const string MODULE = "CensoredContents";

        public int Id => throw new NotImplementedException();

        public string lang;

        public int type;

        public int oldValue;

        public int newValue;

    }
}






