using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIAutomationLibrary.Base.RestApi
{
    public class RestApiManager
    {
        private RestClient restClient;

        public RestApiManager(string baseUrl)
        {
            var options = new RestClientOptions(baseUrl);
            restClient = new RestClient(options);
        }

        public RestApiManager(string baseUrl, string defaultHeaderKey, string defaultHeaderValue)
        {
            var options = new RestClientOptions(baseUrl);
            restClient = new RestClient(options)
                .AddDefaultHeader(defaultHeaderKey, defaultHeaderValue);
        }

        /// <summary>
        /// Execute asynchronus get call on request, awaiting the operation completion. 
        /// </summary>
        /// <param name="endPoint">Request endpoint (Not the base url).</param>
        /// <returns>Returns deserialized response. Throws error on all error cases except 404 in which case returns null.</returns>
        public async Task<RestResponse> GetRequestAsync(string endPoint)
        {
            var response = await restClient.GetAsync(new RestRequest(endPoint, Method.Get));
            return response;
        }

        /// <summary>
        /// Execute asynchronus get call on request, awaiting the operation completion. 
        /// </summary>
        /// <param name="restRequest">Request container data with endpoint and optional header, parameter and/or request body.</param>
        /// <returns>Returns serialized response. Throws error on all error cases except 404 in which case returns null.</returns>
        public async Task<RestResponse> GetRequestAsync(RestRequest restRequest)
        {
            var response = await restClient.ExecuteAsync(restRequest);
            return response;
        }

        /// <summary>
        /// Deserializes RestReponse to generic type. 
        /// </summary>
        /// <param name="restResponse">Serialized RestResponse container data.</param>
        /// <returns>Returns deserialized response of generic type.</returns>
        public T GetResponseContent<T>(RestResponse restResponse)
        {
            var responseContent = restResponse?.Content;
            return JsonConvert.DeserializeObject<T>(responseContent);
        }

        public void Dispose()
        {
            restClient?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
