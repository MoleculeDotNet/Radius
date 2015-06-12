using System;
using Microsoft.SPOT;
using System.Collections;

namespace IngenuityMicro.Radius
{
    public abstract class RadiusApplication : MarshalByRefObject, IRadiusApplication
    {
        private IAppHost _host;

        public RadiusApplication()
        {
        }

        public void InitializeFramework()
        {
            //TODO: Initialize DI container with drivers

            // Call app-specific initialization
            this.Initialize();
        }

        public abstract void Initialize();

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
