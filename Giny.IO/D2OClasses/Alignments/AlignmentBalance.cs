using System;
using Giny.Core.IO.Interfaces;
using Giny.IO.D2O;
using Giny.IO.D2OTypes;
using System.Collections.Generic;

namespace Giny.IO.D2OClasses
{
    [D2OClass("AlignmentBalance", "com.ankamagames.dofus.datacenter.alignments")]
    public class AlignmentBalance : IDataObject , IIndexedData
    {
        public const string MODULE = "AlignmentBalance";

        public int Id => (int)id;

        public int id;
        public int startValue;
        public int endValue;
        public uint nameId;
        public uint descriptionId;

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
        public int StartValue
        {
            get
            {
                return startValue;
            }
            set
            {
                startValue = value;
            }
        }
        [D2OIgnore]
        public int EndValue
        {
            get
            {
                return endValue;
            }
            set
            {
                endValue = value;
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
        [D2OIgnore]
        public uint DescriptionId
        {
            get
            {
                return descriptionId;
            }
            set
            {
                descriptionId = value;
            }
        }

    }
}






