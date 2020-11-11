
using Giny.Auth.Records;
using Giny.Core.DesignPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.Auth.Managers
{
    public class TicketsManager : Singleton<TicketsManager>
    {
        private Dictionary<string, AccountRecord> m_accounts = new Dictionary<string, AccountRecord>();

        public void Add(string ticket, AccountRecord account)
        {
            lock (m_accounts)
                this.m_accounts.Add(ticket, account);
        }
        public AccountRecord Get(string ticket)
        {
            lock (m_accounts)
            {
                if (m_accounts.ContainsKey(ticket))
                {
                    var result = m_accounts[ticket];
                    m_accounts.Remove(ticket);
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
