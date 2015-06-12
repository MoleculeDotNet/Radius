using System;
using Microsoft.SPOT;
using IngenuityMicro.Radius.Core;
using System.Collections;
using Microsoft.SPOT.Hardware;

namespace IngenuityMicro.Radius.AppHost
{
    class AppHostApp : RadiusApplication
    {
        public override void Initialize()
        {
        }

        public override string UniqueName
        {
            get { return "Radius"; }
        }

        public override void NavigateAway()
        {
            throw new NotSupportedException();
        }

        public override void NavigateTo()
        {
            throw new NotSupportedException();
        }

        public override string DisplayName
        {
            get { return "Radius"; }
        }

        public override bool IsVisible { get { return false; } }

        public override void HandleAppMessage(int messageId, string method, Hashtable parms)
        {
            switch (method.ToLower())
            {
                case "settime":
                    if (parms.Contains("time") && parms.Contains("tzoffset"))
                        this.SetTime(messageId, (string)parms["time"], (int)parms["tzoffset"]);
                    break;
                case "getinstalledapps":
                    GetInstalledApps(messageId);
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

            var response = new RadiusMessageResponse(messageId);
            response.Status = 0;
            this.Host.Send(response);
        }

        private void GetInstalledApps(int messageId)
        {
            var apps = this.Host.GetInstalledApps();

            var response = new RadiusMessageResponse(messageId);
            response.Parameters.Add("hosts", apps);
            response.Status = 0;
            this.Host.Send(response);
        }
    }
}
