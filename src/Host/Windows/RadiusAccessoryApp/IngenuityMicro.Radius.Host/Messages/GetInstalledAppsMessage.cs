using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IngenuityMicro.Radius.Host.Messages
{
    public class GetInstalledAppsMessage : RadiusMessage
    {
        public GetInstalledAppsMessage(RadiusDevice device)
            : base(device, "Radius", "GetInstalledApps")
        {
        }

        public override void OnResponseReceived(RadiusMessageResponse response, out bool handled)
        {
            // this message is complete only on success
            handled = false;
            if (response.Status != 0)
                return;

            // Update the app list
        }
    }
}
