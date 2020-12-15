using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class AllianceTaxCollectorDialogQuestionExtendedMessage : TaxCollectorDialogQuestionExtendedMessage  
    { 
        public new const ushort Id = 5803;
        public override ushort MessageId => Id;

        public BasicNamedAllianceInformations alliance;

        public AllianceTaxCollectorDialogQuestionExtendedMessage()
        {
        }
        public AllianceTaxCollectorDialogQuestionExtendedMessage(BasicNamedAllianceInformations alliance)
        {
            this.alliance = alliance;
        }
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            alliance.Serialize(writer);
        }
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            alliance = new BasicNamedAllianceInformations();
            alliance.Deserialize(reader);
        }


    }
}








