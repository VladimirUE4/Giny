using Giny.Core.DesignPattern;
using Giny.Protocol.Custom.Enums;
using Giny.Protocol.Enums;
using Giny.World.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Chat
{
    [WIP("Muted")]
    class ChatChannels
    {
        [ChatChannelHandler(ChatActivableChannelsEnum.CHANNEL_GUILD)]
        public static void HandleChatGuild(WorldClient client, string message)
        {
            if (client.Character.HasGuild)
            {
                client.Character.Guild.Send(ChatChannelsManager.Instance.GetChatServerMessage(ChatActivableChannelsEnum.CHANNEL_GUILD, message, client));
            }
        }
        [ChatChannelHandler(ChatActivableChannelsEnum.CHANNEL_PARTY)]
        public static void HandleChatParty(WorldClient client, string message)
        {
            if (client.Character.HasParty)
            {
                client.Character.Party.SendMembers(ChatChannelsManager.Instance.GetChatServerMessage(ChatActivableChannelsEnum.CHANNEL_PARTY, message, client));
            }
        }
        [ChatChannelHandler(ChatActivableChannelsEnum.CHANNEL_ADMIN)]
        public static void Admin(WorldClient client, string message)
        {
            if (client.Account.Role == ServerRoleEnum.Administrator)
            {
                WorldServer.Instance.Send(ChatChannelsManager.Instance.GetChatServerMessage(ChatActivableChannelsEnum.CHANNEL_ADMIN, message, client));
            }
        }
        [ChatChannelHandler(ChatActivableChannelsEnum.CHANNEL_GLOBAL)]
        public static void HandleChatGlobal(WorldClient client, string message)
        {
            if (!client.Character.Fighting)
            {
                if (client.Character.Map != null)
                {
                    if (client.Character.Map.Instance.Mute && client.Account.Role == ServerRoleEnum.Player)
                    {
                        client.Character.TextInformation(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 113);
                        return;
                    }
                    client.Character.SendMap(ChatChannelsManager.Instance.GetChatServerMessage(ChatActivableChannelsEnum.CHANNEL_GLOBAL, message, client));
                }
                else
                    client.Character.OnChatError(ChatErrorEnum.CHAT_ERROR_INVALID_MAP);
            }
            else
            {
                client.Character.Fighter.Fight.Send(ChatChannelsManager.Instance.GetChatServerMessage(ChatActivableChannelsEnum.CHANNEL_GLOBAL, message, client));
            }
        }
    }
}
