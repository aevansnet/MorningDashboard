using GalaSoft.MvvmLight;
using MorningInfoUniv.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Threading;

namespace MorningInfoUniv.ViewModel
{
    public class PowerMonitorViewModel : ViewModelBase
    {
        IPowerService _pService;
        public PowerMonitorViewModel(IPowerService pService)
        {
            _pService = pService;
            _pService.GotData = (t, p) =>
            {
                DispatcherHelper.CheckBeginInvokeOnUI(() => { Power = p; Temperature = t; });
            };
              
            _pService.ConnectAndListen();
        }

        ushort _power;
        public ushort Power
        {
            get
            {
                return _power;
            }
            set
            {
                _power = value;
                RaisePropertyChanged();
            }
        }

        double _temperature;
        public double Temperature
        {
            get
            {
                return _temperature;
            }
            set
            {
                _temperature = value;
                RaisePropertyChanged();
            }
        }
    }
}
