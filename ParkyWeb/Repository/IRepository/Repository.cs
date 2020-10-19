using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ParkyWeb.Repository.IRepository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public Repository(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<bool> CreateAsync(string url, T CreateObject)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);

            if (CreateObject == null)
            {
                return false;
            }

            request.Content = new StringContent(JsonConvert.SerializeObject(CreateObject), Encoding.UTF8, "application/json");
            var client = _httpClientFactory.CreateClient();
            var respons =await client.SendAsync(request);

            if (respons.StatusCode ==System.Net.HttpStatusCode.Created)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(string url, int id)
        {
            var requsest = new HttpRequestMessage(HttpMethod.Delete, url+id);

            var client = _httpClientFactory.CreateClient();
            HttpResponseMessage response = await client.SendAsync(requsest);

            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public async Task<IList<T>> GetAllAsync(string url)
        {
            var request = new  HttpRequestMessage(HttpMethod.Get, url);

            var client = _httpClientFactory.CreateClient();
            HttpResponseMessage response = await client.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var listJsonObject = await response.Content.ReadAsStringAsync();
                var ListObject = JsonConvert.DeserializeObject<List<T>>(listJsonObject);
                return ListObject;
            }
            return null;
        }

        public async Task<T> GetAsync(string url, int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url+id);

            var client = _httpClientFactory.CreateClient();
            HttpResponseMessage responseMessage = await client.SendAsync(request);

            if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
            {

                var JsonObject = await responseMessage.Content.ReadAsStringAsync();
                var Object = JsonConvert.DeserializeObject<T>(JsonObject);
                return Object;
            }

            return null;
        }

        public async Task<bool> UpdateAsync(string url, T CreateObject)
        {

            var requset = new HttpRequestMessage(HttpMethod.Put, url);
            if (CreateObject == null)
            {
                return false; 
            }

            requset.Content = new StringContent(JsonConvert.SerializeObject(CreateObject), Encoding.UTF8, "application/json");
            var client = _httpClientFactory.CreateClient();
            HttpResponseMessage responseMessage = await client.SendAsync(requset);
            if (responseMessage.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return true;
            }
            else
            {
                return false;
            }
            
          
        }
    }
}
