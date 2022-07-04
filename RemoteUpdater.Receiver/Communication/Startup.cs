using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RemoteUpdater.Receiver.Helper;

namespace RemoteUpdater.Receiver.Communication
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var timeout = new System.TimeSpan(0, (int)SettingsHelper.Settings.TimeOutInMinutes, 0);

            services.AddSignalR(configure => { configure.MaximumReceiveMessageSize = null; configure.ClientTimeoutInterval = timeout; });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}

            //app.UseHttpsRedirection();

            app.UseRouting();

            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<CommunicationHub>("/CommunicationHub");
            });
        }
    }
}
