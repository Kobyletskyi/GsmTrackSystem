
#region using
using System;
using System.Collections.Generic;
using System.Net.Http;
using Bogus;
using BusinessLayer.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Api.Integration.Tests.Extentions;
using FluentAssertions;
using System.Net;
using Microsoft.Net.Http.Headers;
using System.Linq;
using System.Text.RegularExpressions;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.IO;
#endregion

namespace Api.Integration.Tests
{
    public class CommonFixture : IDisposable
    {
        private TestServer _server;
        public HttpClient Client { get; private set; }
        public readonly Faker Faker;

        public readonly string AuthCookieName = ".AspNetCore.Cookies";

        public Queue<string> CleanupResources = new Queue<string>();

        public CommonFixture()
        {
            SetUpClient();
            Faker = new Faker();
        }

        public FormUrlEncodedContent GetUserForm(string userName, string password){
            var content = new Dictionary<string, string>();
            content.Add("userName", userName);
            content.Add("password", password);
            
            return new FormUrlEncodedContent(content);
        }

        private void SetUpClient()
        {
            _server = new TestServer(
                new WebHostBuilder()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseWebRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
            );

            Client = _server.CreateClient();
        }

        #region IDisposable Support
        protected bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {   
                if (disposing)
                {
                    foreach (var url in CleanupResources)
                    {
                        var res = Task.Run(()=>Client.DeleteAsync(url)).Result;
                        Console.WriteLine("Cleanup {0} {1}", url, res.StatusCode);
                    }
                    _server?.Dispose();
                    Client?.Dispose();
                }
                disposedValue = true;
            }
        }
        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}