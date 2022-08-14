using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

namespace RemoteUpdater.Common.Helper
{
    public class IpAddressHelper
    {
        public static string GetIp4Address()
        {
            string? localIP = "127.0.0.1";

            try
            {
                using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
                {

                    socket.Connect("8.8.8.8", 65530);
                    IPEndPoint? endPoint = socket.LocalEndPoint as IPEndPoint;
                    if (endPoint != null)
                    {
                        localIP = endPoint.Address.ToString();
                    }
                }
            }
            catch (Exception e)
            {
                Trace.WriteLine($"Error getting IP Adresse. Exception: {e}");
            }

            return localIP;
        }
    }
}
