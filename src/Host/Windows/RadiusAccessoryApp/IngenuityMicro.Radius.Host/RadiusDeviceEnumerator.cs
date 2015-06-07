using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Devices.Enumeration;

namespace IngenuityMicro.Radius.Host
{
    public class RadiusDeviceEnumerator
    {
        private Dictionary<string, RadiusDevice> _devices = new Dictionary<string, RadiusDevice>();

        public RadiusDeviceEnumerator()
        {
            Task.Run(() => InitializeAsync());
        }

        private async void InitializeAsync()
        {
            var devices = await DeviceInformation.FindAllAsync(GattDeviceService.GetDeviceSelectorFromUuid(new Guid("2AC94B65-C8F4-48A4-804A-C03BC6960B80")));
            foreach (DeviceInformation d in devices)
            {
                Debug.WriteLine(d.Name);
                if (!_devices.ContainsKey(d.Id))
                    _devices.Add(d.Id,new RadiusDevice(d));
            }
        }
    }
}
