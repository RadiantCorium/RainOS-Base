using RainOS.core.objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RainOS.core.services
{
    /// <summary>
    /// Process Manager
    /// </summary>
    internal class PM
    {
        private static List<Process> processes;

        internal static void Init()
        {
            processes = new List<Process>();
        }

        internal static List<Process> GetProcesses()
        {
            return new List<Process>(processes);
        }

        internal static void AddProcess(Process process)
        {
            try
            {
                processes.Add(process);
                process.init();
            }
            catch (Exception ex)
            {
                if (Globals.consoleMode)
                {
                    Console.WriteLine("PM Init error: " + ex.ToString());
                }
                else
                {
                    // TODO: graphical mode popup
                }
            }
        }

        internal static void RemoveProcess(Process process, bool force)
        {
            try
            {
                if (!force)
                {
                    if (process.isCritical)
                    {
                        Console.WriteLine("Unable to kill critical process '" + process.name + "'");
                        return;
                    }
                    process.destroy();
                }

                if (process.isCritical)
                {
                    BSOD.Trigger(new Exception("Critical process '" + process.name + "' died!"));
                }

                processes.Remove(process);
            }
            catch (Exception ex)
            {
                if (Globals.consoleMode)
                {
                    Console.WriteLine("PM Destroy error: " + ex.ToString());
                }
                else
                {
                    // TODO: graphical mode popup
                }
            }
        }

        internal static void UpdateAll()
        {
            try
            {
                foreach (Process process in processes)
                {
                    try
                    {
                        process.update();
                    }
                    catch (Exception e)
                    {
                        // crash the application
                        RemoveProcess(process, true);
                    }
                }
            }
            catch (Exception ex)
            {
                if (Globals.consoleMode)
                {
                    Console.WriteLine("PM Update error: " + ex.ToString());
                }
                else
                {
                    // TODO: graphical mode popup
                }
            }
        }
    }
}
