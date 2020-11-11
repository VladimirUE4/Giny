using Giny.Core.Network.Messages;
using Giny.Protocol.Messages;
using Giny.World.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Handlers.Admin
{
    class AdminHandler
    {
        [MessageHandler]
        public static void HandleAdminQuietCommandMessage(AdminQuietCommandMessage message,WorldClient client)
        {
            try
            {
                var c = message.content.Split(null)[1];
                client.Character.Teleport(int.Parse(c));
            }
            catch
            {
                client.Character.Reply("No");
            }
        }
    }
}
