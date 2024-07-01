#region using
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
#endregion

namespace Api.Integration.Tests
{
    [Collection(nameof(AnonymousCollection))]
    public class AuthSignupTests {
        private readonly CommonFixture Fixture;
        public AuthSignupTests(CommonFixture fixture)
        {
            Fixture = fixture;
        }
        [Fact]
        public async Task SignupReturnsCreatedWithLocationAndCookie()
        {
            string userName = Fixture.Faker.Internet.UserName();
            string password = Fixture.Faker.Internet.Password();
            var credentials = new { userName, password };

            var result = await Fixture.Client.PostJsonAsync<User>(Api.Signup, credentials);            
            result.Response.EnsureSuccessStatusCode();
            result.Response.StatusCode.Should().Be(HttpStatusCode.Created);

            result.Data.Should().NotBeNull();
            result.Data.UserName.Should().Be(userName);
            result.Data.Id.Should().BeGreaterThan(0); 
            
            result.Location.Should().NotBeNullOrWhiteSpace();

            result.Response.Headers.GetCookie(Fixture.AuthCookieName).Should().NotBeNullOrWhiteSpace();
            IEnumerable<string> cookieHeader;
            result.Response.Headers.TryGetValues(HeaderNames.SetCookie, out cookieHeader);
            Fixture.Client.DefaultRequestHeaders.Add(HeaderNames.Cookie, cookieHeader.First());
            Fixture.CleanupResources.Enqueue(result.Location);
        }
        [Fact]
        public async Task SignupWithoutUserNameReturnsBadRequest()
        {
            string password = Fixture.Faker.Internet.Password();
            var credentials = new { password };
            var result = await Fixture.Client.PostJsonAsync<IDictionary<string, string[]>>(Api.Signup, credentials);
            
            result.Response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            result.Data.Should().HaveCount(1);
            result.Data.ValidateMessage("userName");
        }
        [Fact]
        public async Task SignupWithoutPasswordReturnsBadRequest()
        {
            string userName = Fixture.Faker.Internet.UserName();
            var credentials = new { userName };
            var result = await Fixture.Client.PostJsonAsync<IDictionary<string, string[]>>(Api.Signup, credentials);

            result.Response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            result.Data.Should().HaveCount(1);
            result.Data.ValidateMessage("password");
        }
        [Fact]
        public async Task SignupWithEmptyModelReturnsBadRequest()
        {
            var credentials = new { };
            var result = await Fixture.Client.PostJsonAsync<IDictionary<string, string[]>>(Api.Signup, credentials);

            result.Response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            result.Data.Should().HaveCount(2);
            result.Data.ValidateMessage("password");
            result.Data.ValidateMessage("userName");
        }
        [Fact]
        public async Task SignupWithoutModelReturnsBadRequest()
        {
            var result = await Fixture.Client.PostJsonAsync<IDictionary<string, string[]>>(Api.Signup, null);

            result.Response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            result.Data.Should().HaveCount(1);
            result.Data.First().Value.Should().HaveCount(1);
            result.Data.First().Value.First().Should().NotBeNullOrWhiteSpace();
        }
    }

    [Collection(nameof(ExistingUserCollection))]
    public class AuthUserTests {
        private readonly ExistingUserFixture Fixture;
        public AuthUserTests(ExistingUserFixture fixture)
        {
            Fixture = fixture;
        }
        [Fact]
        public async Task GetJsonTokenForExistingAccountReturnsOk()
        {
            var data = new {
                userName = Fixture.CreatedUserResponse.Data.UserName,
                password = Fixture.CreatedUserResponse.Data.PasswordSalt
            };
            HttpResponseData<Token> result = await Fixture.Client.PostJsonAsync<Token>(Api.TokenJson, data);

            result.Response.EnsureSuccessStatusCode();
            result.Response.StatusCode.Should().Be(HttpStatusCode.OK);
            
            result.Data.AccessToken.Should().NotBeNullOrEmpty();
            result.Data.Expires.Should().BeGreaterThan(0);

        }
        [Fact]
        public async Task GetJsonTokenForNotExistingAccountReturnsUnauthorized()
        {
            var data = new {
                userName = Fixture.Faker.Internet.UserName(),
                password = Fixture.Faker.Internet.Password()
            };
            HttpResponseData<Token> result = await Fixture.Client.PostJsonAsync<Token>(Api.TokenJson, data);

            result.Response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

            result.Data.Should().BeNull();

        }
        [Fact]
        public async Task GetJsonTokenWithoutUserNameReturnsBadRequest()
        {
            string password = Fixture.Faker.Internet.Password();
            var credentials = new { password };
            var result = await Fixture.Client.PostJsonAsync<IDictionary<string, string[]>>(Api.TokenJson, credentials);
            
            result.Response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            result.Data.Should().HaveCount(1);
            result.Data.ValidateMessage("userName");
        }
        [Fact]
        public async Task GetJsonTokenWithoutPasswordReturnsBadRequest()
        {
            string userName = Fixture.Faker.Internet.UserName();
            var credentials = new { userName };
            var result = await Fixture.Client.PostJsonAsync<IDictionary<string, string[]>>(Api.TokenJson, credentials);

            result.Response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            result.Data.Should().HaveCount(1);
            result.Data.ValidateMessage("password");
        }
        [Fact]
        public async Task GetJsonTokenWithEmptyModelReturnsBadRequest()
        {
            var credentials = new { };
            var result = await Fixture.Client.PostJsonAsync<IDictionary<string, string[]>>(Api.TokenJson, credentials);

            result.Response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            result.Data.Should().HaveCount(2);
            result.Data.ValidateMessage("password");
            result.Data.ValidateMessage("userName");
        }
        [Fact]
        public async Task GetJsonTokenWithoutModelReturnsBadRequest()
        {
            var result = await Fixture.Client.PostJsonAsync<IDictionary<string, string[]>>(Api.TokenJson, null);

            result.Response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            result.Data.Should().HaveCount(1);
            result.Data.First().Value.Should().HaveCount(1);
            result.Data.First().Value.First().Should().NotBeNullOrWhiteSpace();
        }
        [Fact]
        public async Task GetFormTokenForExistingAccountReturnsOk()
        {
            var content = new Dictionary<string, string>();
            content.Add("userName", Fixture.CreatedUserResponse.Data.UserName);
            content.Add("password", Fixture.CreatedUserResponse.Data.PasswordSalt);
            
            var httpContent = new FormUrlEncodedContent(content);
            HttpResponseData<Token> result = await Fixture.Client.PostAsync<Token>(Api.TokenForm, httpContent);

            result.Response.EnsureSuccessStatusCode();
            result.Response.StatusCode.Should().Be(HttpStatusCode.OK);

            result.Data.AccessToken.Should().NotBeNullOrEmpty();
            result.Data.Expires.Should().BeGreaterThan(0);

        }
        [Fact]
        public async Task GetFormTokenForExistingAccountReturnsUnauthorized()
        {
            var content = new Dictionary<string, string>();
            content.Add("userName", Fixture.Faker.Internet.UserName());
            content.Add("password", Fixture.Faker.Internet.Password());
            
            var httpContent = new FormUrlEncodedContent(content);
            HttpResponseData<Token> result = await Fixture.Client.PostAsync<Token>(Api.TokenForm, httpContent);

            result.Response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

            result.Data.Should().BeNull();

        }
        [Fact]
        public async Task GetFormTokenWithoutUserNameReturnsBadRequest()
        {
            var content = new Dictionary<string, string>();
            content.Add("password", Fixture.CreatedUserResponse.Data.PasswordSalt);
            
            var httpContent = new FormUrlEncodedContent(content);
            var result = await Fixture.Client.PostAsync<IDictionary<string, string[]>>(Api.TokenForm, httpContent);
            
            result.Response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            result.Data.Should().HaveCount(1);
            result.Data.ValidateMessage("userName");
        }
        [Fact]
        public async Task GetFormTokenWithoutPasswordReturnsBadRequest()
        {
            var content = new Dictionary<string, string>();
            content.Add("userName", Fixture.Faker.Internet.UserName());
            
            var httpContent = new FormUrlEncodedContent(content);
            var result = await Fixture.Client.PostAsync<IDictionary<string, string[]>>(Api.TokenForm, httpContent);

            result.Response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            result.Data.Should().HaveCount(1);
            result.Data.ValidateMessage("password");
        }
        [Fact]
        public async Task GetFormTokenWithEmptyModelReturnsBadRequest()
        {
            var content = new Dictionary<string, string>();
            
            var httpContent = new FormUrlEncodedContent(content);
            var result = await Fixture.Client.PostAsync<IDictionary<string, string[]>>(Api.TokenForm, httpContent);

            result.Response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            result.Data.Should().HaveCount(2);
            result.Data.ValidateMessage("password");
            result.Data.ValidateMessage("userName");
        }
        [Fact]
        public async Task SinginForExistingAccountReturnsOk()
        {      
            var data = new { 
                userName = Fixture.CreatedUserResponse.Data.UserName,
                password = Fixture.CreatedUserResponse.Data.PasswordSalt 
            };
            var result = await Fixture.Client.PostJsonAsync<object>(Api.Signin, data);

            result.Response.EnsureSuccessStatusCode();
            result.Response.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Response.Headers.GetCookie(Fixture.AuthCookieName).Should().NotBeNullOrWhiteSpace();
        }
        [Fact]
        public async Task SinginForNotExistingAccountReturnsUnauthorized()
        {
            var credentials = new {
                userName = Fixture.Faker.Internet.UserName(),
                password = Fixture.Faker.Internet.Password()
            };
            var result = await Fixture.Client.PostJsonAsync<object>(Api.Signin, credentials);

            result.Response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            result.Response.Headers.GetCookie(Fixture.AuthCookieName).Should().BeNullOrWhiteSpace();
        }
        [Fact]
        public async Task SinginWithoutUserNameReturnsBadRequest()
        {
            var credentials = new {
                password = Fixture.Faker.Internet.Password()
            };
            var result = await Fixture.Client.PostJsonAsync<IDictionary<string, string[]>>(Api.Signin, credentials);

            result.Response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            result.Data.Should().HaveCount(1);
            result.Data.ValidateMessage("userName");
            result.Response.Headers.GetCookie(Fixture.AuthCookieName).Should().BeNullOrWhiteSpace();
        }
        [Fact]
        public async Task SinginWithoutPasswordReturnsBadRequest()
        {
            var credentials = new {
                userName = Fixture.Faker.Internet.UserName()
            };
            var result = await Fixture.Client.PostJsonAsync<IDictionary<string, string[]>>(Api.Signin, credentials);

            result.Response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            result.Data.Should().HaveCount(1);
            result.Data.ValidateMessage("password");
            result.Response.Headers.GetCookie(Fixture.AuthCookieName).Should().BeNullOrWhiteSpace();
        }
        [Fact]
        public async Task SinginWithEmptyModelReturnsBadRequest()
        {
            var result = await Fixture.Client.PostJsonAsync<IDictionary<string, string[]>>(Api.Signin, new {});
            result.Response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            result.Data.Should().HaveCount(2);
            result.Data.ValidateMessage("password");
            result.Data.ValidateMessage("userName");
            result.Response.Headers.GetCookie(Fixture.AuthCookieName).Should().BeNullOrWhiteSpace();
        }
        [Fact]
        public async Task SinginWithoutModelReturnsBadRequest()
        {
            var result = await Fixture.Client.PostJsonAsync<IDictionary<string, string[]>>(Api.Signin, null);

            result.Response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            result.Data.Should().HaveCount(1);
            result.Data.First().Value.Should().HaveCount(1);
            result.Data.First().Value.First().Should().NotBeNullOrWhiteSpace();
        }
        [Fact]
        public async Task SingoutReturnsOk()
        {
            var response = await Fixture.Client.PostAsync(Api.Signout, null);

            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            //ToDo: check if cookies is deleted
            response.Headers.GetCookie(Fixture.AuthCookieName).Should().BeNullOrWhiteSpace();
        }
    }
}