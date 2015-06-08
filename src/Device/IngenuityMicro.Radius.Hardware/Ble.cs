using System;
using System.IO.Ports;
using Microsoft.SPOT;
using System.Threading;

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

            public void SendData(string data)
            {
                data += "\r\n";
                int maxLen = 8;
                int len = data.Length;
                int offset = 0;
                while (true)
                {
                    var sendLen = len - offset;
                    if (sendLen == 0)
                        break;
                    if (sendLen > maxLen)
                        sendLen = maxLen;

                    _bleSerial.Write(data.Substring(offset, sendLen));
                    offset += sendLen;
                }
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
