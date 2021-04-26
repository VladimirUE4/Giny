﻿using Giny.Core;
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

        public int MaximumClients
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

            if (Clients.Count > MaximumClients)
            {
                MaximumClients = Clients.Count;
            }
        }

        public bool IsOnline(long characterId)
        {
            return GetOnlineClients().Any(x => x.Character.Id == characterId);
        }

        public void Foreach(Action<WorldClient> action)
        {
            foreach (var client in GetOnlineClients())
            {
                action(client);
            }
        }
        public IEnumerable<WorldClient> GetOnlineClients()
        {
            return Clients.Where(x => x.InGame);
        }
        /// <summary>
        /// Returns a client who is not necessarily connected in game 
        /// </summary>
        public WorldClient GetClient(Func<WorldClient, bool> predicate)
        {
            return Clients.FirstOrDefault(predicate);
        }
        /// <summary>
        /// Returns a client who is connected in game 
        /// </summary>
        public WorldClient GetOnlineClient(Func<WorldClient, bool> predicate)
        {
            return Clients.Where(x => x.InGame).FirstOrDefault(predicate);
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
    }
}
