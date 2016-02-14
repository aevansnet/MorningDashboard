using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MorningInfoUniv.RailService;
using MorningInfoUniv.Model;

namespace MorningInfoUniv.RailServiceMessaging
{
    public interface IRailWebservice
    {

        Task<List<TrainService>> GetDepartures(ushort numberOfresults, string startingStation, string destinationStation = null);

    }
}
