using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR;
using RemoteUpdater.Common.Helper;
using RemoteUpdater.Receiver.Helper;
using System.Threading.Tasks;

namespace RemoteUpdater.Receiver.Communication
{
    internal static class WebHostBuilder
    {
        private static IWebHost _host;

        internal static async Task Run()
        {
            if (_host == null && IsStopped)
            {
                var builder = WebHost.CreateDefaultBuilder();

                builder.UseStartup<Startup>();

                builder.UseUrls($"http://{IpAddressHelper.GetIp4Address()}:{SettingsHelper.Settings.HttpPort}");

                _host = builder.Build();

                HubContext = (IHubContext<CommunicationHub>)_host.Services.GetService(typeof(IHubContext<CommunicationHub>));

                IsStopped = false;

                await _host.RunAsync();
            }
        }

        internal static bool IsStopped { get; set; } = true;

        internal static async Task Stop()
        {
            if (_host != null && !IsStopped)
            {
                await _host.StopAsync();

                IsStopped = true;

                _host.Dispose();

                _host = null;
            }
        }

        internal static async Task Restart()
        {
            await Stop();
            await Run();
        }

        internal static IHubContext<CommunicationHub> HubContext {get; private set;}
    }
}
