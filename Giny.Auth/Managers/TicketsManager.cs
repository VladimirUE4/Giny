
using Giny.Auth.Records;
using Giny.Core.DesignPattern;
using Giny.Core.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.Auth.Managers
{
    public class TicketsManager : Singleton<TicketsManager>
    {
        private ConcurrentDictionary<string, AccountRecord> m_accounts = new ConcurrentDictionary<string, AccountRecord>();

        public void Add(string ticket, AccountRecord account)
        {
            lock (m_accounts)
                this.m_accounts.TryAdd(ticket, account);
        }
        public AccountRecord Get(string ticket)
        {
            lock (m_accounts)
            {
                if (m_accounts.ContainsKey(ticket))
                {
                    var result = m_accounts[ticket];
                    m_accounts.TryRemove(ticket);
                    return result;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
