using System;
using Microsoft.SPOT;
using IngenuityMicro.Radius.Core;
using PervasiveDigital.Diagnostics;
using System.Collections;
using Microsoft.SPOT.Hardware;

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
            switch (method.ToLower())
            {
                case "settime":
                    if (parms.Contains("time") && parms.Contains("tzoffset"))
                        this.SetTime(messageId, (string)parms["time"], (int)parms["tzoffset"]);
                    break;
                case "getinstalledapps":
                    break;
                default:
                    break;
            }
        }

        private void SetTime(int messageId, string timeStr, int tzOffset)
        {
            int year = int.Parse(timeStr.Substring(0, 4));
            int month = int.Parse(timeStr.Substring(5, 2));
            int day = int.Parse(timeStr.Substring(8, 2));
            int hour = int.Parse(timeStr.Substring(11, 2));
            int minute = int.Parse(timeStr.Substring(14, 2));
            double second = double.Parse(timeStr.Substring(17, timeStr.Length - 17 - 1));
            int iSecond = (int)System.Math.Truncate(second);
            int iMilli = (int)System.Math.Truncate((second - iSecond) * 1000.0);
            var now = new DateTime(year, month, day, hour, minute, iSecond, iMilli);
            now = now.AddMinutes(tzOffset);  //HACK : we should set the local timezone info so that the watch can handle tz/DST transitions by itself
            Utility.SetLocalTime(now);
        }
    }
}
