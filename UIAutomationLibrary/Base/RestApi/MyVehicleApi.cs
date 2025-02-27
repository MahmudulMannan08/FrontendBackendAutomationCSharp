using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIAutomationLibrary.Base.RestApi.Response;

namespace UIAutomationLibrary.Base.RestApi
{
    public class MyVehicleApi
    {
        RestApiManager restApiManager;

        public MyVehicleApi(string baseUrl, string apiKey, string apiValue)
        {
            restApiManager = new RestApiManager(baseUrl, apiKey, apiValue);
        }

        public RestResponse GetVehicleInformationByVin(string vehicleInformationByVinEndpoint, string vinParameterKey, string vinParameterValue)
        {
            var restRequest = new RestRequest(vehicleInformationByVinEndpoint)
                .AddParameter(vinParameterKey, vinParameterValue);

            return restApiManager.GetRequestAsync(restRequest).GetAwaiter().GetResult();
        }

        public VehicleInformationByVinResponse GetVehicleInformationByVinResponseContent(RestResponse getVehicleInformationByVinResponse)
        {
            return restApiManager.GetResponseContent<VehicleInformationByVinResponse>(getVehicleInformationByVinResponse);
        }

        public void Dispose()
        {
            restApiManager.Dispose();
        }
    }
}
