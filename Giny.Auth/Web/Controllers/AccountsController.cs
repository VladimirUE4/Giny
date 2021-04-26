using Giny.Auth.Records;
using Giny.ORM;
using Giny.Protocol.IPC.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Results;

namespace Giny.Auth.Web.Controllers
{
    [EnableCors("http://localhost:3000", "*", "*")]
    [RoutePrefix("api/accounts")]
    public class AccountsController : ApiController
    {
        public IEnumerable<Account> Get()
        {
            return AccountRecord.GetAccountRecords().Select(x => x.ToAccount());
        }

        public Account Get(int id)
        {
            var accountRecord = AccountRecord.GetAccount(id);
            return accountRecord != null ? accountRecord.ToAccount() : null;
        }

        public Account Get(string username)
        {
            var accountRecord = AccountRecord.GetAccount(username);
            return accountRecord != null ? accountRecord.ToAccount() : null;
        }

        public StatusCodeResult Post([FromBody] Dictionary<string, string> value)
        {
            string username = value["username"];
            string password = value["password"];

            if (AccountRecord.GetAccount(username) != null)
            {
                return StatusCode(HttpStatusCode.Conflict);
            }

            AccountRecord accountRecord = new AccountRecord()
            {
                Username = username,
                Password = password,
            };

            accountRecord.AddInstantElement();

            return StatusCode(HttpStatusCode.OK);
        }
    }


}
