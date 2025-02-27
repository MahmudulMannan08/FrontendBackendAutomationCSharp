using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIAutomationLibrary.Base.RestApi.Response
{
    public class VehicleInformationByVinResponse
    {
        public bool IsPassengerVehicle { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public Trim[] Trims { get; set; }
    }

    public class Trim
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
