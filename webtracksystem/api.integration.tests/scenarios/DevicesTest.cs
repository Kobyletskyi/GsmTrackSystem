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
    public class NotExistingDeviceTests {
        private readonly ExistingUserFixture fixture;

        public NotExistingDeviceTests(ExistingUserFixture fixture)
        {
            this.fixture = fixture;
            var res = Task.Run(()=>this.fixture.AuthorizeUserAsync(Authorization.Cookie)).Result;
        }

        [Fact]
        public async Task PostDeviceReturnsCreatedWithLocation()
        {
            DeviceForCreation device = fixture.GenerateNewDevice();
            var result = await fixture.Client.PostJsonAsync<Device>(Api.Devices, device);

            result.Response.EnsureSuccessStatusCode();
            result.Response.StatusCode.Should().Be(HttpStatusCode.Created);

            result.Data.Should().NotBeNull();
            result.Data.Id.Should().BeGreaterThan(0);
            result.Data.OwnerId.Should().Be(fixture.CreatedUserResponse.Data.Id);
            result.Data.IMEI.Should().Be(device.IMEI);
            result.Data.Title.Should().Be(device.Title);
            result.Data.Description.Should().Be(device.Description);
            //result.Data.SoftwareVersion.Should().Be(device.SoftwareVersion);

            result.Location.Should().NotBeNullOrWhiteSpace();
            
            fixture.Client.DeleteAsync(result.Location);
        }
        [Fact]
        public async Task PostDeviceWithoutModelReturnsBadRequest()
        {
            var result = await fixture.Client.PostJsonAsync<IDictionary<string, string[]>>(Api.Devices, null);

            result.Response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            result.Data.Should().HaveCount(1);
            result.Data.First().Value.Should().HaveCount(1);
            result.Data.First().Value.First().Should().NotBeNullOrWhiteSpace();
        }
        [Fact]
        public async Task PostDeviceWithEmptyModelReturnsBadRequest()
        {
            var result = await fixture.Client.PostJsonAsync<IDictionary<string, string[]>>(Api.Devices, new {});

            result.Response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            result.Data.Should().HaveCount(5);
            result.Data.ValidateMessage("ownerId");
            result.Data.ValidateMessage("imei");
            result.Data.ValidateMessage("title");
            result.Data.ValidateMessage("description");
            result.Data.ValidateMessage("softwareVersion");
        }
        [Fact]
        public async Task PostDeviceWithoutImeiReturnsBadRequest()
        {
            DeviceForCreation device = fixture.GenerateNewDevice();
            device.IMEI = null;
            var result = await fixture.Client.PostJsonAsync<IDictionary<string, string[]>>(Api.Devices, device);

            result.Response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            result.Data.Should().HaveCount(1);
            result.Data.ValidateMessage("imei");
        }
        [Fact]
        public async Task PostDeviceWithoutImeiAndTitleReturnsBadRequest()
        {
            DeviceForCreation device = fixture.GenerateNewDevice();
            device.IMEI = null;
            device.Title = "";
            var result = await fixture.Client.PostJsonAsync<IDictionary<string, string[]>>(Api.Devices, device);

            result.Response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            result.Data.Should().HaveCount(2);
            result.Data.ValidateMessage("imei");
            result.Data.ValidateMessage("title");
        }
        [Fact]
        public async Task PostDeviceWithInvalidImeiReturnsBadRequest()
        {
            DeviceForCreation device = fixture.GenerateNewDevice();
            device.IMEI = "123456789012345678901234567890";
            var result = await fixture.Client.PostJsonAsync<IDictionary<string, string[]>>(Api.Devices, device);

            result.Response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            result.Data.Should().HaveCount(1);
            result.Data.ValidateMessage("imei");
        }
        [Fact]
        public async Task PatchNotExistingDeviceReturnsCreatedWithLocation()
        {
            int newDeviceId = 0;
            DeviceForCreation device = fixture.GenerateNewDevice();
            var patchDoc = new List<object>(){
                new {
                    op = "add",
                    path = "/imei",
                    value = device.IMEI
                },
                new {
                    op = "add",
                    path = "/title",
                    value = device.Title
                },
                new {
                    op = "replace",
                    path = "/ownerId",
                    value = device.OwnerId
                },
                new {
                    op = "replace",
                    path = "/description",
                    value = device.Description
                },
                new {
                    op = "replace",
                    path = "/SoftwareVersion",
                    value = device.SoftwareVersion
                }
            };
            var result = await fixture.Client.PatchJsonAsync<Device>(Api.Devices + "/" + newDeviceId, patchDoc);

            result.Response.EnsureSuccessStatusCode();
            result.Response.StatusCode.Should().Be(HttpStatusCode.Created);

            result.Data.Should().NotBeNull();
            result.Data.Id.Should().BeGreaterThan(0);
            //result.Data.Id.Should().Be(newDeviceId);
            result.Data.OwnerId.Should().Be(fixture.CreatedUserResponse.Data.Id);
            result.Data.IMEI.Should().Be(device.IMEI);
            result.Data.Title.Should().Be(device.Title);
            result.Data.Description.Should().Be(device.Description);
            result.Data.SoftwareVersion.Should().Be(device.SoftwareVersion);

            result.Location.Should().NotBeNullOrWhiteSpace();
            
            await fixture.Client.DeleteAsync(result.Location);
        }
        [Fact]
        public async Task PatchNotExistingDeviceWithEmptyModelReturnsBadRequest()
        {
            int newDeviceId = 0;
            DeviceForCreation device = fixture.GenerateNewDevice();
            var patchDoc = new List<object>(){ };
            var result = await fixture.Client.PatchJsonAsync<IDictionary<string, string[]>>(Api.Devices + "/" + newDeviceId, patchDoc);

            result.Response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            result.Data.Should().HaveCount(5);
            result.Data.ValidateMessage("ownerId");
            result.Data.ValidateMessage("imei");
            result.Data.ValidateMessage("title");
            result.Data.ValidateMessage("description");
            result.Data.ValidateMessage("softwareVersion");
        }
        [Fact]
        public async Task PatchNotExistingDeviceAddInvalidPropertyReturnsBadRequest()
        {
            int newDeviceId = 0;
            DeviceForCreation device = fixture.GenerateNewDevice();
            var patchDoc = new List<object>(){
                new {
                    op = "add",
                    path = "/invalid_property",
                    value = device.IMEI
                },
                new {
                    op = "add",
                    path = "/title",
                    value = device.Title
                },
                new {
                    op = "replace",
                    path = "/ownerId",
                    value = device.OwnerId
                },
                new {
                    op = "replace",
                    path = "/description",
                    value = device.Description
                },
                new {
                    op = "replace",
                    path = "/SoftwareVersion",
                    value = device.SoftwareVersion
                }
            };
            var result = await fixture.Client.PatchJsonAsync<IDictionary<string, string[]>>(Api.Devices + "/" + newDeviceId, patchDoc);

            result.Response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            result.Data.Should().HaveCount(2);
            result.Data.ValidateMessage("imei");
        }
        [Fact]
        public async Task PatchNotExistingDeviceWithoutModelReturnsBadRequest()
        {
            int newDeviceId = 0;
            
            var result = await fixture.Client.PatchJsonAsync<IDictionary<string, string[]>>(Api.Devices + "/" + newDeviceId, null);

            result.Response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            // result.Data.Should().HaveCount(1);
            // result.Data.First().Value.Should().HaveCount(1);
            // result.Data.First().Value.First().Should().NotBeNullOrWhiteSpace();
        }
        [Fact]
        public async Task PatchNotExistingDeviceWithoutImeiReturnsBadRequest()
        {
            int newDeviceId = 0;
            DeviceForCreation device = fixture.GenerateNewDevice();
            var patchDoc = new List<object>(){
                new {
                    op = "add",
                    path = "/title",
                    value = device.Title
                },
                new {
                    op = "add",
                    path = "/ownerId",
                    value = device.OwnerId
                },
                new {
                    op = "add",
                    path = "/description",
                    value = device.Description
                },
                new {
                    op = "add",
                    path = "/SoftwareVersion",
                    value = device.SoftwareVersion
                }
            };
            var result = await fixture.Client.PatchJsonAsync<IDictionary<string, string[]>>(Api.Devices + "/" + newDeviceId, patchDoc);

            result.Response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            result.Data.Should().HaveCount(1);
            result.Data.ValidateMessage("imei");
        }
        [Fact]
        public async Task PatchNotExistingDeviceWithImeiAndTitleReturnsBadRequest()
        {
            int newDeviceId = 0;
            DeviceForCreation device = fixture.GenerateNewDevice();
            var patchDoc = new List<object>(){
                new {
                    op = "add",
                    path = "/ownerId",
                    value = device.OwnerId
                },
                new {
                    op = "add",
                    path = "/description",
                    value = device.Description
                },
                new {
                    op = "add",
                    path = "/SoftwareVersion",
                    value = device.SoftwareVersion
                }
            };
            var result = await fixture.Client.PatchJsonAsync<IDictionary<string, string[]>>(Api.Devices + "/" + newDeviceId, patchDoc);

            result.Response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            result.Data.Should().HaveCount(2);
            result.Data.ValidateMessage("imei");
            result.Data.ValidateMessage("title");
        }
        [Fact]
        public async Task DeleteDeviceReturnsOk()
        {
            DeviceForCreation device = fixture.GenerateNewDevice();
            var postResult = await fixture.Client.PostJsonAsync<Device>(Api.Devices, device);
            var response = await fixture.Client.DeleteAsync(postResult.Location);

            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            HttpResponseData<Device> getResult = await fixture.Client.GetAsync<Device>(postResult.Location);

            getResult.Response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            getResult.Data.Should().BeNull();
        }
    }
    [Collection(nameof(ExistingDeviceCollection))]
    public class ExistingDeviceTests {
        private readonly ExistingDeviceFixture fixture;

        public ExistingDeviceTests(ExistingDeviceFixture fixture)
        {
            this.fixture = fixture;
            var res = Task.Run(()=>this.fixture.AuthorizeUserAsync(Authorization.Cookie)).Result;
        }
        [Fact]
        public async Task GetExistingDeviceReturnsOk()
        {
            HttpResponseData<Device> result = await fixture.Client.GetAsync<Device>(fixture.CreatedDeviceResponse.Location);

            result.Response.EnsureSuccessStatusCode();
            result.Response.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Data.Should().NotBeNull();
            result.Data.Id.Should().Be(fixture.CreatedDeviceResponse.Data.Id);
            result.Data.OwnerId.Should().Be(fixture.CreatedDeviceResponse.Data.OwnerId);
            result.Data.Title.Should().Be(fixture.CreatedDeviceResponse.Data.Title);
            result.Data.Description.Should().Be(fixture.CreatedDeviceResponse.Data.Description);
            result.Data.SoftwareVersion.Should().Be(fixture.CreatedDeviceResponse.Data.SoftwareVersion);
        }
        [Fact]
        public async Task GetNotExistingDeviceReturnsNotFound()
        {
            HttpResponseData<Device> result = await fixture.Client.GetAsync<Device>(Api.Devices + "/0");
            
            result.Response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            result.Data.Should().BeNull();
        }
        [Fact]
        public async Task PutExistingDeviceReturnsNoContent()
        {
            string newTitle = fixture.CreatedDeviceResponse.Data.Title + "_updated";
            DeviceForUpdate device = new DeviceForUpdate {
                Title = newTitle,
                Description = fixture.CreatedDeviceResponse.Data.Description,
                SoftwareVersion = fixture.CreatedDeviceResponse.Data.SoftwareVersion
            };

            HttpResponseData<Device> result = await fixture.Client.PutJsonAsync<Device>(fixture.CreatedDeviceResponse.Location, device);

            result.Response.EnsureSuccessStatusCode();
            result.Response.StatusCode.Should().Be(HttpStatusCode.NoContent);
            result.Data.Should().BeNull();

            HttpResponseData<Device> getResult = await fixture.Client.GetAsync<Device>(fixture.CreatedDeviceResponse.Location);

            getResult.Response.EnsureSuccessStatusCode();
            getResult.Response.StatusCode.Should().Be(HttpStatusCode.OK);
            getResult.Data.Should().NotBeNull();
            getResult.Data.Id.Should().Be(fixture.CreatedDeviceResponse.Data.Id);
            getResult.Data.OwnerId.Should().Be(fixture.CreatedDeviceResponse.Data.OwnerId);
            getResult.Data.Title.Should().Be(newTitle);
            getResult.Data.Description.Should().Be(fixture.CreatedDeviceResponse.Data.Description);
            getResult.Data.SoftwareVersion.Should().Be(fixture.CreatedDeviceResponse.Data.SoftwareVersion);
            fixture.CreatedDeviceResponse.Data.Title = getResult.Data.Title;
        }
        [Fact]
        public async Task PutNotExistingDeviceReturnsNotFound()
        {
            string newTitle = fixture.CreatedDeviceResponse.Data.Title + "_updated";
            DeviceForUpdate device = new DeviceForUpdate {
                Title = newTitle,
                Description = fixture.CreatedDeviceResponse.Data.Description,
                SoftwareVersion = fixture.CreatedDeviceResponse.Data.SoftwareVersion
            };

            HttpResponseData<Device> result = await fixture.Client.PutJsonAsync<Device>(Api.Devices + "/0", device);

            result.Response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            result.Data.Should().BeNull();
        }
        [Fact]
        public async Task PutDeviceWithoutModelReturnsBadRequest()
        {
            var result = await fixture.Client.PutJsonAsync<IDictionary<string, string[]>>(
                fixture.CreatedDeviceResponse.Location, null);

            result.Response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            result.Data.Should().HaveCount(1);
            result.Data.First().Value.Should().HaveCount(1);
            result.Data.First().Value.First().Should().NotBeNullOrWhiteSpace();

            HttpResponseData<Device> getResult = await fixture.Client.GetAsync<Device>(fixture.CreatedDeviceResponse.Location);

            getResult.Response.EnsureSuccessStatusCode();
            getResult.Response.StatusCode.Should().Be(HttpStatusCode.OK);
            getResult.Data.Should().NotBeNull();
            getResult.Data.Id.Should().Be(fixture.CreatedDeviceResponse.Data.Id);
            getResult.Data.OwnerId.Should().Be(fixture.CreatedDeviceResponse.Data.OwnerId);
            getResult.Data.Title.Should().Be(fixture.CreatedDeviceResponse.Data.Title);
            getResult.Data.Description.Should().Be(fixture.CreatedDeviceResponse.Data.Description);
            getResult.Data.SoftwareVersion.Should().Be(fixture.CreatedDeviceResponse.Data.SoftwareVersion);
        }
        [Fact]
        public async Task PutDeviceWithEmptyModelReturnsBadRequest()
        {
            var result = await fixture.Client.PutJsonAsync<IDictionary<string, string[]>>(
                fixture.CreatedDeviceResponse.Location, new {});

            result.Response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            result.Data.Should().HaveCount(3);
            result.Data.ValidateMessage("title");
            result.Data.ValidateMessage("description");
            result.Data.ValidateMessage("softwareVersion");

            HttpResponseData<Device> getResult = await fixture.Client.GetAsync<Device>(fixture.CreatedDeviceResponse.Location);

            getResult.Response.EnsureSuccessStatusCode();
            getResult.Response.StatusCode.Should().Be(HttpStatusCode.OK);
            getResult.Data.Should().NotBeNull();
            getResult.Data.Id.Should().Be(fixture.CreatedDeviceResponse.Data.Id);
            getResult.Data.OwnerId.Should().Be(fixture.CreatedDeviceResponse.Data.OwnerId);
            getResult.Data.Title.Should().Be(fixture.CreatedDeviceResponse.Data.Title);
            getResult.Data.Description.Should().Be(fixture.CreatedDeviceResponse.Data.Description);
            getResult.Data.SoftwareVersion.Should().Be(fixture.CreatedDeviceResponse.Data.SoftwareVersion);
        }
        [Fact]
        public async Task PutDeviceWithoutTitleReturnsBadRequest()
        {
            DeviceForCreation device = fixture.GenerateNewDevice();
            device.Title = null;
            var result = await fixture.Client.PutJsonAsync<IDictionary<string, string[]>>(
                fixture.CreatedDeviceResponse.Location, device);

            result.Response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            result.Data.Should().HaveCount(1);
            result.Data.ValidateMessage("title");

            HttpResponseData<Device> getResult = await fixture.Client.GetAsync<Device>(fixture.CreatedDeviceResponse.Location);

            getResult.Response.EnsureSuccessStatusCode();
            getResult.Response.StatusCode.Should().Be(HttpStatusCode.OK);
            getResult.Data.Should().NotBeNull();
            getResult.Data.Id.Should().Be(fixture.CreatedDeviceResponse.Data.Id);
            getResult.Data.OwnerId.Should().Be(fixture.CreatedDeviceResponse.Data.OwnerId);
            getResult.Data.Title.Should().Be(fixture.CreatedDeviceResponse.Data.Title);
            getResult.Data.Description.Should().Be(fixture.CreatedDeviceResponse.Data.Description);
            getResult.Data.SoftwareVersion.Should().Be(fixture.CreatedDeviceResponse.Data.SoftwareVersion);
        }
        [Fact]
        public async Task PutDeviceWithoutDescriptionAndTitleReturnsBadRequest()
        {
            DeviceForCreation device = fixture.GenerateNewDevice();
            device.Description = null;
            device.Title = "";
            var result = await fixture.Client.PutJsonAsync<IDictionary<string, string[]>>(
                fixture.CreatedDeviceResponse.Location, device);

            result.Response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            result.Data.Should().HaveCount(2);
            result.Data.ValidateMessage("description");
            result.Data.ValidateMessage("title");

            HttpResponseData<Device> getResult = await fixture.Client.GetAsync<Device>(fixture.CreatedDeviceResponse.Location);

            getResult.Response.EnsureSuccessStatusCode();
            getResult.Response.StatusCode.Should().Be(HttpStatusCode.OK);
            getResult.Data.Should().NotBeNull();
            getResult.Data.Id.Should().Be(fixture.CreatedDeviceResponse.Data.Id);
            getResult.Data.OwnerId.Should().Be(fixture.CreatedDeviceResponse.Data.OwnerId);
            getResult.Data.Title.Should().Be(fixture.CreatedDeviceResponse.Data.Title);
            getResult.Data.Description.Should().Be(fixture.CreatedDeviceResponse.Data.Description);
            getResult.Data.SoftwareVersion.Should().Be(fixture.CreatedDeviceResponse.Data.SoftwareVersion);
        }
        [Fact]
        public async Task PatchExistingDeviceFullReplaceReturnsNoContent()
        {
            var newVersion = "1234567";
            var patchDoc = new List<object>(){
                new {
                    op = "copy",
                    path = "/description",
                    from = "/title"
                },
                new {
                    op = "move",
                    from = "/SoftwareVersion",
                    path = "/title"
                },
                new {
                    op = "replace",
                    path = "/SoftwareVersion",
                    value = newVersion
                }
            };
            var result = await fixture.Client.PatchJsonAsync<Device>(
                fixture.CreatedDeviceResponse.Location, patchDoc);

            result.Response.EnsureSuccessStatusCode();
            result.Response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            result.Data.Should().BeNull();

            HttpResponseData<Device> getResult = await fixture.Client.GetAsync<Device>(fixture.CreatedDeviceResponse.Location);

            getResult.Response.EnsureSuccessStatusCode();
            getResult.Response.StatusCode.Should().Be(HttpStatusCode.OK);
            getResult.Data.Should().NotBeNull();
            getResult.Data.Id.Should().Be(fixture.CreatedDeviceResponse.Data.Id);
            getResult.Data.OwnerId.Should().Be(fixture.CreatedDeviceResponse.Data.OwnerId);
            getResult.Data.Title.Should().Be(fixture.CreatedDeviceResponse.Data.SoftwareVersion);
            getResult.Data.Description.Should().Be(fixture.CreatedDeviceResponse.Data.Title);
            getResult.Data.SoftwareVersion.Should().Be(newVersion);
            fixture.CreatedDeviceResponse.Data.Title = getResult.Data.Title;
            fixture.CreatedDeviceResponse.Data.Description = getResult.Data.Description;
            fixture.CreatedDeviceResponse.Data.SoftwareVersion = getResult.Data.SoftwareVersion;
        }
        [Fact]
        public async Task PatchExistingDeviceReplacePartialReturnsNoContent()
        {
            var newTitle = fixture.CreatedDeviceResponse.Data.Title + "_patched";
            var patchDoc = new List<object>(){
                new {
                    op = "replace",
                    path = "/title",
                    value = newTitle
                }
            };
            var result = await fixture.Client.PatchJsonAsync<Device>(
                fixture.CreatedDeviceResponse.Location, patchDoc);

            result.Response.EnsureSuccessStatusCode();
            result.Response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            result.Data.Should().BeNull();

            HttpResponseData<Device> getResult = await fixture.Client.GetAsync<Device>(fixture.CreatedDeviceResponse.Location);

            getResult.Response.EnsureSuccessStatusCode();
            getResult.Response.StatusCode.Should().Be(HttpStatusCode.OK);
            getResult.Data.Should().NotBeNull();
            getResult.Data.Id.Should().Be(fixture.CreatedDeviceResponse.Data.Id);
            getResult.Data.OwnerId.Should().Be(fixture.CreatedDeviceResponse.Data.OwnerId);
            getResult.Data.Title.Should().Be(newTitle);
            getResult.Data.Description.Should().Be(fixture.CreatedDeviceResponse.Data.Description);
            getResult.Data.SoftwareVersion.Should().Be(fixture.CreatedDeviceResponse.Data.SoftwareVersion);
            fixture.CreatedDeviceResponse.Data.Title = getResult.Data.Title;
        }
        [Fact]
        public async Task PatchExistingDeviceRemoveRequiredPropertyReturnsBadRequest()
        {
            var patchDoc = new List<object>(){
                new {
                    op = "remove",
                    path = "/title"
                }
            };
            var result = await fixture.Client.PatchJsonAsync<IDictionary<string, string[]>>(
                fixture.CreatedDeviceResponse.Location, patchDoc);

            result.Response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            result.Data.Should().HaveCount(1);
            result.Data.ValidateMessage("title");

            HttpResponseData<Device> getResult = await fixture.Client.GetAsync<Device>(fixture.CreatedDeviceResponse.Location);

            getResult.Response.EnsureSuccessStatusCode();
            getResult.Response.StatusCode.Should().Be(HttpStatusCode.OK);
            getResult.Data.Should().NotBeNull();
            getResult.Data.Id.Should().Be(fixture.CreatedDeviceResponse.Data.Id);
            getResult.Data.OwnerId.Should().Be(fixture.CreatedDeviceResponse.Data.OwnerId);
            getResult.Data.Title.Should().Be(fixture.CreatedDeviceResponse.Data.Title);
            getResult.Data.Description.Should().Be(fixture.CreatedDeviceResponse.Data.Description);
            getResult.Data.SoftwareVersion.Should().Be(fixture.CreatedDeviceResponse.Data.SoftwareVersion);
        }
        [Fact]
        public async Task PatchExistingDeviceReplaceInvalidPropertyReturnsBadRequest()
        {
            var patchDoc = new List<object>(){
                new {
                    op = "replace",
                    path = "/invalid_property",
                    value = "patched"
                }
            };
            var result = await fixture.Client.PatchJsonAsync<IDictionary<string, string[]>>(
                fixture.CreatedDeviceResponse.Location, patchDoc);
            result.Response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            result.Data.Should().HaveCount(1);
            result.Data.First().Value.Should().HaveCount(1);
            result.Data.First().Value.First().Should().NotBeNullOrWhiteSpace();
        }
    }
}