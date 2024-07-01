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
using System.Text;
using Api.Models;
using System.Threading.Tasks;

namespace Api.Integration.Tests
{
    public class ExistingDeviceFixture : ExistingUserFixture, IDisposable
    {
        public HttpResponseData<Device> CreatedDeviceResponse { get; }
        public Device Device {
            get{
                return CreatedDeviceResponse != null ? CreatedDeviceResponse.Data: null;
            }
        }
        public DeviceIdenty DeviceIdenty {
            get {
                return new DeviceIdenty(){
                    IMEI = CreatedDeviceResponse.Data.IMEI
                };
            }
        }
        public ExistingDeviceFixture(): base()
        {
            var res = Task.Run(()=>AuthorizeUserAsync(Authorization.Cookie)).Result;
            DeviceForCreation device = GenerateNewDevice();
            CreatedDeviceResponse = Task.Run(()=>Client.PostJsonAsync<Device>(Api.Devices, device)).Result;
            ClearAuthorization(Authorization.Cookie);
        }

        public async Task<bool> AuthorizeDevice() {
            var codePostResult = await Client.PostJsonAsync<DeviceCode>(Api.DeviceCode, DeviceIdenty.IMEI);
            var deviceAuth = new DeviceAuthCode(){
                IMEI = DeviceIdenty.IMEI,
                Code = codePostResult.Data.Code
            };
            HttpResponseData<string> rfTokenResult = await Client.PostJsonAsync<string>(Api.DeviceToken, deviceAuth);

            rfTokenResult.Response.EnsureSuccessStatusCode();
            rfTokenResult.Response.StatusCode.Should().Be(HttpStatusCode.OK);
            rfTokenResult.Data.Should().NotBeNullOrEmpty();

            var deviceToken = new DeviceToken() {
                IMEI = DeviceIdenty.IMEI,
                RefreshToken = rfTokenResult.Data
            };
            HttpResponseData<string> tokenResult = await Client.PostJsonAsync<string>(Api.DeviceRefreshToken, deviceToken);

            tokenResult.Response.EnsureSuccessStatusCode();
            tokenResult.Response.StatusCode.Should().Be(HttpStatusCode.OK);
            tokenResult.Data.Should().NotBeNullOrEmpty();

            ClearAuthorization(Authorization.Bearer);
            Client.DefaultRequestHeaders.Add(HeaderNames.Authorization, String.Format("Bearer {0}", tokenResult.Data));
            return true;
        }
    }
}