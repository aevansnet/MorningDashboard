using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MorningInfoUniv.Model
{
    public interface IPowerService
    {
        void ConnectAndListen();
        Action<double, ushort> GotData { set; }
    }
}
