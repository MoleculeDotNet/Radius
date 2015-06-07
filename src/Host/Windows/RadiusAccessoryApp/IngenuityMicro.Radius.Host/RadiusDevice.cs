using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Devices.Enumeration;
using Windows.Storage.Streams;

namespace IngenuityMicro.Radius.Host
{
    public class RadiusDevice
    {
        private GattDeviceService _bluetoothGattDeviceService;
        private IReadOnlyList<GattCharacteristic> _readCharacteristic;
        private GattCharacteristic _incomingData;
        private DeviceInformation _di;
        private ManualResetEvent _initializedEvent = new ManualResetEvent(false);
        private bool _fInitialized = false;
        private bool _fFailed = false;
        private CircularBuffer<byte> _buffer = new CircularBuffer<byte>(1024, 1, 1024);
        private Dictionary<int, RadiusMessage> _sentMessages = new Dictionary<int, RadiusMessage>();

        public RadiusDevice(DeviceInformation di)
        {
            _di = di;

            Task.Run(() => InitializeAsync());
        }

        public bool IsReady { get { return _fInitialized & !_fFailed; } }
        public bool InitializationFailed { get { return _fFailed; } }

        private async void InitializeAsync()
        {
            _bluetoothGattDeviceService = await GattDeviceService.FromIdAsync(_di.Id);
            if (_bluetoothGattDeviceService == null)
            {
                _fFailed = true;
                Debug.WriteLine("Bluetooth gatt service not found");
            }
            else
            {
                try
                {
                    _readCharacteristic = _bluetoothGattDeviceService.GetCharacteristics(new Guid("50E03F22-B496-4A73-9E85-335482ED4B12"));
                    _incomingData = _bluetoothGattDeviceService.GetCharacteristics(new Guid("4FD800F8-D3B6-48F9-B232-29E95984F76D"))[0];
                    _incomingData.ValueChanged += _incomingData_ValueChanged;
                    await _incomingData.WriteClientCharacteristicConfigurationDescriptorAsync(GattClientCharacteristicConfigurationDescriptorValue.Notify);
                }
                catch (Exception ex)
                {
                    _fFailed = true;
                    Debug.WriteLine("Initialization failed with exception : " + ex);
                }
            }
            _fInitialized = true;
            _initializedEvent.Set();

            SendData("#Radius#SetTime:");
        }

        private void EnsureInitialized()
        {
            _initializedEvent.WaitOne();
        }

        private void _incomingData_ValueChanged(GattCharacteristic sender, GattValueChangedEventArgs args)
        {
            var buffer = new byte[args.CharacteristicValue.Length];
            DataReader.FromBuffer(args.CharacteristicValue).ReadBytes(buffer);
            _buffer.Put(buffer);

            var idxNewline = _buffer.IndexOf((byte)0x10); // 0x10 == \n
            if (idxNewline!=-1)
            {
                var data = _buffer.Get(idxNewline);
                _buffer.Skip(1);
                var line = ConvertToString(data).Trim();
                ProcessReceivedData(line);
            }
        }

        public void Send(RadiusMessage msg)
        {
            var text = msg.Serialize();
            SendData(text);
            _sentMessages.Add(msg.MessageId, msg);
        }

        private void ProcessReceivedData(string line)
        {
            if (line[0] != '#' && line[0]!='+')  // # is a response, + is an unsolicited message
                return;

            if (line[0] == '#')
                ProcessResponse(line);
            else if (line[0] == '+')
                ProcessEvent(line);
            else
            {
                // Error!
            }
        }

        private void ProcessResponse(string line)
        {
            var idxEnd = line.IndexOf('#', 1);
            if (idxEnd == -1)
                return;

            var target = line.Substring(1, idxEnd - 1);

            var start = idxEnd + 1;
            idxEnd = line.IndexOf('#', start);
            if (idxEnd == -1)
                return;
            int id;
            if (!Int32.TryParse(line.Substring(start, idxEnd - 1), out id))
                return;

            // response to a non-existent message
            if (!_sentMessages.ContainsKey(id))
                return;

            var replyArgs = new Dictionary<string, string>();
            var args = line.Substring(idxEnd + 1).Split('#');
            foreach (var arg in args)
            {
                var tokens = arg.Split('|');
                if (tokens.Length==2)
                {
                    replyArgs.Add(tokens[0], tokens[1]);
                }
            }

            _sentMessages[id].OnResponseReceived(this, replyArgs);
        }

        private void ProcessEvent(string line)
        { 
        }

        private async void SendData(string val)
        {
            var writer = new DataWriter();
            writer.WriteString(val + "\r\n");
            await _readCharacteristic[0].WriteValueAsync(writer.DetachBuffer());
            // UxOutput.Text = status == GattCommunicationStatus.Success ? "Sent" : "Whoops";
        }

        private static String ConvertToString(Byte[] byteArray)
        {
            var _chars = new char[byteArray.Length];
            bool _completed;
            int _bytesUsed, _charsUsed;
            Encoding.UTF8.GetDecoder().Convert(byteArray, 0, byteArray.Length, _chars, 0, byteArray.Length, false, out _bytesUsed, out _charsUsed, out _completed);
            return new string(_chars, 0, _charsUsed);
        }
    }
}
