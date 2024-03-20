using IRIDIUM_GMDSS_LRIT.Core.ProcessingTask;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRIDIUM_GMDSS_LRIT.Forwarder.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("IRIDIUM GMDSS FORWARDER");

            PositionForwaderTask positionForwarderTask = new PositionForwaderTask();
            positionForwarderTask.SleepIntervalInSeconds = 30;
            positionForwarderTask.StartTask();

            System.Console.ReadLine();
        }
    }
}
