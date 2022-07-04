using System.Diagnostics;
using System.ServiceProcess;

namespace RemoteUpdater.Common.Helper
{
    public static class WindowsServiceHelper
    {
        public static bool StartService(string serviceName)
        {
            var success = false;

            try
            {
                if (ServiceExists(serviceName))
                {
                    ServiceController sc = new ServiceController(serviceName);

                    if (sc.Status.Equals(ServiceControllerStatus.Stopped) || sc.Status.Equals(ServiceControllerStatus.StopPending))
                    {
                        sc.Start();
                        success = true;
                    }
                }
            }
            catch (Exception e)
            {
                Trace.WriteLine(e);
            }

            return success;
        }

        public static bool StopService(string serviceName)
        {
            var success = false; 

            try
            {
                if (ServiceExists(serviceName))
                {
                    ServiceController sc = new ServiceController(serviceName);

                    if (sc.Status.Equals(ServiceControllerStatus.Running) || sc.Status.Equals(ServiceControllerStatus.StartPending))
                    {
                        sc.Stop();
                    }

                    int sleepCount = 10;

                    while (!sc.Status.Equals(ServiceControllerStatus.Stopped) && sleepCount > 0)
                    {
                        Thread.Sleep(500);
                        sleepCount--;
                    }

                    success = sleepCount < 10;
                }
            }
            catch (Exception e)
            {
                Trace.WriteLine(e);
            }

            return success;
        }

        private static bool ServiceExists(string serviceName)
        {
            return ServiceController.GetServices().FirstOrDefault(s => s.ServiceName == serviceName) != null;
        }
    }
}
