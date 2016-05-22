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
using System.Runtime.Serialization.Json;

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
            var arrivals = await MakeArrivalsRequestWithHeaders(numberOfresults, destinationStation, startingStation);
            if (arrivals.GetStationBoardResult.trainServices != null)
            {
                var res = arrivals.GetStationBoardResult.trainServices.Select(s => new TrainService()
                {
                    Destination = s.destination[0].locationName,

                    ScheduledDeparture = s.previousCallingPoints[0].callingPoint.Single(c => c.crs == startingStation).st,
                    ExpectedDepature = s.previousCallingPoints[0].callingPoint.Single(c => c.crs == startingStation).et,
                    ServiceID = s.serviceID })
                                .ToList();

                return res;
            }
            
        
            else
            {
                return new List<TrainService>();
            }

        }


        //public async Task<List<TrainService>> GetDepartures(ushort numberOfresults, string startingStation, string destinationStation = null)
        //{
        //    return GetArrivals(numberOfresults, startingStation, destinationStation);

        //    var arrivals = await MakeArrivalsRequestWithHeaders(numberOfresults, "WAT", "HAV");
        //    var past = await MakePastRequestWithHeaders(numberOfresults, startingStation, destinationStation);
        //    var future = await MakeFutureRequestWithHeaders(numberOfresults, startingStation, destinationStation);
        //    if (past.GetStationBoardResult.trainServices != null)
        //    {
        //        var res =  past.GetStationBoardResult.trainServices.Select(s => new TrainService() {
        //            Destination = s.destination[0].locationName,
        //            ScheduledDeparture = s.std,
        //            ExpectedDepature = s.etd,
        //            ServiceID = s.serviceID})
        //            .ToList();

        //        res.AddRange(future.GetStationBoardResult.trainServices.Select(s => new TrainService()
        //        {
        //            Destination = s.destination[0].locationName,
        //            ScheduledDeparture = s.std,
        //            ExpectedDepature = s.etd,
        //            ServiceID = s.serviceID
        //        })
        //            .ToList());


        //        //DataContractJsonSerializer f = new DataContractJsonSerializer(res.GetType());
        //        //using(System.IO.MemoryStream stream = new System.IO.MemoryStream())
        //        //{
        //        //    f.WriteObject(stream, res);
        //        //    stream.Position = 0;
        //        //    var content = new System.IO.StreamReader(stream).ReadToEnd();
        //        //}
        //        foreach (var serivce in res)
        //        {
        //            var details = await MakeServiceDetailsRequestWithHeaders(serivce.ServiceID);
        //            var callingPoints = details.GetServiceDetailsResult.subsequentCallingPoints.FirstOrDefault().callingPoint;
        //            var destination = callingPoints.Where(s => s.crs == destinationStation);
        //            serivce.ScheduledTermination = destination.FirstOrDefault().st;
        //            serivce.ActualTermination = destination.FirstOrDefault().at;
        //            serivce.EstimatedTermination = destination.FirstOrDefault().et;
        //        }



        //        return res;
        //    }
        //    else
        //    {
        //        return new List<TrainService>();
        //    }
        //}
        public Task<GetArrBoardWithDetailsResponse> MakeArrivalsRequestWithHeaders(ushort numberOfresults, string startingStation, string destinationStation = null)
        {
            var responce = new List<TrainService>();
            using (new OperationContextScope(_service.InnerChannel))
            {
                var msgHeader = MessageHeader.CreateHeader("AccessToken", "", _accessToken);
                OperationContext.Current.OutgoingMessageHeaders.Add(msgHeader);
                //return _service.GetDepartureBoardAsync(numberOfresults, startingStation, destinationStation, FilterType.to, -120, 120);

                return _service.GetArrBoardWithDetailsAsync(numberOfresults, startingStation, destinationStation, FilterType.from, 0, 120);

            }
        }
        public Task<GetDepartureBoardResponse> MakePastRequestWithHeaders(ushort numberOfresults, string startingStation, string destinationStation = null)
        {
            var responce = new List<TrainService>();
            using (new OperationContextScope(_service.InnerChannel))
            {
                var msgHeader = MessageHeader.CreateHeader("AccessToken", "", _accessToken);
                OperationContext.Current.OutgoingMessageHeaders.Add(msgHeader);
                //return _service.GetDepartureBoardAsync(numberOfresults, startingStation, destinationStation, FilterType.to, -120, 120);
               
                return _service.GetDepartureBoardAsync(numberOfresults, startingStation, destinationStation, FilterType.to, -180, 120);
                
            }
        }

        public Task<GetDepartureBoardResponse> MakeFutureRequestWithHeaders(ushort numberOfresults, string startingStation, string destinationStation = null)
        {
            var responce = new List<TrainService>();
            using (new OperationContextScope(_service.InnerChannel))
            {
                var msgHeader = MessageHeader.CreateHeader("AccessToken", "", _accessToken);
                OperationContext.Current.OutgoingMessageHeaders.Add(msgHeader);
                //return _service.GetDepartureBoardAsync(numberOfresults, startingStation, destinationStation, FilterType.to, -120, 120);

                return _service.GetDepartureBoardAsync(numberOfresults, startingStation, destinationStation, FilterType.to, 0, 220);

            }
        }

        public Task<GetServiceDetailsResponse> MakeServiceDetailsRequestWithHeaders(string serviceId)
        {
            var responce = new List<TrainService>();
            using (new OperationContextScope(_service.InnerChannel))
            {
                var msgHeader = MessageHeader.CreateHeader("AccessToken", "", _accessToken);
                OperationContext.Current.OutgoingMessageHeaders.Add(msgHeader);
                return _service.GetServiceDetailsAsync(serviceId);                
            }
        }
    }
}
