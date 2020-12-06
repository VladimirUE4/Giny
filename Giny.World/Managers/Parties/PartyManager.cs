using Giny.Core.DesignPattern;
using Giny.Core.Pool;
using Giny.World.Managers.Entities.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Parties
{
    public class PartyManager : Singleton<PartyManager>
    {
        private UniqueIdProvider m_IdProvider = new UniqueIdProvider(0);

        private Dictionary<int, Party> m_parties = new Dictionary<int, Party>();

        public ClassicalParty CreateParty(Character leader)
        {
            var party = new ClassicalParty(m_IdProvider.Pop(), leader);
            m_parties.Add(party.Id, party);
            return party;
        }

        public void Remove(Party party)
        {
            m_parties.Remove(party.Id);
        }

        public Party GetParty(int partyId)
        {
            Party result = null;
            m_parties.TryGetValue(partyId, out result);
            return result;
        }
    }
}
