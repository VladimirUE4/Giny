﻿using Giny.Core.Collections;
using Giny.Core.DesignPattern;
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
using System.Web.Http.Cors;

namespace Giny.World.Web.Controllers
{
    [EnableCors("http://localhost:3000", "*", "*")]
    [RoutePrefix("api/stats")]
    public class StatsController : ApiController
    {

        public Dictionary<string, string> Get()
        {
            return new Dictionary<string, string>()
            {
                { "connected", WorldServer.Instance.Clients.Count.ToString() },
                { "connectedDistinct",WorldServer.Instance.Clients.DistinctBy(x=>x.Ip).Count().ToString() },
                { "connectedMax",WorldServer.Instance.MaximumClients.ToString() },
                { "characters",CharacterRecord.GetCharacterRecords().Count().ToString() },
                { "items",CharacterItemRecord.GetCharactersItemsCount().ToString() },
            };
        }
    }
}
