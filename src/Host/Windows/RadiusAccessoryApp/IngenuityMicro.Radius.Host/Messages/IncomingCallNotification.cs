using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IngenuityMicro.Radius.Host.Messages
{
    public class IncomingCallNotification : RadiusMessage
    {
        public IncomingCallNotification()
            : base("Notifications", "IncomingCallNotification")
        {
        }

        public string PhoneNumber
        {
            get
            {
                if (this.Parameters.ContainsKey("phoneNo"))
                    return (string)this.Parameters["phoneNo"];
                else
                    return null;
            }
            set
            {
                this.Parameters["phoneNo"] = value;
            }
        }

        public string ContactName
        {
            get
            {
                if (this.Parameters.ContainsKey("contact"))
                    return (string)this.Parameters["contact"];
                else
                    return null;
            }
            set
            {
                this.Parameters["contact"] = value;
            }
        }

        public int CallId
        {
            get
            {
                if (this.Parameters.ContainsKey("callId"))
                    return (int)this.Parameters["callId"];
                else
                    return 0;
            }
            set
            {
                this.Parameters["callId"] = (int)value;
            }
        }

        public override void OnResponseReceived(RadiusMessageResponse response, out bool handled)
        {
            // this message is complete only on success
            handled = false;
            if (response.Status == 0)
                handled = true;
        }
    }
}
