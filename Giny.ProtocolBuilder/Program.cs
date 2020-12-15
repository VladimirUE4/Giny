using Giny.AS3;
using Giny.AS3.Converter;
using Giny.AS3.Expressions;
using Giny.Core;
using Giny.ProtocolBuilder.Converters;
using Giny.ProtocolBuilder.Profiles;
using Giny.ProtocolBuilder.Templates;
using Microsoft.VisualStudio.TextTemplating;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.ProtocolBuilder
{
    class Program
    {
        static void Main(string[] args)
        {
            Logger.OnStartup();
            Stopwatch stopwatch = Stopwatch.StartNew();


            // BuildEnums();

           // BuildMessages();
          //  BuildTypes();
            BuildDatacenter();

            Logger.WriteColor1(string.Format("Build finished in {0}s", stopwatch.Elapsed.Seconds));

            Console.Read();
        }

        static void BuildEnums()
        {
            Logger.Write("Writting Enums...");


            string path = Path.Combine(Constants.SOURCES_PATH, Constants.ENUMS_PATH);

            foreach (string file in Directory.EnumerateFiles(path, "*.*", SearchOption.AllDirectories))
            {
                Logger.Write("Writting enum : " + Path.GetFileNameWithoutExtension(file), MessageState.INFO2);
                EnumProfile @enum = new EnumProfile(file);
                @enum.ProcessTemplate();
            }

        }

        static void BuildMessages()
        {
            Logger.Write("Building Messages...");

            string outputPath = Path.Combine(Environment.CurrentDirectory, Constants.MESSAGES_OUTPUT_PATH);
            string path = Path.Combine(Constants.SOURCES_PATH, Constants.MESSAGES_PATH);

            foreach (string file in Directory.EnumerateFiles(path, "*.*", SearchOption.AllDirectories))
            {
                Logger.Write("Writting message : " + Path.GetFileNameWithoutExtension(file), MessageState.INFO2);
                MessageProfile message = new MessageProfile(file);
                message.ProcessTemplate();
            }
        }
        static void BuildTypes()
        {
            Logger.Write("Building Types...");

            string outputPath = Path.Combine(Environment.CurrentDirectory, Constants.TYPES_OUTPUT_PATH);
            string path = Path.Combine(Constants.SOURCES_PATH, Constants.TYPES_PATH);

            foreach (string file in Directory.EnumerateFiles(path, "*.*", SearchOption.AllDirectories))
            {
                Logger.Write("Writting type : " + Path.GetFileNameWithoutExtension(file), MessageState.INFO2);
                TypeProfile type = new TypeProfile(file);
                type.ProcessTemplate();

            }


        }
        static void BuildDatacenter()
        {
            Logger.Write("Building Datacenter...");

            string outputPath = Path.Combine(Environment.CurrentDirectory, Constants.DATACENTER_OUTPUT_PATH);
            string path = Path.Combine(Constants.SOURCES_PATH, Constants.DATACENTER_PATH);

            foreach (string file in Directory.EnumerateFiles(path, "*.*", SearchOption.AllDirectories))
            {
                Logger.Write("Writting datacenter : " + Path.GetFileNameWithoutExtension(file), MessageState.INFO2);
                DatacenterProfile datacenter = new DatacenterProfile(file);
                datacenter.ProcessTemplate();
            }
        }
    }
}
