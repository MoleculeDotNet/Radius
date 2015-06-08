using System;
using Microsoft.SPOT;
using IngenuityMicro.Radius.Hardware;
using System.Collections;

namespace IngenuityMicro.Radius.Core
{
    public abstract class RadiusApplication
    {
        private IAppHost _host;

        public RadiusApplication()
        {
        }

        public virtual void Initialize(IAppHost host)
        {
            _host = host;
        }

        protected IAppHost Host { get { return _host; } }

        public abstract string UniqueName { get; }

        public abstract string DisplayName { get; }

        public abstract void HandleAppMessage(int messageId, string method, Hashtable parms);

        // Should this app show up in the app list?
        public virtual bool IsVisible { get { return true; } }

        public bool IsActiveApp
        {
            get
            {
                return _host.ActiveApp == this;
            }
        }

        public virtual void NavigateAway()
        {
        }

        public virtual void NavigateTo()
        {
        }

        public virtual void OnGesture(Gesture gesture, out bool handled)
        {
            handled = false;
        }

        public Audio.Buzzer Buzzer { get { return _host.Buzzer; } }
        public Ble Bluetooth { get{ return _host.Bluetooth; } }
        public Sharp128 Display { get { return _host.Display; } }
        public Mpu9150 Accelerometer { get { return _host.Accelerometer; } }
    }
}
