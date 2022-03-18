using Giny.AS3;
using Giny.Core;
using Giny.Core.Extensions;
using Giny.ProtocolBuilder.Converters;
using Giny.ProtocolBuilder.Templates;
using Microsoft.VisualStudio.TextTemplating;
using Mono.TextTemplating;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Giny.ProtocolBuilder.Profiles
{
    public abstract class Profile
    {
        public abstract string TemplateFileName
        {
            get;
        }
        public abstract string RelativeOutputPath
        {
            get;
        }
        private string OutputPath
        {
            get;
            set;
        }
        public abstract string OutputDirectory
        {
            get;
        }

        public string TemplatePath
        {
            get
            {
                return Path.Combine(Path.Combine(Environment.CurrentDirectory, Constants.TEMPLATES_PATH), TemplateFileName);
            }
        }
        public AS3File AS3File
        {
            get;
            private set;
        }
        public virtual bool ParseMethods => true;

        public Profile(string as3FilePath)
        {
            this.AS3File = new AS3File(as3FilePath, ParseMethods);
        }

        public abstract DofusConverter CreateDofusConverter();
        /// <summary>
        /// improve
        /// </summary>
        /// <returns></returns>
        public string GetRelativeOutputPath()
        {
            if (RelativeOutputPath == string.Empty)
                return string.Empty;

            var directory = Path.GetDirectoryName(AS3File.FilePath);
            string dir = directory.Replace(RelativeOutputPath, string.Empty);
            dir = dir.UpperAfterChar('\\') + "\\";
            return dir;
        }

        public abstract bool Skip();

        public string ProcessTemplate()
        {
            if (Skip())
            {
                Logger.Write(AS3File.FileName + " skipped.", Channels.Warning);
                return string.Empty;
            }
            var directoryPath = Path.Combine(OutputDirectory, GetRelativeOutputPath());

            var engine = new TemplatingEngine();
            var host = new TemplateHost(TemplatePath);

            var converter = CreateDofusConverter();
            host.Session["Converter"] = converter;

            var text = File.ReadAllText(TemplatePath);
            var output = engine.ProcessTemplate(text, host);

            foreach (CompilerError error in host.Errors)
            {
                Logger.Write(error.ErrorText + " line (" + error.Line + ")", Channels.Critical);
            }


            if (host.Errors.Count > 0)
            {
                Console.Read();
                Environment.Exit(0);
            }

            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            string filePath = directoryPath + AS3File.ClassName + host.FileExtension;

            File.WriteAllText(filePath, output);

            return filePath;
        }
    }
}
