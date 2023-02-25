using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM.Services
{
    public class BaseService
    {
        public static Task consoleKeyTask = Task.Run(() => { MonitorKeypress(); });

        public static  async Task StartTask()
        {
            //await Task.Delay(100);
            await consoleKeyTask;
        }
        public static void MonitorKeypress()
        {
            ConsoleKeyInfo cki = new ConsoleKeyInfo();
            do
            {
                // true hides the pressed character from the console
                cki = Console.ReadKey(true);

                // Wait for an ESC
            } while (cki.Key != ConsoleKey.Escape);

            Environment.Exit(0);
        }
    }
}
