using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using MorningInfoUniv.RailService;
using MorningInfoUniv.Model;
using System.IO;

namespace MorningInfoUniv.RailServiceMessaging
{
    public class DesignRailWebservice : IRailWebservice
    {

        private string _accessToken;
        public DesignRailWebservice(string accessToken)
        {
            _accessToken = accessToken;
            
        }


        public async Task<List<TrainService>> GetDepartures(ushort numberOfresults, string startingStation, string destinationStation = null)
        {
            string jsondata = "[{\"Destination\":\"London Waterloo\",\"ExpectedDepature\":\"On time\",\"ScheduledDeparture\":\"05:40\",\"ServiceID\":\"4Y4RiQh11azN2cNQbqXRMg==\"},{\"Destination\":\"London Waterloo\",\"ExpectedDepature\":\"On time\",\"ScheduledDeparture\":\"06:11\",\"ServiceID\":\"oLzkuSqsNqD3EXcBjlDnoA==\"},{\"Destination\":\"London Waterloo\",\"ExpectedDepature\":\"On time\",\"ScheduledDeparture\":\"06:34\",\"ServiceID\":\"KI1NhkIjHQTHu0EO\\/jHwnA==\"},{\"Destination\":\"London Waterloo\",\"ExpectedDepature\":\"On time\",\"ScheduledDeparture\":\"06:50\",\"ServiceID\":\"aU4o7Ay61mJk2bFgSTT2GQ==\"},{\"Destination\":\"London Waterloo\",\"ExpectedDepature\":\"07:50\",\"ScheduledDeparture\":\"07:00\",\"ServiceID\":\"R1+GaoylsCDD+zt5\\/9IIgg==\"},{\"Destination\":\"London Waterloo\",\"ExpectedDepature\":\"On time\",\"ScheduledDeparture\":\"07:11\",\"ServiceID\":\"Gl2YbopD\\/bQh+vZoHmXHFw==\"}]";
            var deserializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(List<TrainService>));

            var bytes = Encoding.Unicode.GetBytes(jsondata);
            return deserializer.ReadObject(new MemoryStream(bytes)) as List<TrainService>;          
        }
    }
}
