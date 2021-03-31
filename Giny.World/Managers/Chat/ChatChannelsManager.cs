using Giny.Core.DesignPattern;
using Giny.Protocol.Enums;
using Giny.World.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Giny.Protocol.Messages;
using Giny.Protocol.Types;
using Giny.Core.Extensions;
using Giny.Core.Network.Messages;

namespace Giny.World.Managers.Chat
{
    public class ChatChannelsManager : Singleton<ChatChannelsManager>
    {
        public delegate void ChatHandlerDelegate(WorldClient client, string message);
        private readonly Dictionary<ChatActivableChannelsEnum, ChatHandlerDelegate> ChatHandlers = new Dictionary<ChatActivableChannelsEnum, ChatHandlerDelegate>();

        [StartupInvoke("Chat Channels", StartupInvokePriority.SixthPath)]
        public void Initialize()
        {
            foreach (var method in typeof(ChatChannels).GetMethods())
            {
                var attribute = method.GetCustomAttribute<ChatChannelHandlerAttribute>();

                if (attribute != null)
                {
                    ChatHandlers.Add(attribute.Channel, (ChatHandlerDelegate)Delegate.CreateDelegate(typeof(ChatHandlerDelegate), method));
                }
            }

        }
        public void Handle(WorldClient client, string message, ChatActivableChannelsEnum channel)
        {
            if (message.StartsWith(ChatCommandsManager.COMMANDS_PREFIX))
            {
                ChatCommandsManager.Instance.Handle(message.Remove(0, 1), client, client);
                return;
            }

            var handler = ChatHandlers.FirstOrDefault(x => x.Key == channel);
            if (handler.Value != null)
                handler.Value(client, message);
            else
                client.Character.Reply("Ce chat n'est pas géré");
        }


        /*   public ChatServerWithObjectMessage GetChatServerWithObjectMessage(ChatActivableChannelsEnum channel, ObjectItem[] items, string message, WorldClient client)
  {
      return new ChatServerWithObjectMessage((byte)channel, message, (int)DateExtensions.DateTimeToUnixTimestamp(DateTime.Now), string.Empty, client.Character.Id,
          client.Character.Name, client.Account.id, items);
  } */
        public ChatServerMessage GetChatServerMessage(ChatActivableChannelsEnum channel, string message, WorldClient client)
        {
            return new ChatServerMessage()
            {
                channel = (byte)channel,
                content = message,
                fingerprint = string.Empty,
                prefix = string.Empty,
                senderAccountId = client.Account.Id,
                senderId = client.Character.Id,
                senderName = client.Character.Name,
                timestamp = DateTime.Now.GetUnixTimeStamp(),
            };
        }

        public ChatServerCopyMessage GetChatServerCopyMessage(ChatActivableChannelsEnum channel, string message, WorldClient client, WorldClient target)
        {
            return new ChatServerCopyMessage()
            {
                channel = (byte)channel,
                content = message,
                fingerprint = string.Empty,
                timestamp = DateTime.Now.GetUnixTimeStamp(),
                receiverId = target.Character.Id,
                receiverName = target.Character.Name,
            };
        }
    }
}
