using System;
using Microsoft.SPOT;
using IngenuityMicro.Radius.Hardware;
using System.Collections;

namespace IngenuityMicro.Radius.Core
{
    public abstract class RadiusApplication : IRadiusApplication
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

        private IDisplay _display;
        public IDisplay Display
        {
            get
            {
                if (_display == null)
                {
                    _display = (IDisplay)DiContainer.Instance.Resolve(typeof(IDisplay));
                }
                return _display;
            }
        }
    }
}
