using System;
using Giny.Core.IO.Interfaces;
using Giny.IO.D2O;
using Giny.IO.D2OTypes;
using System.Collections.Generic;

namespace Giny.IO.D2OClasses
{
    [D2OClass("Url", "com.ankamagames.dofus.datacenter.misc")]
    public class Url : IDataObject, IIndexedData
    {
        public const string MODULE = "Url";

        public int Id => (int)id;

        public int id;
        public int browserId;
        public string url;
        public string param;
        public string method;

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
        public int BrowserId
        {
            get
            {
                return browserId;
            }
            set
            {
                browserId = value;
            }
        }
        [D2OIgnore]
        public string Url_
        {
            get
            {
                return url;
            }
            set
            {
                url = value;
            }
        }
        [D2OIgnore]
        public string Param
        {
            get
            {
                return param;
            }
            set
            {
                param = value;
            }
        }
        [D2OIgnore]
        public string Method
        {
            get
            {
                return method;
            }
            set
            {
                method = value;
            }
        }

    }
}






