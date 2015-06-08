using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IngenuityMicro.Radius.Host.Messages
{
    public class SetTimeMessage : RadiusMessage
    {
        public SetTimeMessage()
            : base("Radius", "SetTime")
        {
        }

        public DateTime CurrentUtcTime
        {
            get 
            {
                if (this.Parameters.ContainsKey("time"))
                    return (DateTime)this.Parameters["time"];
                else
                    return DateTime.MinValue;
            }
            set
            {
                this.Parameters["time"] = value.ToString("o");
            }
        }

        public int TzOffset
        {
            get
            {
                if (this.Parameters.ContainsKey("tzoffset"))
                    return (int)this.Parameters["tzoffset"];
                else
                    return 0;
            }
            set
            {
                this.Parameters["tzoffset"] = value;
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
