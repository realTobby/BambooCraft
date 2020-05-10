using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BambooCraft
{

    public enum Severity
    {
        Information,
        Warning,
        Error,
        Network,
        Packet,
        Exception,
        Critical
    }
    public class Logging
    {
        public void Log(Severity severity, string Message)
        {
            SetColor(severity);
            Console.WriteLine("[" + severity.ToString() + "] " + Message);
            Console.ForegroundColor = ConsoleColor.White;
        }

        private void SetColor(Severity severity)
        {
            switch(severity)
            {
                case Severity.Information:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case Severity.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case Severity.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case Severity.Critical:
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    break;
                case Severity.Packet:
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    break;
                case Severity.Network:
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    break;
                case Severity.Exception:
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    break;
            }
        }

    }
}
