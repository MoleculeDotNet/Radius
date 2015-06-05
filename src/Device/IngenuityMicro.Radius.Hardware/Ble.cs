using System;
using System.IO.Ports;
using Microsoft.SPOT;

namespace IngenuityMicro.Radius.Hardware
{

        public class Ble
        {
            public delegate void RfPipeReceivedHandler(string val);
            public event RfPipeReceivedHandler DataReceived;
            private readonly SimpleSerial _bleSerial;
            private string[] _dataIn;

            public Ble()
            {
                _bleSerial = new SimpleSerial("COM1", 57600, Parity.None, 8, StopBits.One);
                _bleSerial.DataReceived += _rfSerial_DataReceived;
                _bleSerial.Open();
            }
            /// <summary>
            /// Send data via the Rf Pipe
            /// </summary>
            /// <param name="data">Data to send</param>
            public void SendData(string data)
            {
                _bleSerial.WriteLine(data);
            }
            void _rfSerial_DataReceived(object sender, SerialDataReceivedEventArgs e)
            {
                _dataIn = _bleSerial.Deserialize();
                for (int index = 0; index < _dataIn.Length; index++)
                {
                    if (DataReceived != null)
                    {
                        DataReceived(_dataIn[index]);
                    }
                }
            }
        }
    }
