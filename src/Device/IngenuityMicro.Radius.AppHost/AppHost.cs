using System;
using Microsoft.SPOT;

using IngenuityMicro.Radius.Fonts;
using IngenuityMicro.Radius.Hardware;
using IngenuityMicro.Radius.Core;
using System.Collections;
using System.Threading;
using PervasiveDigital.Diagnostics;
using PervasiveDigital.Utilities;

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

        public void AddApplication(RadiusApplication app)
        {
            AddApplication(app, false);
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

        public RadiusApplication ActiveApp
        {
            get { return _activeApp; }
        }


        public void Run()
        {
            AddApplication(new AppHostApp());
            Thread.Sleep(Timeout.Infinite);
        }

        public Audio.Buzzer Buzzer { get { return _buzzer; } }
        public Ble Bluetooth { get { return _ble; } }
        public Sharp128 Display { get { return _display; } }
        public Mpu9150 Accelerometer { get { return _mpu; } }

        public void SerialDataReceived(string msg)
        {
            // currently, we get our strings already parsed out into lines
            this.MessageReceived(msg);
        }

        private void MessageReceived(string msg)
        {
            // get the target app
            if (msg[0] != '#')
            {
                //Event: Message introducer not found
                Logger.Error((AutoTag)0x0000001, LoggingCategories.Host);
            }
            var idxEnd = msg.IndexOf('#', 1);
            if (idxEnd == -1)
            {
                //Event: Target-app identifier's terminator was not found
                Logger.Error((AutoTag)0x0000001, LoggingCategories.Host);
                return;
            }
            var target = msg.Substring(1, idxEnd - 1);

            if (_apps.Contains(target))
            {
                ((RadiusApplication)_apps[target]).HandleAppMessage(msg.Substring(idxEnd + 1));
            }
        }
    }
}
