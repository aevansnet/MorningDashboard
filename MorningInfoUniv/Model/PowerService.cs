using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.SerialCommunication;
using Windows.Storage.Streams;

namespace MorningInfoUniv.Model
{
    public class PowerService :  IPowerService
    {

        private SerialDevice serialPort = null;
        private CancellationTokenSource ReadCancellationTokenSource;
        DataReader dataReaderObject = null;
        public Action<double, ushort> GotData { get; set; }

 
        public async void ConnectAndListen()
        {
            // attempt to connect to the first available serial device - will have to do for now
            try
            {
                string aqs = SerialDevice.GetDeviceSelector();
                var dis = await DeviceInformation.FindAllAsync(aqs);

                if (dis.Count < 1)
                {
                   // Status = "No serial devices found";
                }

                var entry = dis[0];
                serialPort = await SerialDevice.FromIdAsync(entry.Id);

                // Configure serial settings
                serialPort.WriteTimeout = TimeSpan.FromMilliseconds(1000);
                serialPort.ReadTimeout = TimeSpan.FromMilliseconds(1000);
                serialPort.BaudRate = 57600;
                serialPort.Parity = SerialParity.None;
                serialPort.StopBits = SerialStopBitCount.One;
                serialPort.DataBits = 8;
                serialPort.Handshake = SerialHandshake.None;

                // Create cancellation token object to close I/O operations when closing the device
                ReadCancellationTokenSource = new CancellationTokenSource();
                Listen();
            }

            catch (Exception ex)
            {
               // Status = ex.Message;
            }
        }

        /// <summary>
        /// - Create a DataReader object
        /// - Create an async task to read from the SerialDevice InputStream
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Listen()
        {
            try
            {
                if (serialPort != null)
                {
                    dataReaderObject = new DataReader(serialPort.InputStream);

                    // keep reading the serial input
                    while (true)
                    {
                        await ReadAsync(ReadCancellationTokenSource.Token);
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.GetType().Name == "TaskCanceledException")
                {
                   // Status = "Reading task was cancelled, closing device and cleaning up";
                    CloseDevice();
                }
                else
                {
                   // Status = ex.Message;
                }
            }
            finally
            {
                // Cleanup once complete
                if (dataReaderObject != null)
                {
                    dataReaderObject.DetachStream();
                    dataReaderObject = null;
                }
            }
        }

        /// <summary>
        /// ReadAsync: Task that waits on data and reads asynchronously from the serial device InputStream
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task ReadAsync(CancellationToken cancellationToken)
        {
            Task<UInt32> loadAsyncTask;

            uint ReadBufferLength = 1024;

            // If task cancellation was requested, comply
            cancellationToken.ThrowIfCancellationRequested();

            // Set InputStreamOptions to complete the asynchronous read operation when one or more bytes is available
            dataReaderObject.InputStreamOptions = InputStreamOptions.Partial;

            // Create a task object to wait for data on the serialPort.InputStream
            loadAsyncTask = dataReaderObject.LoadAsync(ReadBufferLength).AsTask(cancellationToken);

            // Launch the task and wait
            UInt32 bytesRead = await loadAsyncTask;
            if (bytesRead > 0)
            {
                // sometimes we'll get partial data here so will have to re-address later. for now we seem to be lucky
                var rcvdText = dataReaderObject.ReadString(bytesRead);
                // if we just check for newline terminator for now
                if (rcvdText.EndsWith("\n"))
                {
                    //System.Diagnostics.Debug.WriteLine("got data:" + rcvdText);
                    ParseData(rcvdText);
                }
            }
        }

        private void ParseData(string rcvdText)
        {

            try {
                string expression = @".*?<tmpr>([\d.]+)</tmpr>.*<ch1><watts>0*(\d+)</watts></ch1>";

                var match = Regex.Match(rcvdText, expression);
                if (match.Success)
                {
                    var temperature = match.Groups[1].Value;
                    var watts = match.Groups[2].Value;
                    GotData(double.Parse(temperature), ushort.Parse(watts));
                }
            }
            catch
            {//
                
            }       
        }


        private void CloseDevice()
        {
            if (serialPort != null)
            {
                serialPort.Dispose();
            }
            serialPort = null;            
        }

    }
}
