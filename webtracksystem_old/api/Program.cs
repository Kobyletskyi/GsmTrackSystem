using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using System.Net;

namespace Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            
            //X509Certificate2 cert = getCertificate();
            string webFolder = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development"
                ? @"..\web\content"
                : @"content";
            string currentPath = Directory.GetCurrentDirectory();
            string webPath = Path.Combine(currentPath, webFolder);
            var host = new WebHostBuilder()
                .UseKestrel(options => {
                    options.AddServerHeader = false;
                    //options.Listen(IPAddress.Any, 443, listenOptions => listenOptions.UseHttps(cert));
                     })
                .UseContentRoot(currentPath)
                .UseWebRoot(webPath)
                .UseIISIntegration()
                .UseStartup<Startup>()
                //.UseEnvironment("Staging")
                .Build();

            host.Run();
        }
        private static X509Certificate2 getCertificate()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddEnvironmentVariables()
                .AddJsonFile("certificate.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"certificate.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", reloadOnChange: true, optional: true)
                .Build();
            var certificateSettings = config.GetSection("certificateSettings");
            string certificateFileName = certificateSettings.GetValue<string>("filename");
            string certificatePassword = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development"
                ? certificateSettings.GetValue<string>("password")
                : Environment.GetEnvironmentVariable("CERT_PASSWORD");
            return new X509Certificate2(certificateFileName, certificatePassword);
        } 
    }
}
