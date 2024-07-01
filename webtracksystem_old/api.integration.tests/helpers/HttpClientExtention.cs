using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Api.Integration.Tests.Extentions
{
    public class HttpResponseData<T> where T: class{
        public HttpResponseData(HttpResponseMessage response){
            Response = response;
            string json = Task.Run(()=>response.Content.ReadAsStringAsync()).Result;
            if(typeof(T).FullName == typeof(string).FullName){
                Data = json as T;
            }else if(!String.IsNullOrWhiteSpace(json)){
                Data = JsonConvert.DeserializeObject<T>(json);
            }
            Location = response.Headers.GetHeaderLocation();
        }
        public HttpResponseMessage Response { get; }
        public T Data { get; }
        public string Location { get; }
    }
    public static class HttpClientExtention{
        public static async Task<HttpResponseData<T>> GetAsync<T>(this HttpClient client, string url) where T: class
        {
            HttpResponseMessage response = await client.GetAsync(url);
            return new HttpResponseData<T>(response);
        }
        public static async Task<HttpResponseData<T>> PostAsync<T>(this HttpClient client, string url, HttpContent httpContent) where T: class
        {
            HttpResponseMessage response = await client.PostAsync(url, httpContent);
            return new HttpResponseData<T>(response);
        }
        public static async Task<HttpResponseData<T>> PutAsync<T>(this HttpClient client, string url, HttpContent httpContent) where T: class
        {
            HttpResponseMessage response = await client.PutAsync(url, httpContent);
            return new HttpResponseData<T>(response);
        }
        public static async Task<HttpResponseData<T>> PatchAsync<T>(this HttpClient client, string url, HttpContent httpContent) where T: class
        {
            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("PATCH"), url);
            request.Content = httpContent;
            HttpResponseMessage response = await client.SendAsync(request);
            return new HttpResponseData<T>(response);
        }
        public static async Task<HttpResponseData<T>> PostJsonAsync<T>(this HttpClient client, string url, Object data) where T: class
        {
            var httpContent = new StringContent(JsonConvert.SerializeObject(data).ToString(), Encoding.UTF8, "application/json");
            return await PostAsync<T>(client, url, httpContent);
        }
        public static async Task<HttpResponseData<T>> PutJsonAsync<T>(this HttpClient client, string url, Object data) where T: class
        {
            var httpContent = new StringContent(JsonConvert.SerializeObject(data).ToString(), Encoding.UTF8, "application/json");
            return await PutAsync<T>(client, url, httpContent);
        }
        public static async Task<HttpResponseData<T>> PatchJsonAsync<T>(this HttpClient client, string url, Object data) where T: class
        {
            var httpContent = new StringContent(JsonConvert.SerializeObject(data).ToString(), Encoding.UTF8, "application/json");
            return await PatchAsync<T>(client, url, httpContent);
        }
    }
}