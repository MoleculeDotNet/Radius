using System;
using Microsoft.SPOT;

using IngenuityMicro.Radius.Fonts;
using IngenuityMicro.Radius.Hardware;
using IngenuityMicro.Radius.Core;
using System.Collections;
using System.Threading;

namespace IngenuityMicro.Radius.AppHost
{
    public class AppHost : IAppHost
    {
        private Audio.Buzzer _buzzer;
        private Ble _ble;
        private Sharp128 _display;
        private Mpu9150 _mpu;
        private Hashtable _apps = new Hashtable();
        private RadiusApplication _defaultApp = null;
        private RadiusApplication _activeApp = null;

        public AppHost(Audio.Buzzer buzzer, Ble ble, Sharp128 display, Mpu9150 mpu)
        {
            _buzzer = buzzer;
            _ble = ble;
            _display = display;
            _mpu = mpu;
        }

        public void AddApplication(RadiusApplication app, bool isDefaultApp)
        {
            if (_apps.Contains(app.UniqueName))
                _apps.Remove(app.UniqueName);
            _apps.Add(app.UniqueName, app);
            if (isDefaultApp)
                _defaultApp = app;
            app.Initialize(this);
            if (isDefaultApp)
                SwitchTo(app);
        }

        public void SwitchTo(RadiusApplication app)
        {
            if (_activeApp != null)
                _activeApp.NavigateAway();
            _activeApp = app;
            _activeApp.NavigateTo();
        }

        public bool IsActiveApp(RadiusApplication app)
        {
            return _activeApp == app;
        }


        public void Run()
        {
            Thread.Sleep(Timeout.Infinite);
        }

        public Audio.Buzzer Buzzer { get { return _buzzer; } }
        public Ble Bluetooth { get { return _ble; } }
        public Sharp128 Display { get { return _display; } }
        public Mpu9150 Accelerometer { get { return _mpu; } }

    }
}
