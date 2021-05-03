using Giny.Auth.Records;
using Giny.Auth;
using Giny.Core;
using Giny.Core.Network;
using Giny.Core.Network.Messages;
using Giny.IO.RawPatch;
using Giny.Protocol;
using Giny.Protocol.Custom.Enums;
using Giny.Protocol.Enums;
using Giny.Protocol.Messages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Giny.Core.Time;
using Giny.Core.IO;
using Giny.Core.Cryptography;
using Giny.Core.Extensions;

namespace Giny.Auth.Network
{
    public class AuthClient : Client
    {
        public DateTime InQueueUntil
        {
            get;
            set;
        }
        public bool QueueShowed
        {
            get;
            set;
        }
        public IdentificationMessage IdentificationMessage
        {
            get;
            set;
        }
        public AccountRecord Account
        {
            get;
            set;
        }
        public WorldCharacterRecord[] Characters
        {
            get;
            set;
        }
        public byte[] AesKey
        {
            get;
            set;
        }
        public bool HasRights
        {
            get
            {
                return Account.Role > ServerRoleEnum.Player;
            }
        }

        public string Ticket
        {
            get;
            set;
        }

        public AuthClient(Socket socket) : base(socket)
        {
            Send(new ProtocolRequired(ConfigFile.Instance.Version));
            Send(new HelloConnectMessage(string.Empty, new byte[0]));
            Send(new RawDataMessage(RawPatchManager.Instance.GetRawPatch("AuthPatch")));
        }


        public override void OnConnected()
        {
            throw new NotImplementedException();
        }

        public override void OnConnectionClosed()
        {
            OnDisconnected();
        }

        public override void OnFailToConnect(Exception ex)
        {
            throw new NotImplementedException();
        }

        public void GenerateTicket()
        {
            this.Ticket = new AsyncRandom().RandomString(32);
        }
        public byte[] EncryptTicket()
        {
            using (var writer = new BigEndianWriter())
            {
                writer.WriteByte((byte)Ticket.Length);
                writer.WriteUTFBytes(Ticket);
                return AES.Encrypt(writer.Data, AesKey);
            }
        }

        public override void OnMessageReceived(NetworkMessage message)
        {
            Logger.Write("Received " + message,Channels.Info);
            ProtocolMessageManager.HandleMessage(message, this);
        }

        public override void OnSended(IAsyncResult result)
        {
            Logger.Write("Send " + result.AsyncState);
        }

        public override void OnDisconnected()
        {
            Logger.Write("Client disconnected.");
            AuthServer.Instance.RemoveClient(this);
        }
        public void OnIdentificationFailed(IdentificationFailureReasonEnum reason)
        {
            Send(new IdentificationFailedMessage((byte)reason));
        }
        public void OnSelectedServerRefused(short serverId, ServerConnectionErrorEnum error, ServerStatusEnum serverStatus)
        {
            Send(new SelectedServerRefusedMessage(serverId, (byte)error, (byte)serverStatus));
        }
        public void OnNicknameRefusedMessage(NicknameRefusedReasonEnum reason)
        {
            Send(new NicknameRefusedMessage((byte)reason));
        }
        public void DisplayLoginQueue(short position, short total)
        {
            Send(new LoginQueueStatusMessage(position, total));
            QueueShowed = true;
        }
        public void CloseLoginQueue()
        {
            if (QueueShowed)
            {
                Send(new LoginQueueStatusMessage(0, 0));
            }
        }
        /// <summary>
        /// End this func.
        /// </summary>
        public void OnIdentificationSuccess(bool wasConnected)
        {
            Send(new IdentificationSuccessMessage(Account.Username, Account.Nickname,
                 Account.Id, 0, HasRights, HasRights, string.Empty, 0, 0, 0, wasConnected, 0));
        }
        public void SendServerList()
        {
            Send(new ServersListMessage(WorldServerRecord.GetGameServerInformations(this), 0, true));
        }
        public byte GetCharactersSlots(short serverId)
        {
            return (byte)Characters.Where(x => x.ServerId == serverId).Count();
        }

    }
}