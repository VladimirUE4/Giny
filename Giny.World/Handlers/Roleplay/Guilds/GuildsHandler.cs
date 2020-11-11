using Giny.Core.Network.Messages;
using Giny.Protocol.Messages;
using Giny.World.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Handlers.Roleplay.Guilds
{
    class GuildsHandler
    {
        [MessageHandler]
        public static void HandleGuildGetInformationsMessage(GuildGetInformationsMessage message, WorldClient client)
        {
            client.Send(new GuildInformationsGeneralMessage()
            {
                abandonnedPaddock = false,
                creationDate = 0,
                experience = 0,
                expLevelFloor = 0,
                expNextLevelFloor = 0,
                level = 1,
                nbConnectedMembers = 0,
                nbTotalMembers = 1,
            });
        }
    }
}
