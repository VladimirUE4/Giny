using System;
using Giny.Core.IO.Interfaces;
using Giny.IO.D2O;
using Giny.IO.D2OTypes;
using System.Collections.Generic;

namespace Giny.IO.D2OClasses
{
    [D2OClass("Challenge", "com.ankamagames.dofus.datacenter.challenges")]
    public class Challenge : IDataObject , IIndexedData
    {
        public const string MODULE = "Challenge";

        public int Id => (int)id;

        public int id;
        public uint nameId;
        public uint descriptionId;
        public List<uint> incompatibleChallenges;
        public int categoryId;
        public uint iconId;
        public string activationCriterion;
        public string completionCriterion;

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
        [D2OIgnore]
        public List<uint> IncompatibleChallenges
        {
            get
            {
                return incompatibleChallenges;
            }
            set
            {
                incompatibleChallenges = value;
            }
        }
        [D2OIgnore]
        public int CategoryId
        {
            get
            {
                return categoryId;
            }
            set
            {
                categoryId = value;
            }
        }
        [D2OIgnore]
        public uint IconId
        {
            get
            {
                return iconId;
            }
            set
            {
                iconId = value;
            }
        }
        [D2OIgnore]
        public string ActivationCriterion
        {
            get
            {
                return activationCriterion;
            }
            set
            {
                activationCriterion = value;
            }
        }
        [D2OIgnore]
        public string CompletionCriterion
        {
            get
            {
                return completionCriterion;
            }
            set
            {
                completionCriterion = value;
            }
        }

    }
}






