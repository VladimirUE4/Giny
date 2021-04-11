using Giny.Core.DesignPattern;
using Giny.Core.Pool;
using Giny.ORM;
using Giny.Protocol.Custom.Enums;
using Giny.Protocol.Types;
using Giny.World.Managers.Entities.Characters;
using Giny.World.Records.Guilds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Guilds
{
    public class GuildsManager : Singleton<GuildsManager>
    {
        public const int MaxMemberCount = 240;

        public const int MotdMaxLength = 255;

        private readonly Dictionary<long, Guild> Guilds = new Dictionary<long, Guild>();

        private UniqueIdProvider UniqueIdProvider
        {
            get;
            set;
        }

        [StartupInvoke("Guilds", StartupInvokePriority.SixthPath)]
        public void Initialize()
        {
            foreach (var guildRecord in GuildRecord.GetGuilds())
            {
                Guilds.Add(guildRecord.Id, new Guild(guildRecord));
            }

            int lastId = 0;

            if (Guilds.Count > 0)
            {
                lastId = (int)Guilds.Keys.OrderByDescending(x => x).FirstOrDefault();
            }

            UniqueIdProvider = new UniqueIdProvider(lastId);
        }
        public GuildCreationResultEnum CreateGuild(Character owner, string guildName, GuildEmblem guildEmblem)
        {
            if (owner.HasGuild)
            {
                return GuildCreationResultEnum.GUILD_CREATE_ERROR_ALREADY_IN_GUILD;
            }

            GuildEmblemRecord emblem = new GuildEmblemRecord(guildEmblem.symbolShape, guildEmblem.symbolColor, guildEmblem.backgroundShape,
                guildEmblem.backgroundColor);

            if (GuildRecord.Exists(guildName))
            {
                return GuildCreationResultEnum.GUILD_CREATE_ERROR_NAME_ALREADY_EXISTS;
            }

            if (GuildRecord.Exists(emblem))
            {
                return GuildCreationResultEnum.GUILD_CREATE_ERROR_EMBLEM_ALREADY_EXISTS;
            }

            GuildRecord record = new GuildRecord()
            {
                Emblem = emblem,
                Experience = 0,
                Id = UniqueIdProvider.Pop(),
                CreationDate = DateTime.Now,
                Members = new List<GuildMemberRecord>(),
                Motd = new GuildMotd(),
                Name = guildName
            };

            Guild instance = new Guild(record);

            if (instance.Join(owner, true))
            {
                Guilds.Add(record.Id, instance);
                record.AddElement();
                return GuildCreationResultEnum.GUILD_CREATE_OK;
            }
            else
            {
                return GuildCreationResultEnum.GUILD_CREATE_ERROR_REQUIREMENT_UNMET;
            }
        }

        public Guild GetGuild(long guildId)
        {
            return Guilds[guildId];
        }
    }
}
