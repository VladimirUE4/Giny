using Giny.Core.Network.Messages;
using Giny.Protocol.Enums;
using Giny.Protocol.Messages;
using Giny.World.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Giny.World.Handlers
{
    class ContextHandler
    {
        [MessageHandler]
        public static void HandleGameContextCreateRequestMessage(GameContextCreateRequestMessage message, WorldClient client)
        {
            client.Character.CreateContext(GameContextEnum.ROLE_PLAY);
            client.Character.Teleport(client.Character.Record.MapId, client.Character.Record.CellId);
            client.Character.RefreshStats();
        }
    }
}
