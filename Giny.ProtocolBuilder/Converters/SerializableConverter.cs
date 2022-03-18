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
        public override void Prepare()
        {
            var serializeMethod = GetMethodToWrite("Serialize");
            var deserializeMethod = GetMethodToWrite("Deserialize");

            serializeMethod.RenameVariable("NaN", "double.NaN");
            serializeMethod.RenameVariable("super", "base");
            serializeMethod.RenameVariable("output", "writer");
            serializeMethod.RenameVariable("length", "Length");
            serializeMethod.RenameType("ICustomDataOutput", "IDataWriter");
            serializeMethod.RenameMethodCall("writeUTF", "WriteUTF");
            serializeMethod.RenameMethodCall("writeVarInt", "WriteVarInt");
            serializeMethod.RenameMethodCall("writeByte", "WriteByte");
            serializeMethod.RenameMethodCall("writeVarLong", "WriteVarLong");
            serializeMethod.RenameMethodCall("writeShort", "WriteShort");
            serializeMethod.RenameMethodCall("writeVarShort", "WriteVarShort");
            serializeMethod.RenameMethodCall("writeBoolean", "WriteBoolean");
            serializeMethod.RenameMethodCall("writeDouble", "WriteDouble");
            serializeMethod.RenameMethodCall("writeInt", "WriteInt");
            serializeMethod.RenameMethodCall("writeFloat", "WriteFloat");
            serializeMethod.RenameMethodCall("writeUnsignedInt", "WriteUInt");
            serializeMethod.RenameMethodCall("setFlag", "SetFlag");
            serializeMethod.RenameMethodCall("serialize", "Serialize");

            serializeMethod.ReplaceUnchangedExpression("getTypeId()", "TypeId");

            serializeMethod.SetModifiers(AS3ModifiersEnum.@override);
            DofusHelper.IOWriteCastRecursively(serializeMethod.Expressions);
            DofusHelper.DeductFieldTypes(File, serializeMethod.Expressions); // order is importants!
            DofusHelper.RenameDofusTypesSerializeMethodsRecursively(serializeMethod.Expressions);
            DofusHelper.RenameSerializeAs_(serializeMethod.Expressions);
            DofusHelper.ChangeTypeIdToProperty(serializeMethod.Expressions);

            deserializeMethod.RenameMethodCall("readByte", "ReadByte");

            deserializeMethod.RenameVariable("NaN", "double.NaN");
            deserializeMethod.RenameVariable("super", "base");
            deserializeMethod.RenameVariable("input", "reader");
            deserializeMethod.RenameVariable("length", "Length");
            deserializeMethod.RenameType("ICustomDataInput", "IDataReader");
            deserializeMethod.RenameMethodCall("getInstance", "GetInstance");
            deserializeMethod.RenameMethodCall("readUTF", "ReadUTF");
            deserializeMethod.RenameMethodCall("readVarInt", "ReadVarInt");
            deserializeMethod.RenameMethodCall("readVarLong", "ReadVarLong");
            deserializeMethod.RenameMethodCall("readShort", "ReadShort");
            deserializeMethod.RenameMethodCall("readVarShort", "ReadVarShort");
            deserializeMethod.RenameMethodCall("readBoolean", "ReadBoolean");
            deserializeMethod.RenameMethodCall("readDouble", "ReadDouble");
            deserializeMethod.RenameMethodCall("readInt", "ReadInt");
            deserializeMethod.RenameMethodCall("readUnsignedInt", "ReadUInt");
            deserializeMethod.RenameMethodCall("readUnsignedShort", "ReadUShort");
            deserializeMethod.RenameMethodCall("readVarUhShort", "ReadVarUhShort");
            deserializeMethod.RenameMethodCall("readVarUhInt", "ReadVarUhInt");
            deserializeMethod.RenameMethodCall("readVarUhLong", "ReadVarUhLong");
            deserializeMethod.RenameMethodCall("readUnsignedByte", "ReadSByte");
            deserializeMethod.RenameMethodCall("readFloat", "ReadFloat");
            deserializeMethod.RenameMethodCall("getFlag", "GetFlag");
            deserializeMethod.RenameMethodCall("deserialize", "Deserialize");

            deserializeMethod.SetModifiers(AS3ModifiersEnum.@override);
            DofusHelper.IOReadCastRecursively(this, File, deserializeMethod, deserializeMethod.Expressions);
            DofusHelper.InstantiateArrays(this, File, deserializeMethod);
            DofusHelper.TransformVectorPushIntoCSharpArrayIndexer(File, this, deserializeMethod);
            DofusHelper.CreateGenericTypeForProtocolInstance(deserializeMethod.Expressions);
            DofusHelper.CastEnumsComparaisons(deserializeMethod.Expressions);
        }
        protected override AS3Method[] SelectMethodsToWrite()
        {
            return
                GetCtors().
                Concat(new AS3Method[]
                {
                    GetSerializeMethod(),
                    GetDeserializeMethod()}
                )
                .ToArray();
        }

        protected override AS3Field[] SelectFieldsToWrite()
        {
            return base.SelectFieldsToWrite();
        }
        public SerializableConverter(AS3File file) : base(file)
        {

        }
    }
}
