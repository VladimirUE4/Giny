using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Giny.Core.Network.Messages;
using Giny.Protocol.Enums;
using Giny.Protocol.Messages;
using Giny.World.Managers.Chat;
using Giny.World.Network;

namespace Giny.World.Handlers.Roleplay.Social
{
    class ChatHandler
    {
        [MessageHandler]
        public static void HandleChatSmileyRequestMessage(ChatSmileyRequestMessage message, WorldClient client)
        {
            client.Character.DisplaySmiley(message.smileyId);
        }
        [MessageHandler]
        public static void HandleChatMultiClient(ChatClientMultiMessage message, WorldClient client)
        {
            ChatChannelsManager.Instance.Handle(client, message.content, (ChatActivableChannelsEnum)message.channel);
        }

        [MessageHandler]
        public static void ChatClientPrivate(ChatClientPrivateMessage message, WorldClient client)
        {
            if (message.receiver == client.Character.Name)
            {
                client.Character.OnChatError(ChatErrorEnum.CHAT_ERROR_INTERIOR_MONOLOGUE);
                return;
            }

            WorldClient target = WorldServer.Instance.GetOnlineClient(x => x.Character.Name == message.receiver);

            if (target != null)
            {
                target.Send(ChatChannelsManager.Instance.GetChatServerMessage(ChatActivableChannelsEnum.PSEUDO_CHANNEL_PRIVATE, message.content, client));
                client.Send(ChatChannelsManager.Instance.GetChatServerCopyMessage(ChatActivableChannelsEnum.PSEUDO_CHANNEL_PRIVATE, message.content, client, target));
            }
            else
            {
                client.Character.OnChatError(ChatErrorEnum.CHAT_ERROR_RECEIVER_NOT_FOUND);
            }

        }
    }
}
