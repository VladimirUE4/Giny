using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Giny.AS3;
using Giny.AS3.Converter;
using Giny.AS3.Enums;
using Giny.AS3.Expressions;
using Giny.Core.IO;

namespace Giny.ProtocolBuilder.Converters
{
    public abstract class SerializableConverter : DofusConverter
    {
        public override string GetImplements()
        {
            return string.Empty;
        }
        protected AS3Method SerializeMethod
        {
            get;
            set;
        }
        protected AS3Method DeserializeMethod
        {
            get;
            set;
        }
        private AS3Method[] GetCtors()
        {
            List<AS3Method> results = new List<AS3Method>();

            AS3Method ctor = File.CreateConstructor(AS3AccessorsEnum.@public);

            if (ctor.Parameters.Length > 0)
            {
                AS3Method emptyCtor = File.CreateEmptyConstructor();
                results.Add(emptyCtor);
            }
            results.Add(ctor);

            return results.ToArray();
        }
        private AS3Method GetSerializeMethod()
        {
            AS3Method serializeMethod = File.GetMethod("serializeAs_" + GetClassName());
            serializeMethod.Rename("Serialize");
            return serializeMethod;
        }


        private AS3Method GetDeserializeMethod()
        {
            AS3Method deserializeMethod = AS3Method.RencapsulateMethod(File, "deserializeAs_" + GetClassName(), "Deserialize");
            return deserializeMethod;
        }
        public override void Initialize()
        {
            this.SerializeMethod = GetMethodToWrite("Serialize");
            this.DeserializeMethod = GetMethodToWrite("Deserialize");

            SerializeMethod.RenameVariable("NaN", "double.NaN");
            SerializeMethod.RenameVariable("super", "base");
            SerializeMethod.RenameVariable("output", "writer");
            SerializeMethod.RenameVariable("length", "Length");
            SerializeMethod.RenameType("ICustomDataOutput", "IDataWriter");
            SerializeMethod.RenameMethodCall("writeUTF", "WriteUTF");
            SerializeMethod.RenameMethodCall("writeVarInt", "WriteVarInt");
            SerializeMethod.RenameMethodCall("writeByte", "WriteByte");
            SerializeMethod.RenameMethodCall("writeVarLong", "WriteVarLong");
            SerializeMethod.RenameMethodCall("writeShort", "WriteShort");
            SerializeMethod.RenameMethodCall("writeVarShort", "WriteVarShort");
            SerializeMethod.RenameMethodCall("writeBoolean", "WriteBoolean");
            SerializeMethod.RenameMethodCall("writeDouble", "WriteDouble");
            SerializeMethod.RenameMethodCall("writeInt", "WriteInt");
            SerializeMethod.RenameMethodCall("writeFloat", "WriteFloat");
            SerializeMethod.RenameMethodCall("writeUnsignedInt", "WriteUInt");
            SerializeMethod.RenameMethodCall("setFlag", "SetFlag");
            SerializeMethod.RenameMethodCall("serialize", "Serialize");

            SerializeMethod.ReplaceUnchangedExpression("getTypeId()", "TypeId");

            SerializeMethod.SetModifiers(AS3ModifiersEnum.@override);
            DofusHelper.IOWriteCastRecursively(SerializeMethod.Expressions);
            DofusHelper.DeductFieldTypes(File, SerializeMethod.Expressions); // order is importants!
            DofusHelper.RenameDofusTypesSerializeMethodsRecursively(SerializeMethod.Expressions);
            DofusHelper.RenameSerializeAs_(SerializeMethod.Expressions);
            DofusHelper.ChangeTypeIdToProperty(this.SerializeMethod.Expressions);

            DeserializeMethod.RenameMethodCall("readByte", "ReadByte");

            DeserializeMethod.RenameVariable("NaN", "double.NaN");
            DeserializeMethod.RenameVariable("super", "base");
            DeserializeMethod.RenameVariable("input", "reader");
            DeserializeMethod.RenameVariable("length", "Length");
            DeserializeMethod.RenameType("ICustomDataInput", "IDataReader");
            DeserializeMethod.RenameMethodCall("getInstance", "GetInstance");
            DeserializeMethod.RenameMethodCall("readUTF", "ReadUTF");
            DeserializeMethod.RenameMethodCall("readVarInt", "ReadVarInt");
            DeserializeMethod.RenameMethodCall("readVarLong", "ReadVarLong");
            DeserializeMethod.RenameMethodCall("readShort", "ReadShort");
            DeserializeMethod.RenameMethodCall("readVarShort", "ReadVarShort");
            DeserializeMethod.RenameMethodCall("readBoolean", "ReadBoolean");
            DeserializeMethod.RenameMethodCall("readDouble", "ReadDouble");
            DeserializeMethod.RenameMethodCall("readInt", "ReadInt");
            DeserializeMethod.RenameMethodCall("readUnsignedInt", "ReadUInt");
            DeserializeMethod.RenameMethodCall("readUnsignedShort", "ReadUShort");
            DeserializeMethod.RenameMethodCall("readVarUhShort", "ReadVarUhShort");
            DeserializeMethod.RenameMethodCall("readVarUhInt", "ReadVarUhInt");
            DeserializeMethod.RenameMethodCall("readVarUhLong", "ReadVarUhLong");
            DeserializeMethod.RenameMethodCall("readUnsignedByte", "ReadSByte");
            DeserializeMethod.RenameMethodCall("readFloat", "ReadFloat");
            DeserializeMethod.RenameMethodCall("getFlag", "GetFlag");
            DeserializeMethod.RenameMethodCall("deserialize", "Deserialize");

            DeserializeMethod.SetModifiers(AS3ModifiersEnum.@override);
            DofusHelper.IOReadCastRecursively(this, File, DeserializeMethod, DeserializeMethod.Expressions);
            DofusHelper.InstantiateArrays(this, File, DeserializeMethod);
            DofusHelper.TransformVectorPushIntoCSharpArrayIndexer(File, this, DeserializeMethod);
            DofusHelper.CreateGenericTypeForProtocolInstance(DeserializeMethod.Expressions);
            DofusHelper.CastEnumsComparaisons(DeserializeMethod.Expressions);
        }
        protected override AS3Method[] GetMethodsToWrite()
        {
            return
                GetCtors().
                Concat(new AS3Method[]
                { GetSerializeMethod(),
                  GetDeserializeMethod() })
                .ToArray();
        }

        protected override AS3Field[] GetFieldsToWrite()
        {
            return base.GetFieldsToWrite();
        }
        public SerializableConverter(AS3File file) : base(file)
        {

        }
    }
}
