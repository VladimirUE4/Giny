using Giny.Core;
using Giny.Core.DesignPattern;
using Giny.Core.Extensions;
using Giny.ORM;
using Giny.ORM.Cyclic;
using Giny.Protocol.Enums;
using Giny.Protocol.Messages;
using Giny.World.Network;
using Giny.World.Records;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Giny.World.Managers
{
    public class WorldSaveManager : Singleton<WorldSaveManager>
    {
        public const double SAVE_INTERVAL_MINUTES = 30;

        private Task m_queueRefresherTask;

        [StartupInvoke("Save Task", StartupInvokePriority.Last)]
        public void CreateNextTask()
        {
            m_queueRefresherTask = Task.Factory.StartNewDelayed((int)((SAVE_INTERVAL_MINUTES * 60) * 1000), PerformSave);
        }


        public void PerformSave()
        {
            if (WorldServer.Instance.Status == ServerStatusEnum.ONLINE)
            {
                WorldServer.Instance.SetServerStatus(ServerStatusEnum.SAVING);

                WorldServer.Instance.OnConnectedClients(x => x.Character.TextInformation(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 164));

                foreach (var client in WorldServer.Instance.GetConnectedClients())
                {
                    client.Character.Record.UpdateElement();
                }

                CyclicSaveTask.Instance.Save();

                WorldServer.Instance.OnConnectedClients(x => x.Character.TextInformation(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 165));

                WorldServer.Instance.SetServerStatus(ServerStatusEnum.ONLINE);
                CreateNextTask();
            }
            else
            {
                Logger.Write("Unable to save world server, server is busy...", Channels.Warning);
                CreateNextTask();
            }
        }

    }
}
