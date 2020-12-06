using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Giny.Core.DesignPattern
{
    public class WIPManager
    {
        public static BindingFlags BindingFlags = BindingFlags.NonPublic | BindingFlags.Public
          | BindingFlags.Default | BindingFlags.GetField | BindingFlags.SetField | BindingFlags.Instance | BindingFlags.Static;

        public const string TypeLog = "Class: ({1}.cs) Type : ({0})  State: ({2}) Comment: ({3})";
        public const string MethodLog = "Class: ({1}.cs) Method: {0}() State: ({2}) Comment: ({3})";
        public const string FieldLog = "Class: ({1}.cs) Field: ({0}) State: ({2}) Comment: ({3})";

        public static void Analyse(Assembly assembly)
        {
            string result = string.Empty;
            Stopwatch stopwatch = Stopwatch.StartNew();
            foreach (var type in assembly.GetTypes())
            {
                WIPAttribute attribute = type.GetCustomAttributes(false).OfType<WIPAttribute>().FirstOrDefault();

                if (attribute != null)
                {
                    Print(TypeLog, type.Name, type.Name, attribute.State, attribute.Comment);
                }

                foreach (var method in type.GetMethods(BindingFlags).Where(x => x.DeclaringType == type))
                {
                    attribute = method.GetCustomAttributes(false).OfType<WIPAttribute>().FirstOrDefault();

                    if (attribute != null)
                    {
                        Print(MethodLog, method.Name, type.Name, attribute.State, attribute.Comment);
                    }
                }
                foreach (var field in type.GetFields(BindingFlags))
                {
                    attribute = field.GetCustomAttributes(false).OfType<WIPAttribute>().FirstOrDefault();

                    if (attribute != null)
                    {
                        Print(FieldLog, field.Name, type.Name, attribute.State, attribute.Comment);
                    }
                }
                foreach (var property in type.GetProperties(BindingFlags))
                {
                    attribute = property.GetCustomAttributes(false).OfType<WIPAttribute>().FirstOrDefault();

                    if (attribute != null)
                    {
                        Print(FieldLog, property.Name, type.Name, attribute.State, attribute.Comment);
                    }
                }
            }

            Logger.Write("Analysis executed in " + stopwatch.ElapsedMilliseconds + "ms", MessageState.INFO);

        }
        private static void Print(string formatter, string name, string type, WIPState state, string comment)
        {
            string result = string.Format(formatter, name, type, state, comment);

            switch (state)
            {
                case WIPState.Ok:
                    Logger.Write(result, MessageState.SUCCES);
                    break;
                case WIPState.Architecture:
                    Logger.Write(result, MessageState.WARNING);
                    break;
                case WIPState.BadCode:
                    Logger.Write(result, MessageState.ERROR);
                    break;
                case WIPState.Todo:
                    Logger.WriteColor1(result);
                    break;
                default:
                    throw new Exception("Unhandled WIP State : " + state);
            }
        }
    }
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = false)]
    public class WIPAttribute : Attribute
    {
        public WIPState State
        {
            get;
            private set;
        }
        public string Comment
        {
            get;
            private set;
        }
        public WIPAttribute(WIPState state = WIPState.Ok, string comment = null)
        {
            this.State = state;
            this.Comment = comment;
        }
        public WIPAttribute(string comment)
        {
            this.Comment = comment;
            this.State = WIPState.None;
        }
    }
    public enum WIPState
    {
        Ok,
        BadCode,
        Todo,
        Architecture,
        None,
    }
}
