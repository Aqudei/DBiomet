using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            File.WriteAllText("printing.txt", "HELLO\r\nWORLD");

            var procInfo = new ProcessStartInfo
            {
                FileName = "print",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                Arguments = string.Format("/D:\"\\\\{0}\\XP58C\" \"printing.txt\"", Environment.MachineName)
            };

            var p = Process.Start(procInfo);
            Console.WriteLine(p.StandardOutput.ReadToEnd());
            Console.ReadLine();
        }
    }
}
