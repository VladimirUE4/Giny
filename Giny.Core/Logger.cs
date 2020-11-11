using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Giny.Core
{
    public enum MessageState
    {
        INFO = 0,
        INFO2 = 1,
        IMPORTANT_INFO = 2,
        WARNING = 3,
        ERROR = 4,
        ERROR_FATAL = 5,
        SUCCES = 6,
    }
    public class Logger
    {
        private const ConsoleColor COLOR_1 = ConsoleColor.Magenta;
        private const ConsoleColor COLOR_2 = ConsoleColor.DarkMagenta;

        private static Dictionary<MessageState, ConsoleColor> Colors = new Dictionary<MessageState, ConsoleColor>()
        {
            { MessageState.INFO,            ConsoleColor.Gray },
            { MessageState.INFO2,           ConsoleColor.DarkGray },
            { MessageState.IMPORTANT_INFO,  ConsoleColor.White },
            { MessageState.SUCCES,          ConsoleColor.Green },
            { MessageState.WARNING,         ConsoleColor.Yellow },
            { MessageState.ERROR ,          ConsoleColor.DarkRed},
            { MessageState.ERROR_FATAL,     ConsoleColor.Red }
        };

        private static void Logo()
        {
            WriteColor1(@"   ______   _                     ");
            WriteColor1(@" .' ___  | (_)                    ");
            WriteColor1(@"/ .'   \_| __   _ .--.    _   __  ");
            WriteColor1(@"| |   ____[  | [ `.-. |  [ \ [  ] ");
            WriteColor1(@"\ `.___]   | |  | | | |   \ '/ /  ");
            WriteColor2(@" `._____.'[___][___||__][\_:  /   ");
            WriteColor2(@"     written by Skinz    \__.'    ");

        }
        public static void Write(object value, MessageState state = MessageState.INFO)
        {
            WriteColored(value, Colors[state]);
        }
        public static void WriteColor1(object value)
        {
            WriteColored(value, COLOR_1);
        }
        public static void WriteColor2(object value)
        {
            WriteColored(value, COLOR_2);
        }
        private static void WriteColored(object value, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(value);
        }
        public static void NewLine()
        {
            Console.WriteLine(Environment.NewLine);
        }
        public static void OnStartup()
        {
            Console.Title = Assembly.GetCallingAssembly().GetName().Name;
            Logo();
            NewLine();
        }
    }
}
