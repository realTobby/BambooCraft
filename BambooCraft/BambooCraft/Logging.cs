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
        private Severity loggingSeverity;

        public Logging(Severity loggingSeverity)
        {
            this.loggingSeverity = loggingSeverity;
        }

        public void Log(string Message)
        {
            SetColor();
            Console.WriteLine("[" + loggingSeverity.ToString() + "] " + Message);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public void Log(Severity loggingSeverity, string Message)
        {
            Severity lastSeverity = this.loggingSeverity;
            this.loggingSeverity = loggingSeverity;
            SetColor();
            Console.WriteLine("[" + loggingSeverity.ToString() + "] " + Message);
            Console.ForegroundColor = ConsoleColor.White;
            this.loggingSeverity = lastSeverity;
        }

        private void SetColor()
        {
            switch(loggingSeverity)
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
