using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Threading;
using MorningInfoUniv.RailServiceMessaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MorningInfoUniv.Model
{
    public class TrainServiceCache : ObservableCollection<ArrivalDepartureBoard>
    {

        private IRailWebservice _webservice;

        public TrainServiceCache(IRailWebservice webservice)
        {
            _webservice = webservice;
        }

        public void StartContinuousUpdate(string station, CancellationToken token, string destination = null)
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    try
                    {
                        await GetArrDepData(station, destination);
                        
                    }
                    catch (Exception ex)
                    {
                    }
                    finally
                    {
                        await Task.Delay(60000);
                    }
                }
            }, token);
        }


        public async Task GetArrDepData(string station, string destination = null)
        {
            var arrDeparture = this.FirstOrDefault(s => station == s.Station);

            DispatcherHelper.CheckBeginInvokeOnUI(() =>
            {
                
                if (arrDeparture == null)
                {
                    arrDeparture = new ArrivalDepartureBoard() { Station = station };
                    Add(arrDeparture);
                }
                arrDeparture.Updating = true;

            });
                var services = await _webservice.GetDepartures(18, station, destination);
                //var services = await _webservice.GetArrivals(18, station, destination);


            DispatcherHelper.CheckBeginInvokeOnUI(() =>
            {
               

                var newIDs = services.Select(s => s.ServiceID);
                var existingIDs = arrDeparture.Select(s => s.ServiceID).ToList();               

                // remove any sevrices that have dissappeared
                foreach(var id in existingIDs)
                {
                    if (!newIDs.Contains(id))
                    {
                        arrDeparture.Remove(arrDeparture.First(s => s.ServiceID == id));
                    }
                }             

                foreach (var service in services)
                {
                   
                    var serviceToUpdate = arrDeparture.FirstOrDefault(s => s.ServiceID == service.ServiceID);
                    if(serviceToUpdate == null)
                    {
                        // new service so add it
                        arrDeparture.Add(service);
                    }
                    else
                    {
                        arrDeparture[arrDeparture.IndexOf(serviceToUpdate)] = service;
                        //OnCollectionChanged(new System.Collections.Specialized.NotifyCollectionChangedEventArgs(null, serviceToUpdate));
                    }
                   
                }
                arrDeparture.LastUpdated = DateTime.Now;
                arrDeparture.Updating = false;
            });


        }
    }

    public class ArrivalDepartureBoard : ObservableCollection<TrainService> 
    {
        public string Station { get; set; }
        private DateTime _lastUpdated;
        public DateTime LastUpdated
        {
            get
            {
                return _lastUpdated;
            }
            set {
                _lastUpdated = value;
                OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs("LastUpdated"));
            }
        }
        private bool _updating;
        public bool Updating
        {
            get
            {
                return _updating;
            }
            set
            {
                _updating = value;
                OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs("Updating"));
            }
        }

    }
    public class TrainService : ObservableObject
    {
        public string Destination { get; set; }
        public string ScheduledDeparture { get; set; }
        public string ExpectedDepature { get; set; }
        public string ServiceID { get; set; }
        public string ActualTermination { get; set; }
        public string EstimatedTermination { get; set; }
        public string ScheduledTermination { get; set; }

    }
}
