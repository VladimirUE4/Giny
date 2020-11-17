using Giny.Core;
using Giny.Core.DesignPattern;
using Giny.Core.Extensions;
using Giny.Core.IO;
using Giny.IO.MA3;
using Giny.ORM;
using Giny.World.Records.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Misc
{
    public class MA3Manager : Singleton<MA3Manager>
    {
        public const string ITEMS_URL = "http://www.dofus.tools/myAvatar3/assets/data/Items.ma3";

        public const string MOUNTS_URL = "http://www.dofus.tools/myAvatar3/assets/data/Mounts.ma3";

        [StartupInvoke("MA3 Builder", StartupInvokePriority.Last)]
        public void Initialize()
        {
            Logger.Write("Downloading Items.ma3 from " + ITEMS_URL, MessageState.INFO2);

            try
            {
                ProcessMA3();
            }
            catch (Exception ex)
            {
                Logger.Write("Unable to download Items.ma3 from " + ITEMS_URL, MessageState.WARNING);
            }

        }

        private static void ProcessMA3()
        {
            MA3ItemFile itemFile = new MA3ItemFile(Web.DownloadData(ITEMS_URL));

            foreach (var item in itemFile.Items)
            {
                if (ItemRecord.ItemExists(item.Id))
                {
                    var itemRecord = ItemRecord.GetItem(item.Id);


                    if (item.Skin != 0 && itemRecord.AppearenceId != item.Skin)
                    {
                        WeaponRecord weapon = WeaponRecord.GetWeapon(item.Id);

                        if (weapon != null)
                        {
                            weapon.AppearenceId = (short)item.Skin;
                            weapon.UpdateInstantElement();
                        }
                        else
                        {
                            itemRecord.AppearenceId = (short)item.Skin;
                            itemRecord.UpdateInstantElement();
                        }
                        Logger.Write(string.Format("({0}) apparenceId updated.", itemRecord.Name), MessageState.INFO2);
                    }
                    else if (item.Look != string.Empty && item.Look != itemRecord.Look)
                    {
                        itemRecord.Look = item.Look;
                        itemRecord.UpdateInstantElement();
                        Logger.Write(string.Format("({0}) look updated.", itemRecord.Name), MessageState.INFO2);
                    }
                }
            }
        }
    }
}
