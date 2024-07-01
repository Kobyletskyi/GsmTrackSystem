using System.Net;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using System.Collections.Generic;
using System.Net.Http;
using Bogus;
using System;
using System.Linq;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using BusinessLayer.Models;
using Api.Integration.Tests.Extentions;
using Api.Models;
using System.Text;

namespace Api.Integration.Tests
{
    [Collection(nameof(ExistingUserCollection))]
    public class UsersTests {
        private readonly ExistingUserFixture Fixture;
        public UsersTests(ExistingUserFixture fixture)
        {
            Fixture = fixture;
        }
        [Fact]
        public async Task SuccessfullyGetExistingAccountWithCookie()
        {
            await Fixture.AuthorizeUserAsync(Authorization.Cookie);
            HttpResponseData<User> result = await Fixture.Client.GetAsync<User>(Fixture.CreatedUserResponse.Location);

            result.Response.EnsureSuccessStatusCode();
            result.Response.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Data.Id.Should().Be(Fixture.CreatedUserResponse.Data.Id);
            result.Data.UserName.Should().Be(Fixture.CreatedUserResponse.Data.UserName);
            Fixture.ClearAuthorization(Authorization.Cookie);
        }
        [Fact]
        public async Task SuccessfullyGetExistingAccountWithBearer()
        {
            await Fixture.AuthorizeUserAsync(Authorization.Bearer);
            HttpResponseData<User> result = await Fixture.Client.GetAsync<User>(Fixture.CreatedUserResponse.Location);

            result.Response.EnsureSuccessStatusCode();
            result.Response.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Data.Id.Should().Be(Fixture.CreatedUserResponse.Data.Id);
            result.Data.UserName.Should().Be(Fixture.CreatedUserResponse.Data.UserName);
            Fixture.ClearAuthorization(Authorization.Bearer);
        }
        [Fact]
        public async Task SuccessfullyDeleteOwnUserWithCookie()
        {
            string userName = Fixture.Faker.Internet.UserName();
            string password = Fixture.Faker.Internet.Password();
            var credentials = new { userName, password };
            var userPostResult = await Fixture.Client.PostJsonAsync<User>(Api.Signup, credentials);
            IEnumerable<string> cookieHeader;
            userPostResult.Response.Headers.TryGetValues(HeaderNames.SetCookie, out cookieHeader);
            Fixture.Client.DefaultRequestHeaders.Add(HeaderNames.Cookie, cookieHeader.First());

            var deleteResponse = await Fixture.Client.DeleteAsync(userPostResult.Location);

            deleteResponse.EnsureSuccessStatusCode();
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

            HttpResponseData<User> userGetResult = await Fixture.Client.GetAsync<User>(userPostResult.Location);

            userGetResult.Response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            userGetResult.Data.Should().BeNull();

            
            var signinResult = await Fixture.Client.PostJsonAsync<object>(Api.Signin, credentials);
            
            signinResult.Response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            signinResult.Response.Headers.GetCookie(Fixture.AuthCookieName).Should().BeNullOrWhiteSpace();

            Fixture.Client.DefaultRequestHeaders.Remove(HeaderNames.Cookie);
            Fixture.ClearAuthorization(Authorization.Cookie);
        }
        public async Task UnsuccessfullyDeleteOtherUser()
        {
            string userName = Fixture.Faker.Internet.UserName();
            string password = Fixture.Faker.Internet.Password();
            var credentials = new { userName, password };
            var userPostResult = await Fixture.Client.PostJsonAsync<User>(Api.Signup, credentials);
            IEnumerable<string> cookieHeader;
            userPostResult.Response.Headers.TryGetValues(HeaderNames.SetCookie, out cookieHeader);
            await Fixture.AuthorizeUserAsync(Authorization.Cookie);

            var deleteResponse = await Fixture.Client.DeleteAsync(userPostResult.Location);

            deleteResponse.StatusCode.Should().Be(HttpStatusCode.Forbidden);

            HttpResponseData<User> userGetResult = await Fixture.Client.GetAsync<User>(userPostResult.Location);

            userGetResult.Response.EnsureSuccessStatusCode();
            userGetResult.Response.StatusCode.Should().Be(HttpStatusCode.OK);
            userGetResult.Data.Should().NotBeNull();

            var signinResult = await Fixture.Client.PostJsonAsync<object>(Api.Signin, credentials);
            
            signinResult.Response.EnsureSuccessStatusCode();
            signinResult.Response.StatusCode.Should().Be(HttpStatusCode.OK);
            signinResult.Response.Headers.GetCookie(Fixture.AuthCookieName).Should().NotBeNullOrWhiteSpace();

            Fixture.Client.DefaultRequestHeaders.Remove(HeaderNames.Cookie);
        }
    }
}