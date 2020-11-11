using Giny.Auth.Network;
using Giny.ORM.Attributes;
using Giny.ORM.Interfaces;
using Giny.ORM.IO;
using Giny.Protocol.Custom.Enums;
using Giny.Protocol.IPC.Types;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.Auth.Records
{
    [Table("accounts", -1, false)]
    public class AccountRecord : ITable
    {
        long ITable.Id => Id;

        [Primary]
        public int Id
        {
            get;
            set;
        }

        public string Username
        {
            get;
            set;
        }

        public string Password
        {
            get;
            set;
        }
        [Update]
        public short LastSelectedServerId
        {
            get;
            set;
        }
        [Update]
        public List<string> IPs
        {
            get;
            set;
        }
        public byte CharacterSlots
        {
            get;
            set;
        }
        [Update]
        public bool Banned
        {
            get;
            set;
        }
        public ServerRoleEnum Role
        {
            get;
            set;
        }
        [Update]
        public string Nickname
        {
            get;
            set;
        }


        public Account GetAccount()
        {
            return new Account(Id, Username, Nickname, Banned, CharacterSlots, Role, LastSelectedServerId);
        }


        public static AccountRecord GetAccount(string username)
        {
            return DatabaseReader.ReadFirst<AccountRecord>("Username", username);
        }

        public static bool NicknameExist(string nickname)
        {
            AccountRecord account = DatabaseReader.ReadFirst<AccountRecord>("Nickname", nickname);
            return account != null;
        }

    }
}
