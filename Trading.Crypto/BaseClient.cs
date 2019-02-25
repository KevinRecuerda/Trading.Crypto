using System;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;

namespace Trading.Crypto
{
    public class ApiException : Exception
    {
        public ApiException(HttpStatusCode statusCode, string message) : base(message)
        {
            StatusCode = statusCode;
        }
        public HttpStatusCode StatusCode { get; set; }
    }
    public abstract class BaseClient
    {
        public BaseClient(string baseUrl)
        {
            BaseUrl = baseUrl;
        }

        public string BaseUrl { get; }

        public async Task<T> GetAsync<T>(RestRequest request) where T : new()
        {
            var response = await this.GetResponseAsync(request);

            if (response.ErrorException != null)
                throw new ApiException(response.StatusCode, response.ErrorMessage);

            var data = JsonConvert.DeserializeObject<T>(response.Content);
            return data;
        }

        public async Task<IRestResponse> GetResponseAsync(RestRequest request)
        {
            var client = new RestClient(this.BaseUrl);

            var response = await client.ExecuteTaskAsync(request);

            return response;
        }
    }
}