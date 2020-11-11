using Giny.Core.Network.Messages;
using Giny.Protocol.IPC.Messages;
using Giny.World.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Handlers
{
    class IPCHandler
    {
        [MessageHandler]
        public static void HandleDisconnectRequest(DisconnectClientRequestMessage message, IPCClient client)
        {
            var otherClient = WorldServer.Instance.GetWorldClient(message.accountId);
            otherClient?.Disconnect();
            DisconnectClientResultMessage resultMessage = new DisconnectClientResultMessage(otherClient != null); // add client ip in packet to verify same account is conecting?
            client.SendIPCAnswer(resultMessage, message);
        }
    }
}
