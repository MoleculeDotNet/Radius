using System;
using Microsoft.SPOT;
using IngenuityMicro.Radius.Core;

namespace IngenuityMicro.Radius.DefaultApplications
{
    public class NotificationApp : RadiusApplication
    {
        public override string UniqueName
        {
            get { return "Notifications"; }
        }

        public override string DisplayName
        {
            get { return "Notifications"; }
        }

        public override void HandleAppMessage(int messageId, string method, System.Collections.Hashtable parms)
        {
        }

        public override bool IsVisible { get { return false; } }

        public override void NavigateTo()
        {
            base.NavigateTo();
        }

        public override void NavigateAway()
        {
            base.NavigateAway();
        }
    }
}
