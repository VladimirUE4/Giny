using System;
using Giny.Core.IO.Interfaces;
using Giny.IO.D2O;
using Giny.IO.D2OTypes;
using System.Collections.Generic;

namespace Giny.IO.D2OClasses
{
    [D2OClass("AlignmentEffect", "com.ankamagames.dofus.datacenter.alignments")]
    public class AlignmentEffect : IDataObject , IIndexedData
    {
        public const string MODULE = "AlignmentEffect";

        public int Id => (int)id;

        public int id;
        public uint characteristicId;
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
        public uint CharacteristicId
        {
            get
            {
                return characteristicId;
            }
            set
            {
                characteristicId = value;
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






