using System;
using Giny.Core.IO.Interfaces;
using Giny.IO.D2O;
using Giny.IO.D2OTypes;
using System.Collections.Generic;

namespace Giny.IO.D2OClasses
{
    [D2OClass("AbuseReasons", "com.ankamagames.dofus.datacenter.abuse")]
    public class AbuseReasons : IDataObject , IIndexedData
    {
        public const string MODULE = "AbuseReasons";

        public int Id => throw new NotImplementedException();

        public uint _abuseReasonId;
        public uint _mask;
        public int _reasonTextId;

        [D2OIgnore]
        public uint AbuseReasonId
        {
            get
            {
                return _abuseReasonId;
            }
            set
            {
                _abuseReasonId = value;
            }
        }
        [D2OIgnore]
        public uint Mask
        {
            get
            {
                return _mask;
            }
            set
            {
                _mask = value;
            }
        }
        [D2OIgnore]
        public int ReasonTextId
        {
            get
            {
                return _reasonTextId;
            }
            set
            {
                _reasonTextId = value;
            }
        }

    }
}






