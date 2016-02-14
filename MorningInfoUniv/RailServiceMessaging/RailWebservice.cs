using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using MorningInfoUniv.RailService;
using MorningInfoUniv.Model;
using System.Runtime.CompilerServices;

namespace MorningInfoUniv.RailServiceMessaging
{
    public class RailWebservice : IRailWebservice
    {

        private AccessToken _accessToken;
        private LDBServiceSoapClient _service;
        public RailWebservice(string accessToken)
        {
            _accessToken = new AccessToken() { TokenValue = accessToken };
            _service = new LDBServiceSoapClient();
        }

        public async Task<List<TrainService>> GetDepartures(ushort numberOfresults, string startingStation, string destinationStation = null)
        {            
            var response = await MakeRequestWithHeaders(numberOfresults, startingStation, destinationStation);
            if (response.GetStationBoardResult.trainServices != null)
            {
                return response.GetStationBoardResult.trainServices.Select(s => new TrainService() {
                    Destination = s.destination[0].locationName,
                    ScheduledDeparture = s.std,
                    ExpectedDepature = s.etd,
                    ServiceID = s.serviceID})
                    .ToList();
            }
            else
            {
                return new List<TrainService>();
            }
        }

        public Task<GetDepartureBoardResponse> MakeRequestWithHeaders(ushort numberOfresults, string startingStation, string destinationStation = null)
        {
            var responce = new List<TrainService>();
            using (new OperationContextScope(_service.InnerChannel))
            {
                var msgHeader = MessageHeader.CreateHeader("AccessToken", "", _accessToken);
                OperationContext.Current.OutgoingMessageHeaders.Add(msgHeader);
                return _service.GetDepartureBoardAsync(numberOfresults, startingStation, destinationStation, FilterType.to, 0, 280);
                //return _service.GetDepBoardWithDetailsAsync(numberOfresults, startingStation, destinationStation, FilterType.to, 0, 180);
                
            }
        }
    }
}
