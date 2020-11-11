using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Giny.Core.Network.Messages;
using Giny.Protocol.Messages;
using Giny.Protocol.Types;
using Giny.World.Network;

namespace Giny.World.Handlers.Social
{
    class FriendsHandler
    {
        [MessageHandler]
        public static void HandleFriendsGetListMessage(FriendsGetListMessage message,WorldClient client)
        {
            client.Send(new FriendsListMessage(new FriendInformations[0]));
        }
        [MessageHandler]
        public static void HandleIgnoredGetListMessage(IgnoredGetListMessage message,WorldClient client)
        {
            client.Send(new IgnoredListMessage(new IgnoredInformations[0]));
        }
    }
}
