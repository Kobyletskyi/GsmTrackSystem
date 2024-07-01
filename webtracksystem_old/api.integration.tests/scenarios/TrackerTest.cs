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
    [Collection(nameof(ExistingUserCollection))]
    public class NotExistingTrackerTests {
        private readonly ExistingUserFixture fixture;

        public NotExistingTrackerTests(ExistingUserFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public async Task PostCodeForNotExistingDevice()
        {
            var result = await fixture.Client.PostJsonAsync<DeviceCode>(Api.DeviceCode, "IMEI");
            result.Response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            result.Data.Should().BeNull();
        }
        [Fact]
        public async Task GetTokenForNotExistingDevice()
        {
            var deviceAuth = new DeviceAuthCode(){
                IMEI = "IMEI",
                Code = 1111
            };
            HttpResponseData<string> tokenResult = await fixture.Client.PostJsonAsync<string>(Api.DeviceToken, deviceAuth);

            tokenResult.Response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            tokenResult.Data.Should().BeNullOrEmpty();
        }
        [Fact]
        public async Task RefreshTokenForNotExistingDevice()
        {
            var deviceToken = new DeviceToken(){
                IMEI = "IMEI",
                RefreshToken = "refreshtoken"
            };
            HttpResponseData<string> tokenResult = await fixture.Client.PostJsonAsync<string>(Api.DeviceRefreshToken, deviceToken);

            tokenResult.Response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            tokenResult.Data.Should().BeNullOrEmpty();
        }
    }
    [Collection(nameof(ExistingDeviceCollection))]
    public class ExistingTrackerUnauthorizedTests {
        private readonly ExistingDeviceFixture fixture;

        public ExistingTrackerUnauthorizedTests(ExistingDeviceFixture fixture)
        {
            this.fixture = fixture;
        }
        [Fact]
        public async Task SuccessfullyPostCode()
        {
            var result = await fixture.Client.PostJsonAsync<DeviceCode>(Api.DeviceCode, fixture.DeviceIdenty.IMEI);

            result.Response.EnsureSuccessStatusCode();
            result.Response.StatusCode.Should().Be(HttpStatusCode.Created);

            result.Data.Should().NotBeNull();
            result.Data.Id.Should().BeGreaterThan(0);
            result.Data.IMEI.Should().Be(fixture.CreatedDeviceResponse.Data.IMEI);
            result.Data.Code.Should().BeGreaterThan(0);
            result.Data.Expiration.Should().BeBefore(DateTime.UtcNow.AddHours(24));
            //result.Data.Title.Should().Be(Fixture.CreatedDeviceResponse.Data.Title);
            //result.Data.OwnerId.Should().Be(Fixture.CreatedDeviceResponse.Data.OwnerId);

            result.Location.Should().NotBeNullOrWhiteSpace();

            await fixture.Client.DeleteAsync(result.Location);
        }
        [Fact]
        public async Task SuccessfullyGetToken()
        {
            var identy = fixture.DeviceIdenty;
            var codePostResult = await fixture.Client.PostJsonAsync<DeviceCode>(Api.DeviceCode, identy.IMEI);
            var deviceAuth = new DeviceAuthCode(){
                IMEI = identy.IMEI,
                Code = codePostResult.Data.Code
            };
            HttpResponseData<string> tokenResult = await fixture.Client.PostJsonAsync<string>(Api.DeviceToken, deviceAuth);

            tokenResult.Response.EnsureSuccessStatusCode();
            tokenResult.Response.StatusCode.Should().Be(HttpStatusCode.OK);
            tokenResult.Data.Should().NotBeNullOrEmpty();
            await fixture.AuthorizeUserAsync(Authorization.Cookie);
            HttpResponseData<DeviceCode> codeGetResult = await fixture.Client.GetAsync<DeviceCode>(codePostResult.Location);
            codeGetResult.Response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            codeGetResult.Data.Should().BeNull();
            fixture.ClearAuthorization(Authorization.Cookie);
        }
        [Fact]
        public async Task GetTokenWithInvalidCode()
        {
            var identy = fixture.DeviceIdenty;
            var codePostResult = await fixture.Client.PostJsonAsync<DeviceCode>(Api.DeviceCode, identy.IMEI);
            var deviceAuth = new DeviceAuthCode(){
                IMEI = identy.IMEI,
                Code = codePostResult.Data.Code - 1
            };
            HttpResponseData<string> tokenResult = await fixture.Client.PostJsonAsync<string>(Api.DeviceToken, deviceAuth);

            tokenResult.Response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            tokenResult.Data.Should().BeNullOrWhiteSpace();

            await fixture.AuthorizeUserAsync(Authorization.Cookie);
            var codeGetResult = await fixture.Client.GetAsync<DeviceCode>(codePostResult.Location);
            codeGetResult.Response.EnsureSuccessStatusCode();
            codeGetResult.Response.StatusCode.Should().Be(HttpStatusCode.OK);
            codeGetResult.Data.Should().NotBeNull();
            codeGetResult.Data.Id.Should().Be(codePostResult.Data.Id);
            codeGetResult.Data.IMEI.Should().Be(codePostResult.Data.IMEI);
            //codeGetResult.Data.OwnerId.Should().Be(codePostResult.Data.OwnerId);
            //codeGetResult.Data.Title.Should().Be(codePostResult.Data.Title);
            codeGetResult.Data.Code.Should().Be(codePostResult.Data.Code);
            codeGetResult.Data.Expiration.Should().Be(codePostResult.Data.Expiration);

            //Fixture.CleanupResources.Enqueue(codePostResult.Location);
            fixture.ClearAuthorization(Authorization.Cookie);
        }
    }
    [Collection(nameof(ExistingDeviceCollection))]
    public class ExistingTrackerRefreshTokenTests {
        private readonly ExistingDeviceFixture fixture;
        public ExistingTrackerRefreshTokenTests(ExistingDeviceFixture fixture)
        {
            this.fixture = fixture;
        }
        [Fact]
        public async Task SuccessfullyRefreshToken()
        {
            await fixture.AuthorizeDevice();
            fixture.ClearAuthorization(Authorization.Bearer);
        }
        [Fact]
        public async Task RefreshInvalidToken()
        {
            var identy = fixture.DeviceIdenty;
            var codePostResult = await fixture.Client.PostJsonAsync<DeviceCode>(Api.DeviceCode, identy.IMEI);
            var deviceAuth = new DeviceAuthCode(){
                IMEI = identy.IMEI,
                Code = codePostResult.Data.Code
            };
            HttpResponseData<string> rfTokenResult = await fixture.Client.PostJsonAsync<string>(Api.DeviceToken, deviceAuth);

            var deviceToken = new DeviceToken() {
                IMEI = identy.IMEI,
                RefreshToken = "rfTokenResult" 
            };
            HttpResponseData<string> tokenResult = await fixture.Client.PostJsonAsync<string>(Api.DeviceRefreshToken, deviceToken);

            tokenResult.Response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            tokenResult.Data.Should().BeNullOrEmpty();
        }
        [Fact]
        public async Task GetTimeWithoutToken()
        {
            //fixture.ClearAuthorization(Authorization.Bearer);
            var response = await fixture.Client.GetAsync(Api.TrackerTime);
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
        [Fact]
        public async Task GetGpsAssistanceWithoutToken()
        {
            //fixture.ClearAuthorization(Authorization.Bearer);
            var response = await fixture.Client.GetAsync(Api.TrackerAssistence);
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
        [Fact]
        public async Task PostLocationWithoutToken()
        {
            //fixture.ClearAuthorization(Authorization.Bearer);
            string trackDate = "2016/3/11 21:33:11";
            string imei = "111107000000015";
            string data = String.Format("{0},{1},1,193416.00,0000.00000,N,00000.00000,E,0.000,NF,5303302,3750001,0.000,0.00,0.000,,99.99,99.99,99.99,0,0,0*20",
            imei, trackDate);
            var result = await fixture.Client.PostJsonAsync<GpsLocation>(Api.TrackerLocation, data);
            result.Response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            result.Data.Should().BeNull();
        }
    }
    [Collection(nameof(ExistingDeviceCollection))]
    public class ExistingTrackerAuthorizedTests {
        private readonly ExistingDeviceFixture fixture;
        public ExistingTrackerAuthorizedTests(ExistingDeviceFixture fixture)
        {
            this.fixture = fixture;
            var res = Task.Run(()=>this.fixture.AuthorizeDevice()).Result;
        }
        [Fact]
        public async Task SuccessfullyPostFirstLocation()
        {
            string trackDate = "2016/3/11 21:33:11";
            string imei = fixture.CreatedDeviceResponse.Data.IMEI;
            string data = String.Format("{0},{1},1,193416.00,0000.00000,N,00000.00000,E,0.000,NF,5303302,3750001,0.000,0.00,0.000,,99.99,99.99,99.99,0,0,0*20",
            imei, trackDate);
            var result = await fixture.Client.PostJsonAsync<GpsLocation>(Api.TrackerLocation, data);

            result.Response.EnsureSuccessStatusCode();
            result.Response.StatusCode.Should().Be(HttpStatusCode.Created);

            result.Data.Should().BeNull();

            result.Location.Should().NotBeNullOrWhiteSpace();

            ///var trackGetResult = await Fixture.Client.GetAsync<Track>(codePostResult.Location);
            //Fixture.CleanupResources.Enqueue(result.Location);
        }
        [Fact]
        public async Task SuccessfullyPostNextLocation()
        {
            string trackDate = "2016/3/11 21:33:11";
            string imei = fixture.CreatedDeviceResponse.Data.IMEI;
            string data = String.Format("{0},{1},1,193417.00,0000.00000,N,00000.00000,E,0.000,NF,5303302,3750001,0.000,0.00,0.000,,99.99,99.99,99.99,0,0,0*20",
            imei, trackDate);
            var result = await fixture.Client.PostJsonAsync<GpsLocation>(Api.TrackerLocation, data);

            result.Response.EnsureSuccessStatusCode();
            result.Response.StatusCode.Should().Be(HttpStatusCode.Created);

            result.Data.Should().BeNull();

            result.Location.Should().NotBeNullOrWhiteSpace();
            
            //Fixture.CleanupResources.Enqueue(result.Location);
        }
        [Fact]
        public async Task SuccessfullyGetTime()
        {
            var result = await fixture.Client.GetAsync<string>(Api.TrackerTime);
            result.Response.EnsureSuccessStatusCode();
            result.Response.StatusCode.Should().Be(HttpStatusCode.OK);

            result.Data.Should().NotBeNullOrWhiteSpace();
            //ToDo: parse response
        }
        [Fact]
        public async Task SuccessfullyGetGpsAssistance()
        {
            var response = await fixture.Client.GetAsync(Api.TrackerAssistence);
            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}