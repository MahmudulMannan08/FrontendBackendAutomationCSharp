using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIAutomationLibrary.Base.RestApi
{
    public class ReserveVehicleApi
    {
        RestApiManager restApiManager;

        public ReserveVehicleApi(string baseUrl)
        {
            restApiManager = new RestApiManager(baseUrl);
        }

        public RestResponse ExpiredReservationsApi(string expiredReservationEndPoint)
        {
            return restApiManager.GetRequestAsync(expiredReservationEndPoint).GetAwaiter().GetResult();
        }

        public void Dispose()
        {
            restApiManager.Dispose();
        }
    }
}