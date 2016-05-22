using GalaSoft.MvvmLight;
using MorningInfoUniv.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Threading;
using Microsoft.Azure.Devices.Client;

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
            ConnectToHub();
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
                if(deviceClient != null)
                {
                    SendEvent(_power.ToString());
                }
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

        DeviceClient deviceClient;


        // Define the connection string to connect to IoT Hub
        private const string DeviceConnectionString = "HostName=PiHub1.azure-devices.net;DeviceId=Kitchen1;SharedAccessKey=5W3Or4NT1RmUR1z4jr/qm/FUTgtp88wkEEJA2tBiZ/k=";
        void ConnectToHub()
        {
            // Create the IoT Hub Device Client instance
            deviceClient = DeviceClient.CreateFromConnectionString(DeviceConnectionString);

            // Send an event
           // SendEvent(deviceClient).Wait();

            // Receive commands in the queue
            //ReceiveCommands(deviceClient).Wait();

            
        }
        // Create a message and send it to IoT Hub.
        async Task SendEvent(string power)
        {
            string dataBuffer;
            dataBuffer = power;
            Message eventMessage = new Message(Encoding.UTF8.GetBytes(dataBuffer));
            await deviceClient.SendEventAsync(eventMessage);
        }
        // Receive messages from IoT Hub
        static async Task ReceiveCommands(DeviceClient deviceClient)
        {
            System.Diagnostics.Debug.WriteLine("\nDevice waiting for commands from IoTHub...\n");
            Message receivedMessage;
            string messageData;
            while (true)
            {
                receivedMessage = await deviceClient.ReceiveAsync(TimeSpan.FromSeconds(1));

                if (receivedMessage != null)
                {
                    messageData = Encoding.ASCII.GetString(receivedMessage.GetBytes());
                    System.Diagnostics.Debug.WriteLine("\t{0}> Received message: {1}", DateTime.Now.ToLocalTime(), messageData);
                    await deviceClient.CompleteAsync(receivedMessage);
                }
            }
        }
    }



}
