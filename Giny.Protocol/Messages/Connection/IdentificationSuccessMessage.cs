using System;
using System.Collections.Generic;
using Giny.Core.Network.Messages;
using Giny.Protocol.Types;
using Giny.Core.IO.Interfaces;
using Giny.Protocol;
using Giny.Protocol.Enums;

namespace Giny.Protocol.Messages
{ 
    public class IdentificationSuccessMessage : NetworkMessage  
    { 
        public new const ushort Id = 8394;
        public override ushort MessageId => Id;

        public string login;
        public string nickname;
        public int accountId;
        public byte communityId;
        public bool hasRights;
        public bool hasConsoleRight;
        public string secretQuestion;
        public double accountCreation;
        public double subscriptionElapsedDuration;
        public double subscriptionEndDate;
        public bool wasAlreadyConnected;
        public byte havenbagAvailableRoom;

        public IdentificationSuccessMessage()
        {
        }
        public IdentificationSuccessMessage(string login,string nickname,int accountId,byte communityId,bool hasRights,bool hasConsoleRight,string secretQuestion,double accountCreation,double subscriptionElapsedDuration,double subscriptionEndDate,bool wasAlreadyConnected,byte havenbagAvailableRoom)
        {
            this.login = login;
            this.nickname = nickname;
            this.accountId = accountId;
            this.communityId = communityId;
            this.hasRights = hasRights;
            this.hasConsoleRight = hasConsoleRight;
            this.secretQuestion = secretQuestion;
            this.accountCreation = accountCreation;
            this.subscriptionElapsedDuration = subscriptionElapsedDuration;
            this.subscriptionEndDate = subscriptionEndDate;
            this.wasAlreadyConnected = wasAlreadyConnected;
            this.havenbagAvailableRoom = havenbagAvailableRoom;
        }
        public override void Serialize(IDataWriter writer)
        {
            byte _box0 = 0;
            _box0 = BooleanByteWrapper.SetFlag(_box0,0,hasRights);
            _box0 = BooleanByteWrapper.SetFlag(_box0,1,hasConsoleRight);
            _box0 = BooleanByteWrapper.SetFlag(_box0,2,wasAlreadyConnected);
            writer.WriteByte((byte)_box0);
            writer.WriteUTF((string)login);
            writer.WriteUTF((string)nickname);
            if (accountId < 0)
            {
                throw new Exception("Forbidden value (" + accountId + ") on element accountId.");
            }

            writer.WriteInt((int)accountId);
            if (communityId < 0)
            {
                throw new Exception("Forbidden value (" + communityId + ") on element communityId.");
            }

            writer.WriteByte((byte)communityId);
            writer.WriteUTF((string)secretQuestion);
            if (accountCreation < 0 || accountCreation > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + accountCreation + ") on element accountCreation.");
            }

            writer.WriteDouble((double)accountCreation);
            if (subscriptionElapsedDuration < 0 || subscriptionElapsedDuration > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + subscriptionElapsedDuration + ") on element subscriptionElapsedDuration.");
            }

            writer.WriteDouble((double)subscriptionElapsedDuration);
            if (subscriptionEndDate < 0 || subscriptionEndDate > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + subscriptionEndDate + ") on element subscriptionEndDate.");
            }

            writer.WriteDouble((double)subscriptionEndDate);
            if (havenbagAvailableRoom < 0 || havenbagAvailableRoom > 255)
            {
                throw new Exception("Forbidden value (" + havenbagAvailableRoom + ") on element havenbagAvailableRoom.");
            }

            writer.WriteByte((byte)havenbagAvailableRoom);
        }
        public override void Deserialize(IDataReader reader)
        {
            byte _box0 = reader.ReadByte();
            hasRights = BooleanByteWrapper.GetFlag(_box0,0);
            hasConsoleRight = BooleanByteWrapper.GetFlag(_box0,1);
            wasAlreadyConnected = BooleanByteWrapper.GetFlag(_box0,2);
            login = (string)reader.ReadUTF();
            nickname = (string)reader.ReadUTF();
            accountId = (int)reader.ReadInt();
            if (accountId < 0)
            {
                throw new Exception("Forbidden value (" + accountId + ") on element of IdentificationSuccessMessage.accountId.");
            }

            communityId = (byte)reader.ReadByte();
            if (communityId < 0)
            {
                throw new Exception("Forbidden value (" + communityId + ") on element of IdentificationSuccessMessage.communityId.");
            }

            secretQuestion = (string)reader.ReadUTF();
            accountCreation = (double)reader.ReadDouble();
            if (accountCreation < 0 || accountCreation > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + accountCreation + ") on element of IdentificationSuccessMessage.accountCreation.");
            }

            subscriptionElapsedDuration = (double)reader.ReadDouble();
            if (subscriptionElapsedDuration < 0 || subscriptionElapsedDuration > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + subscriptionElapsedDuration + ") on element of IdentificationSuccessMessage.subscriptionElapsedDuration.");
            }

            subscriptionEndDate = (double)reader.ReadDouble();
            if (subscriptionEndDate < 0 || subscriptionEndDate > 9.00719925474099E+15)
            {
                throw new Exception("Forbidden value (" + subscriptionEndDate + ") on element of IdentificationSuccessMessage.subscriptionEndDate.");
            }

            havenbagAvailableRoom = (byte)reader.ReadSByte();
            if (havenbagAvailableRoom < 0 || havenbagAvailableRoom > 255)
            {
                throw new Exception("Forbidden value (" + havenbagAvailableRoom + ") on element of IdentificationSuccessMessage.havenbagAvailableRoom.");
            }

        }


    }
}








