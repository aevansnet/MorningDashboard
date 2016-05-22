using System;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using MorningInfoUniv.Model;
using MorningInfoUniv.RailServiceMessaging;
using System.Collections.ObjectModel;
using System.Linq;

namespace MorningInfoUniv.ViewModel
{
    public class DeparturesViewModel : ViewModelBase
    {   
        private readonly TrainServiceCache _serviceCache;

        private ArrivalDepartureBoard _departures;
        public ArrivalDepartureBoard Departures {
            get
            {
                return _departures;
            }
            set
            {
                _departures = value;
                RaisePropertyChanged();
            }
        }
        public string Station { get; set; }

        public DeparturesViewModel(TrainServiceCache serviceCache)
        {
            _serviceCache = serviceCache;
            Station = "HAV";

            _serviceCache.CollectionChanged += _serviceCache_CollectionChanged;
           

            if (IsInDesignModeStatic)
            {
                LoadDepartures();
            }
        }

        private void _serviceCache_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (var newItem in e.NewItems)
                {
                    var departureBoard = newItem as ArrivalDepartureBoard;
                    if(departureBoard.Station == Station)
                    {
                        Departures = departureBoard;
                        departureBoard.CollectionChanged += DepartureBoard_CollectionChanged;
                    }
                }
            }
        }

        private void DepartureBoard_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
           
        }

        public async void LoadDepartures()
        {

            _serviceCache.StartContinuousUpdate(Station, new System.Threading.CancellationToken(), "WAT");

            //var results = await _railWebservice.GetDepartures(10, "HAV");
            //Departures.Clear();
            //foreach (var result in results)
            //    Departures.Add(result);


        }


    }
}