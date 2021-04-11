using Giny.Core.Network.Messages;
using Giny.Protocol.Custom.Enums;
using Giny.Protocol.Enums;
using Giny.Protocol.Messages;
using Giny.World.Managers.Guilds;
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
        public static void HandleGuildCreationRequest(GuildCreationValidMessage message, WorldClient client)
        {
            GuildCreationResultEnum result = GuildsManager.Instance.CreateGuild(client.Character, message.guildName, message.guildEmblem);
            client.Character.OnGuildCreated(result);
        }

        [MessageHandler]
        public static void HandleGuildGetInformationsMessage(GuildGetInformationsMessage message, WorldClient client)
        {
            switch ((GuildInformationsTypeEnum)message.infoType)
            {
                case GuildInformationsTypeEnum.INFO_GENERAL:
                    client.Send(client.Character.Guild.GetGuildInformationsGeneralMessage());
                    break;
                case GuildInformationsTypeEnum.INFO_MEMBERS:
                    client.Send(client.Character.Guild.GetGuildInformationsMembersMessage());
                    break;
                case GuildInformationsTypeEnum.INFO_BOOSTS:
                    break;
                case GuildInformationsTypeEnum.INFO_PADDOCKS:
                    break;
                case GuildInformationsTypeEnum.INFO_HOUSES:
                    break;
                case GuildInformationsTypeEnum.INFO_TAX_COLLECTOR_GUILD_ONLY:
                    break;
                case GuildInformationsTypeEnum.INFO_TAX_COLLECTOR_ALLIANCE:
                    break;
                case GuildInformationsTypeEnum.INFO_TAX_COLLECTOR_LEAVE:
                    break;
                default:
                    break;
            }
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
