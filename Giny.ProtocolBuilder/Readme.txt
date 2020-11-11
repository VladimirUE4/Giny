Imports from DofusInvoker.swf sources:

com.ankamagames.dofus.datacenter
com.ankamagames.dofus.network.messages
com.ankamagames.dofus.network.enums
com.ankamagames.dofus.network.types


Patch : https://www.youtube.com/watch?v=iYpeW4VqFw0

------------------Kernel.as------------------
if(buildType != -1 && buildType > -1)
{
    BuildInfos.VERSION.buildType = buildType;
}
------------------ServerControlFrame.as------------------
l94 change parameter l.loadBytes(rdMsg.content,lc);
getslot 2
getproperty Qname(PackageNamespace(""),"content")
------------------Signature.as---------------------------
Function verify() : Boolean
return true.

AuthentificationManager.as

      public function initAESKey() : void
      {
         this._AESKey = this.generateRandomAESKey();
      }

	  a transformer en

      public function initAESKey() : ByteArray
      {
         this._AESKey = this.generateRandomAESKey();
         return this._AESKey;
      }

      Fucking raw data message Id is now 1528



CharacterCreationRequestMessage
colors = new int[5];

ObjectFeedMessage



 uint _mealLen = (uint)reader.ReadUShort();
            meal = new ObjectItemQuantity[_mealLen];  <------------
            for (uint _i2 = 0;_i2 < _mealLen;_i2++)


GameRolePlayGroupMonsterInformations

static infos -> non static