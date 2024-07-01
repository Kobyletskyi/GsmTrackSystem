using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using BusinessLayer.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Api.Integration.Tests.Extentions;
using System.Threading.Tasks;
using Api.Models;

namespace Api.Integration.Tests
{
    public class ExistingUserFixture : CommonFixture
    {
        public HttpResponseData<User> CreatedUserResponse { get; }

        public ExistingUserFixture(): base()
        {
            string userName = Faker.Internet.UserName();
            string password = Faker.Internet.Password();
            var credentials = new { userName, password };
            CreatedUserResponse = Task.Run(()=>Client.PostJsonAsync<User>(Api.Signup, credentials)).Result;
            CreatedUserResponse.Data.PasswordSalt = password;
            CleanupResources.Enqueue(CreatedUserResponse.Location);
        }

        public async Task<bool> AuthorizeUserAsync(Authorization type, string userName = null, string password = null) {
            userName = userName ?? CreatedUserResponse.Data.UserName;
            password = password ?? CreatedUserResponse.Data.PasswordSalt;
            switch (type)
            {
                case Authorization.Bearer: {
                    var data = new {
                        userName,
                        password
                    };
                    Client.DefaultRequestHeaders.Remove(HeaderNames.Authorization);
                    HttpResponseData<Token> result = await Client.PostJsonAsync<Token>(Api.TokenJson, data);

                    result.Response.EnsureSuccessStatusCode();
                    result.Response.StatusCode.Should().Be(HttpStatusCode.OK);
                    
                    result.Data.AccessToken.Should().NotBeNullOrEmpty();
                    result.Data.Expires.Should().BeGreaterThan(0);

                    Client.DefaultRequestHeaders.Add(HeaderNames.Authorization, String.Format("Bearer {0}", result.Data.AccessToken));
                    break;
                }
                default:{
                    var result = await Client.PostJsonAsync<object>(Api.Signin, new { userName, password });
                    result.Response.EnsureSuccessStatusCode();
                    result.Response.StatusCode.Should().Be(HttpStatusCode.OK);
                    result.Response.Headers.GetCookie(AuthCookieName).Should().NotBeNullOrWhiteSpace();
                    IEnumerable<string> cookieHeader;
                    result.Response.Headers.TryGetValues(HeaderNames.SetCookie, out cookieHeader);
                    Client.DefaultRequestHeaders.Add(HeaderNames.Cookie, cookieHeader.First());
                    break;
                }
            }
            return true;
        }
        public void ClearAuthorization(Authorization type) {
            switch (type)
            {
                case Authorization.Bearer: 
                    Client.DefaultRequestHeaders.Remove(HeaderNames.Authorization);
                    break;
                case Authorization.Cookie: 
                    Client.DefaultRequestHeaders.Remove(HeaderNames.Cookie);
                    break;
                default:
                    Client.DefaultRequestHeaders.Remove(HeaderNames.Authorization);
                    Client.DefaultRequestHeaders.Remove(HeaderNames.Cookie);
                    break;
            }
        }

        public DeviceForCreation GenerateNewDevice(){
            return new DeviceForCreation(){
                OwnerId = CreatedUserResponse.Data.Id,
                IMEI = Faker.Internet.Mac(),
                Title = Faker.Commerce.Product(),
                Description = Faker.Lorem.Text(),
                SoftwareVersion = Faker.System.Version().ToString()
            };
        }
        protected override void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                var res = Task.Run(()=>AuthorizeUserAsync(Authorization.Bearer)).Result;
            }
            base.Dispose(disposing);
        }
    }
}