using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using MorningInfoUniv.Model;
using MorningInfoUniv.RailServiceMessaging;

namespace MorningInfoUniv.ViewModel
{
    public class ViewModelLocator
    {
        public const string SecondPageKey = "SecondPage";

        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            var nav = new NavigationService();
            nav.Configure(SecondPageKey, typeof(SecondPage));
            SimpleIoc.Default.Register<INavigationService>(() => nav);
      
            SimpleIoc.Default.Register<IDialogService, DialogService>();

            SimpleIoc.Default.Register<TrainServiceCache>();

            if (ViewModelBase.IsInDesignModeStatic)
            {
                SimpleIoc.Default.Register<IDataService, Design.DesignDataService>();
                SimpleIoc.Default.Register<IRailWebservice>(() => new DesignRailWebservice(""));

            }
            else
            {
                SimpleIoc.Default.Register<IDataService, DataService>();
                SimpleIoc.Default.Register<IRailWebservice>(() => new RailWebservice("xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"));

            }

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<ClockViewModel>();
            SimpleIoc.Default.Register<DeparturesViewModel>();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public MainViewModel Main => ServiceLocator.Current.GetInstance<MainViewModel>();
        public ClockViewModel Clock => ServiceLocator.Current.GetInstance<ClockViewModel>();
        public DeparturesViewModel Departures => ServiceLocator.Current.GetInstance<DeparturesViewModel>();
    }
}
