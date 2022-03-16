# Patch client

Exporter les scripts .as a partir du Dofus invoker:
```
com.ankamagames.dofus.datacenter
com.ankamagames.dofus.network.messages
com.ankamagames.dofus.network.enums
com.ankamagames.dofus.network.types
```

Patch ( déprécié) : https://www.youtube.com/watch?v=iYpeW4VqFw0

* Kernel.as  (optionel) 

if(buildType != -1 && buildType > -1)
{
    BuildInfos.VERSION.buildType = buildType;
}
* ServerControlFrame.as
Remplacer le paramètre content par rdMsg.content
``` l.loadBytes(rdMsg.content,lc);```
* Signature.as
```
Function verify() : Boolean
return true.
```

* AuthentificationManager.as
```actionscript
public function initAESKey() : void
{
    this._AESKey = this.generateRandomAESKey();
}
``` 
transformer en

```actionscript
public function initAESKey() : ByteArray
{
    this._AESKey = this.generateRandomAESKey();
     return this._AESKey;
}
```

# Protocol

* Fixer les éventuels nom de variables invalide en C# (@base)
* Apres la génération des D2O Classes utiliser Giny.D2O pour vérifiez les éventuels field manquant après la génération.
* Verifier l'id du Raw data message dans MessageReceiver.

CharacterCreationRequestMessage
colors = new int[5];

ObjectFeedMessage



 uint _mealLen = (uint)reader.ReadUShort();
            meal = new ObjectItemQuantity[_mealLen];  <------------
            for (uint _i2 = 0;_i2 < _mealLen;_i2++)


GameRolePlayGroupMonsterInformations

static infos a rajouter (non static)



