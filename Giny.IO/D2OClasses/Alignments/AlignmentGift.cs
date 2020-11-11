using System;
using Giny.Core.IO.Interfaces;
using Giny.IO.D2O;
using Giny.IO.D2OTypes;
using System.Collections.Generic;

namespace Giny.IO.D2OClasses
{
    [D2OClass("AlignmentGift", "com.ankamagames.dofus.datacenter.alignments")]
    public class AlignmentGift : IDataObject , IIndexedData
    {
        public const string MODULE = "AlignmentGift";

        public int Id => (int)id;

        public int id;
        public uint nameId;
        public int effectId;
        public uint gfxId;

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
        [D2OIgnore]
        public int EffectId
        {
            get
            {
                return effectId;
            }
            set
            {
                effectId = value;
            }
        }
        [D2OIgnore]
        public uint GfxId
        {
            get
            {
                return gfxId;
            }
            set
            {
                gfxId = value;
            }
        }

    }
}






