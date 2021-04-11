using Giny.Core;
using Giny.Core.DesignPattern;
using Giny.Core.Network;
using Giny.Core.Network.Messages;
using Giny.Protocol.Enums;
using Giny.Protocol.IPC.Messages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Network
{
    public class WorldServer : Singleton<WorldServer>
    {
        public SynchronizedCollection<WorldClient> Clients
        {
            get;
            private set;
        }
        public ServerStatusEnum Status
        {
            get;
            private set;
        } = ServerStatusEnum.STARTING;

        private Server Server
        {
            get;
            set;
        }
        public bool Started
        {
            get;
            private set;
        }


        public WorldServer()
        {
            this.Clients = new SynchronizedCollection<WorldClient>();
            this.Started = false;
        }

      
        public void Start(string ip, int port)
        {
            this.Server = new Server(ip, port);
            this.Server.OnSocketConnected += OnClientConnected;
            this.Server.OnServerFailedToStart += OnServerFailedToStart;
            this.Server.OnServerStarted += OnServerStarted;
            this.Server.Start();
            this.Started = true;
        }


        public void RemoveClient(WorldClient worldClient)
        {
            Clients.Remove(worldClient);
        }
        public void AddClient(WorldClient client)
        {
            Clients.Add(client);
        }
        public bool IsOnline(long characterId)
        {
            return GetConnectedClients().Any(x => x.Character.Id == characterId);
        }

        public void OnConnectedClients(Action<WorldClient> action)
        {
            foreach (var client in Clients.Where(x => x.InGame))
            {
                action(client);
            }
        }
        public IEnumerable<WorldClient> GetConnectedClients()
        {
            return Clients.Where(x => x.InGame);
        }
        public WorldClient GetConnectedClient(string name)
        {
            return Clients.FirstOrDefault(x => x.Character.Name == name);
        }
        public WorldClient GetConnectedClient(int accountId)
        {
            return Clients.FirstOrDefault(x => x.InGame && x.Account.Id == accountId);
        }
        private void OnClientConnected(Socket acceptSocket)
        {
            Logger.Write("(World) New client connected.");
            WorldClient client = new WorldClient(acceptSocket);
            Clients.Add(client);
        }
        private void OnServerFailedToStart(Exception ex)
        {
            Logger.Write("(World) Unable to start WorldServer : " + ex, Channels.Critical);
            SetServerStatus(ServerStatusEnum.OFFLINE);
        }
        public void SendServerStatusToAuth()
        {
            IPCManager.Instance.Send(new IPCServerStatusUpdateMessage(Status));
        }
        public void SetServerStatus(ServerStatusEnum status)
        {
            this.Status = status;

            if (IPCManager.Instance.Connected)
            {
                SendServerStatusToAuth();
            }
        }
        public void Send(NetworkMessage message)
        {
            foreach (var client in Clients)
            {
                client.Send(message);
            }
        }
        private void OnServerStarted()
        {
            Logger.Write("(World) World Server started", Channels.Log);
            SetServerStatus(ServerStatusEnum.ONLINE);
        }
        public WorldClient GetWorldClient(int accountId)
        {
            return Clients.FirstOrDefault(x => x.Account.Id == accountId);
        }
    }
}
