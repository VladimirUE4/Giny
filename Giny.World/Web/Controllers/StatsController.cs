using Giny.Core.Extensions;
using Giny.World.Network;
using Giny.World.Records.Characters;
using Giny.World.Records.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Giny.World.Web.Controllers
{
    [RoutePrefix("api/stats")]
    public class StatsController : ApiController
    {
        public Dictionary<string, string> Get()
        {
            return new Dictionary<string, string>()
            {
                { "connected", WorldServer.Instance.Clients.Count.ToString() },
                { "connectedDistinct",WorldServer.Instance.Clients.DistinctBy(x=>x.Ip).Count().ToString() },
                { "characters",CharacterRecord.GetCharacterRecords().Count().ToString() },
                { "items",CharacterItemRecord.GetCharactersItemsCount().ToString() },
            };
        }
    }
}
