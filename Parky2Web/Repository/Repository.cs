using Newtonsoft.Json;
using Parky2Web.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Parky2Web.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {

        private readonly IHttpClientFactory _clientFactory;

        public Repository(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }
        public async Task<bool> CreateAsync(T obj,string url)
        {
           
            if (obj == null)
            {
                return false;
            }
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Content = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");

            var client = _clientFactory.CreateClient();
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> DeleteAsync(int id, string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, url+id);
           

            var client = _clientFactory.CreateClient();

            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return true;
            }

            return false;
        }

        public async Task<List<T>> GetAllAsync(string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url );
            var client = _clientFactory.CreateClient();
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var jsonObject = await response.Content.ReadAsStringAsync();
                var listObj = JsonConvert.DeserializeObject<List<T>>(jsonObject);
                return listObj;
            }
            else
            {
                return null;
            }
        }

        public async Task<T> GetByIdAsync(int id, string url)
        {
           
            var request = new HttpRequestMessage(HttpMethod.Get, url+id);
            var client = _clientFactory.CreateClient();
            HttpResponseMessage response = await client.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var jsonObject = await response.Content.ReadAsStringAsync();
                var Obj = JsonConvert.DeserializeObject<T>(jsonObject);
                return Obj;
            }

            return null;
        }

        public async Task<bool> UpdateAsync(T obj, string url)
        {
            if (obj == null)
            {
                return false;
            }
            var request = new HttpRequestMessage(HttpMethod.Put, url);
       
      
            request.Content = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");

            var client = _clientFactory.CreateClient();
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return true;
            }

            return false;

        }
    }
}
