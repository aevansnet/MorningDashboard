using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using MorningInfoUniv.RailService;
using MorningInfoUniv.Model;

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

            return new List<TrainService>()
            {
                new TrainService() { Destination = "London Waterloo", ScheduledDeparture="06:34" },
                new TrainService() { Destination = "London Waterloo", ScheduledDeparture="08:50" },
                new TrainService() { Destination = "London Waterloo", ScheduledDeparture="09:15" },
                new TrainService() { Destination = "London Waterloo", ScheduledDeparture="10:50" },
                new TrainService() { Destination = "London Waterloo", ScheduledDeparture="12:50" },
                new TrainService() { Destination = "London Waterloo", ScheduledDeparture="14:50" }
            };
        }
    }
}
