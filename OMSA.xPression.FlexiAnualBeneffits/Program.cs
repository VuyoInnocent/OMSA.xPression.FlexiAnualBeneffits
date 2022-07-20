using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OMSA.xPression.FlexiAnualBeneffits
{
    class ServiceWrapper : HostService
    {
        static void Main(string[] args)
        {
            if (args.Length == 0) // Run as a service
            {
                Trace.WriteLine("Started service.");
                Run(new ServiceWrapper());
            }
            else if (args[0] == "-i") // Install
            {
                InstallService(ConfigurationManager.AppSettings["servicename"], ConfigurationManager.AppSettings["servicedescription"], null, null);
            }
            else if (args[0] == "-u") // Uninstall
            {
                UninstallService(ConfigurationManager.AppSettings["servicename"]);
            }
            else if (args[0] == "-t") // Test
            {
                ServiceWrapper testService = new ServiceWrapper();
                Trace.WriteLine("Started service in test mode (press Esc to stop).");
                testService.OnStart(args);
                while (Console.ReadKey(true).Key != ConsoleKey.Escape)
                {
                    Thread.Sleep(100);
                }
                testService.OnStop();
            }
            else
            {
                Console.WriteLine("Argument: [-i | -u | -t] (Install, Uninstall, Test)");
            }
        }
    }
}
