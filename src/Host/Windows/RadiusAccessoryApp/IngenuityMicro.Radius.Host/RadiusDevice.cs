using IngenuityMicro.Radius.Host.Messages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
        private readonly RadiusDeviceEnumerator _parent;
        private GattDeviceService _bluetoothGattDeviceService;
        private IReadOnlyList<GattCharacteristic> _readCharacteristic;
        private GattCharacteristic _incomingData;
        private DeviceInformation _di;
        private ManualResetEvent _initializedEvent = new ManualResetEvent(false);
        private bool _fInitialized = false;
        private bool _fFailed = false;
        private CircularBuffer<byte> _buffer = new CircularBuffer<byte>(1024, 1, 1024);

        public RadiusDevice(RadiusDeviceEnumerator parent, DeviceInformation di)
        {
            _parent = parent;
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

                    var setTimeMsg = new SetTimeMessage();
                    setTimeMsg.CurrentUtcTime = DateTime.UtcNow;
                    var tx = TimeZoneInfo.Local;
                    var offset = (int)tx.BaseUtcOffset.TotalMinutes;
                    if (tx.IsDaylightSavingTime(DateTime.Now))
                        offset = offset + 60;
                    setTimeMsg.TzOffset = offset;
                    this.Send(setTimeMsg);

                    var getAppsMsg = new GetInstalledAppsMessage();
                    this.Send(getAppsMsg);
                }
                catch (Exception ex)
                {
                    _fFailed = true;
                    Debug.WriteLine("Initialization failed with exception : " + ex);
                }
            }
            _fInitialized = true;
            _initializedEvent.Set();
        }

        private void EnsureInitialized()
        {
            _initializedEvent.WaitOne();
        }

        private void _incomingData_ValueChanged(GattCharacteristic sender, GattValueChangedEventArgs args)
        {
            var len = args.CharacteristicValue.Length;
            var buffer = new byte[len];
            DataReader.FromBuffer(args.CharacteristicValue).ReadBytes(buffer);
            _buffer.Put(buffer);

            var idxNewline = _buffer.IndexOf((byte)0x0a); // 0x0a == \n
            if (idxNewline!=-1)
            {
                var data = _buffer.Get(idxNewline);
                _buffer.Skip(1);
                var line = ConvertToString(data).Trim();
                Debug.WriteLine("RECV: (" + this._di.Name + ") : \"" + line + '"');
                ProcessReceivedData(line);
            }
        }

        public void Send(RadiusMessage msg)
        {
            var text = msg.Serialize();
            Debug.WriteLine("SEND: (" + this._di.Name + ") : \"" + text + '"');
            SendData(text);
            _parent.AddSentMessage(msg);
        }

        private void ProcessReceivedData(string line)
        {
            if (line == null || line.Length == 0)
                return;

            try
            {
                var jobj = JObject.Parse(line);
                if (jobj["replyTo"] != null)
                {
                    // this is a reply
                    var messageId = (int)jobj["replyTo"];
                    int status = -1;
                    if (jobj["status"] != null)
                        status = (int)jobj["status"];

                    Dictionary<string, object> results = new Dictionary<string, object>();
                    if (jobj["result"] != null)
                    {
                        results = JsonConvert.DeserializeObject<Dictionary<string, object>>(jobj["result"].ToString());
                    }

                    var msg = _parent.FindSentMessage(messageId);
                    if (msg != null)
                    {
                        bool handled;
                        msg.OnResponseReceived(new RadiusMessageResponse(msg, status, results), out handled);
                        if (handled)
                            _parent.RemoveSentMessage(messageId);
                    }
                }
                else if (jobj["eventId"] != null)
                {

                }
            }
            catch (Exception ex)
            {
                // the show must go on
            }
        }

        private void ProcessEvent(string line)
        {
            throw new NotImplementedException();
        }

        private async void SendData(string val)
        {
            val += "\r\n";
            int maxLen = 16;
            int len = val.Length;
            int offset = 0;
            while (true)
            {
                var sendLen = len - offset;
                if (sendLen == 0)
                    break;
                if (sendLen > maxLen)
                    sendLen = maxLen;

                var writer = new DataWriter();
                writer.WriteString(val.Substring(offset, sendLen));
                await _readCharacteristic[0].WriteValueAsync(writer.DetachBuffer());
                offset += sendLen;
            }
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
