using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;

namespace ICICI_Dealer_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    //webBuilder.UseStartup<Startup>().ConfigureKestrel(c =>
                    //c.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(2)
                    //);
                    webBuilder.ConfigureKestrel(serverOptions =>
                    {
                        serverOptions.ConfigureHttpsDefaults(co =>
                        {
                            co.SslProtocols = SslProtocols.Tls12;
                        });
                    });
                    webBuilder.UseWebRoot("wwwroot").UseStartup<Startup>();
                });
    }
}
