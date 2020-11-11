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
    class ChatChannels
    {
        [ChatChannelHandler(ChatActivableChannelsEnum.CHANNEL_GLOBAL)]
        public static void HandleChatGlobal(WorldClient client, string message)
        {
            /* if (client.Character.Record.Muted)
             {
                 client.Character.TextInformation(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 107);
                 return;
             } */
            if (!client.Character.Fighting)
            {
                if (client.Character.Map != null)
                {
                    if (client.Character.Map.Instance.Mute && client.Account.Role == ServerRoleEnum.PLAYER)
                    {
                        client.Character.TextInformation(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 113);
                        return;
                    }
                    client.Character.SendMap(ChatChannelsManager.Instance.GetChatServerMessage(ChatActivableChannelsEnum.CHANNEL_GLOBAL, message, client));
                }
                else
                    client.Character.OnChatError(ChatErrorEnum.CHAT_ERROR_INVALID_MAP);
            }
            //   else
            //    {
            //  client.Character.Fighter.Fight.Send(GetChatServerMessage(ChatActivableChannelsEnum.CHANNEL_GLOBAL, message, client));
            //  client.Character.Fighter.Fight.CheckFightEnd();

            //  }
        }
    }
}
