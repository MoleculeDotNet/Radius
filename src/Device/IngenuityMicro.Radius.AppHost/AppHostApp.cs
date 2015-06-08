using System;
using Microsoft.SPOT;
using IngenuityMicro.Radius.Core;
using PervasiveDigital.Diagnostics;
using System.Collections;

namespace IngenuityMicro.Radius.AppHost
{
    class AppHostApp : RadiusApplication
    {
        public override void Initialize(IAppHost host)
        {
            base.Initialize(host);
        }

        public override string UniqueName
        {
            get { return "Radius"; }
        }

        public override void NavigateAway()
        {
            base.NavigateAway();
            //Event: You cannot navigate away from this app and it should never become active.  It exists only to sink messages for the app host.
            Logger.Critical((AutoTag)0x00000000, LoggingCategories.Host);
        }

        public override void NavigateTo()
        {
            base.NavigateTo();
            //Event: You cannot navigate to this app and it should never become active. It exists only to sink messages for the app host.
            Logger.Critical((AutoTag)0x00000000, LoggingCategories.Host);
        }
        public override string DisplayName
        {
            get { return "Radius"; }
        }

        public override void HandleAppMessage(int messageId, string method, Hashtable parms)
        {
        }
    }
}
