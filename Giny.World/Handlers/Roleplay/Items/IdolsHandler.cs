﻿using Giny.Core.DesignPattern;
using Giny.Core.Network.Messages;
using Giny.Protocol.Messages;
using Giny.World.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Handlers.Roleplay.Items
{
    class IdolsHandler
    {
        [WIP]
        [MessageHandler]
        public static void HandleIdolPartyRegisterRequest(IdolPartyRegisterRequestMessage message,WorldClient client)
        {
            client.Send(new IdolListMessage(new short[0], new short[0], new Protocol.Types.PartyIdol[0]));
            
        }
    }
}
