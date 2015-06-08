using System;
using Microsoft.SPOT;
using IngenuityMicro.Radius.Core;
using IngenuityMicro.Radius.Hardware;
using System.Threading;

namespace IngenuityMicro.Radius.DefaultApplications
{
    public class MenuApp : RadiusApplication
    {
        public override string UniqueName
        {
            get { return "MainMenu"; }
        }

        public override string DisplayName
        {
            get { return "MainMenu"; }
        }

        public override void HandleAppMessage(int messageId, string method, System.Collections.Hashtable parms)
        {
        }

        public override bool IsVisible { get { return false; }}

        public override void NavigateTo()
        {
            base.NavigateTo();

            Display.ClearAll(true);
            Display.DrawText("This is a test", 0, 20, 1, false);
            Display.DrawText("This is another test", 0, 40, 1, true);
            Display.Render();

        }

        public override void NavigateAway()
        {
            base.NavigateAway();
        }
    }
}
