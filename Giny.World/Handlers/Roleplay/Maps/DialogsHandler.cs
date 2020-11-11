using Giny.Core.Network.Messages;
using Giny.Protocol.Enums;
using Giny.Protocol.Messages;
using Giny.World.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Handlers.Roleplay.Maps
{
    class DialogsHandler
    {
        [MessageHandler]
        public static void HandleLeaveDialogRequest(LeaveDialogRequestMessage message, WorldClient client)
        {
            client.Send(new LeaveDialogMessage((byte)DialogTypeEnum.DIALOG_DIALOG));
            client.Character.LeaveDialog();
        }
    }
}
