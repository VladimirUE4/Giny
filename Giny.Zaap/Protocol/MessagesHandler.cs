using Giny.Core;
using Giny.Zaap.Network;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.Zaap.Protocol
{
    public class MessagesHandler
    {
        public static void Handle(ZaapClient client, ZaapMessage message)
        {
            switch (message)
            {
                case ConnectArgs connectArgs:
                    HandleConnectArgs(client, connectArgs);
                    break;
                case SettingsGet settingsGet:
                    HandleSettingsGet(client, settingsGet);
                    break;
                case UserInfoGet userInfoGet:
                    HandleUserInfoGet(client, userInfoGet);
                    break;
                case AuthGetGameToken authGetGameToken:
                    HandleAuthGetGameToken(client, authGetGameToken);
                    break;
                default:
                    Logger.Write("Unhandled message " + message.GetType().Name, Channels.Warning);
                    break;
            }
        }

        private static void HandleAuthGetGameToken(ZaapClient client, AuthGetGameToken message)
        {
            client.Send(new AuthGetGameTokenResult(message.CreateReply("success")));
        }

        private static void HandleUserInfoGet(ZaapClient client, UserInfoGet message)
        {
            client.Send(new UserInfosGetResult(message.CreateReply("success")));
        }

        private static void HandleSettingsGet(ZaapClient client, SettingsGet message)
        {
            string reply = null;

            switch (message.Key)
            {
                case "autoConnectType":
                    reply = "false";
                    break;
                case "language":
                    reply = "fr";
                    break;
                case "connectionPort":
                    reply = "443";
                    break;
                default:
                    break;
            }
            client.Send(new SettingsGetResult(message.CreateReply(reply)));
        }

        private static void HandleConnectArgs(ZaapClient client, ConnectArgs message)
        {
            client.Send(new ConnectResult(message.CreateReply("succces")));
        }
    }
}
