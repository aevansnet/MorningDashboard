using System;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using MorningInfoUniv.Model;

namespace MorningInfoUniv.ViewModel
{
    public class ClockViewModel : ViewModelBase
    {
        public const string ClockPropertyName = "Clock";

        private readonly IDataService _dataService;
        private readonly INavigationService _navigationService;
        private string _clock = "Starting...";       
        private bool _runClock;

        public string Clock
        {
            get
            {
                return _clock;
            }
            set
            {
                Set(ClockPropertyName, ref _clock, value);
            }
        }    
      
        public ClockViewModel(
            IDataService dataService,
            INavigationService navigationService)
        {
            _dataService = dataService;
            _navigationService = navigationService;
        }

        public void RunClock()
        {
            _runClock = true;

            Task.Run(async () =>
            {
                while (_runClock)
                {
                    try
                    {
                        DispatcherHelper.CheckBeginInvokeOnUI(() =>
                        {
                            Clock = DateTime.Now.ToString("HH:mm:ss");
                        });

                        await Task.Delay(1000);
                    }
                    catch (Exception ex)
                    {
                    }
                }
            });
        }

        public void StopClock()
        {
            _runClock = false;
        }       
    }
}