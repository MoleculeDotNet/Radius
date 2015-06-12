using System;
using System.Collections;
using Microsoft.SPOT;
using IngenuityMicro.Radius;

namespace MainMenu
{
    public class Application : RadiusApplication
    {
        public override void Initialize()
        {
            Debug.Print(this.UniqueName + " Initializing...");
        }

        public override string UniqueName
        {
            get { return "MainMenu"; }  // must match pe name for now
        }

        public override string DisplayName
        {
            get { return "Main Menu"; }
        }

        public override void HandleAppMessage(int messageId, string method, Hashtable parms)
        {
        }

        public override void NavigateAway()
        {
            base.NavigateAway();
        }

        public override void NavigateTo()
        {
            base.NavigateTo();
            Display.ClearAll(true);
            Display.DrawText("This is a test", 0, 20, 1, false);
            Display.DrawText("This is another test", 0, 40, 1, true);
            Display.Render();
        }

        public override void OnGesture(Gesture gesture, out bool handled)
        {
            handled = false;
        }
    }
}
