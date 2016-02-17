using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MorningInfoUniv.Model
{
    class MockPowerService : IPowerService
    { 
        public Action<double, ushort> GotData { get; set; }

        public void ConnectAndListen()
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(7000);
                    Random r = new Random(DateTime.Now.Millisecond);
                    double temperature = r.Next(190, 240) / 10;
                    ushort power = (ushort)r.Next(243, 8000);
                    GotData(temperature, power);

                }
            });
        }
    }
}
