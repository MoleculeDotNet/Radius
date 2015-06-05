using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Devices.Enumeration;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace RadiusAccessoryApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        List<DeviceInformation> _devices = new List<DeviceInformation>();
        private string val;
        private GattDeviceService _bluetoothGattDeviceService;
        private IReadOnlyList<GattCharacteristic> _readCharacteristic;
        private GattCharacteristic _incomingFluff;
        private byte[] fluffData;
        string[] _dataIn;
        private StringBuilder idString = new StringBuilder();
        private string yes;

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.


            //TEST CODE!
            var devices = await DeviceInformation.FindAllAsync(GattDeviceService.GetDeviceSelectorFromUuid(new Guid("2AC94B65-C8F4-48A4-804A-C03BC6960B80")));
            foreach (DeviceInformation d in devices)
            {
                Debug.WriteLine(d.Name);
                _devices.Add(d);
                foreach (var p in d.Properties)
                {
                    Debug.WriteLine(p.Key + " : " + p.Value);
                }
            }

            _bluetoothGattDeviceService = await GattDeviceService.FromIdAsync(_devices[0].Id);
            if (_bluetoothGattDeviceService == null)
                Debug.WriteLine("Bluetooth gatt service not found");
            else
            {
                _readCharacteristic = _bluetoothGattDeviceService.GetCharacteristics(new Guid("50E03F22-B496-4A73-9E85-335482ED4B12"));
                _incomingFluff = _bluetoothGattDeviceService.GetCharacteristics(new Guid("4FD800F8-D3B6-48F9-B232-29E95984F76D"))[0];
                _incomingFluff.ValueChanged += _incomingFluff_ValueChanged;
                await _incomingFluff.WriteClientCharacteristicConfigurationDescriptorAsync(GattClientCharacteristicConfigurationDescriptorValue.Notify);
            }

            //END TEST CODE
        }

        void _incomingFluff_ValueChanged(GattCharacteristic sender, GattValueChangedEventArgs args)
        {
            fluffData = new byte[args.CharacteristicValue.Length];
            DataReader.FromBuffer(args.CharacteristicValue).ReadBytes(fluffData);

            foreach (byte b in fluffData)
            {
                if (b == '\n')
                {
                    idString.Clear();
                }
                else
                {
                    if (b == '\r')
                    {
                        yes = idString.ToString();

                    }
                    else
                    {
                        idString.Append(Convert.ToChar(b));
                    }
                }
            }
        }

        private async void SendData(string val)
        {
            var writer = new DataWriter();
            writer.WriteString(val + "\r\n");
            await _readCharacteristic[0].WriteValueAsync(writer.DetachBuffer());
            // UxOutput.Text = status == GattCommunicationStatus.Success ? "Sent" : "Whoops";
        }

    }
}
