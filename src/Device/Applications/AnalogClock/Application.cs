using System;
using System.Collections;
using Microsoft.SPOT;
using IngenuityMicro.Radius;

namespace AnalogClock
{
    public class Application : RadiusApplication
    {
        public override void Initialize(IAppHost host)
        {
            Debug.Print(this.UniqueName + " Initializing...");
        }

        public override string UniqueName
        {
            get { return this.GetType().FullName; }
        }

        public override string DisplayName
        {
            get { return "Analog Clock"; }
        }

        public override void HandleAppMessage(int messageId, string method, Hashtable parms)
        {
        }

        public override void NavigateAway()
        {
        }

        public override void NavigateTo()
        {
        }

        public override void OnGesture(Gesture gesture, out bool handled)
        {
            handled = false;
        }
    }
}
