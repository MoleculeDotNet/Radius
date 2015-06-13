using System;
using System.Collections;
using Microsoft.SPOT;
using IngenuityMicro.Radius;
using System.Threading;

namespace AnalogClock
{
    public class Application : RadiusApplication
    {
        private Timer _timer;
        private Int32 _hourAngle;
        private Int32 _hourX;
        private Int32 _hourY;

        private Int32 _minuteAngle;
        private Int32 _minuteX;
        private Int32 _minuteY;

        private Int32 _secAngle;
        private Int32 _secondX;
        private Int32 _secondY;

        public override void Initialize()
        {
            Debug.Print(this.UniqueName + " Initializing...");

            DiContainer.Instance.Install(
                new IngenuityMicro.Radius.Hardware.AppEnvironmentInstaller()
                );

            Debug.Print(this.UniqueName + " Initialization complete");
        }

        public override string UniqueName
        {
            get { return "AnalogClock"; } // must match pe name, for now
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
            _timer.Dispose();
            _timer = null;
        }

        public override void NavigateTo()
        {
            Display.ClearAll(false);
            Display.DrawBitmap(0, 0, Bmp.FaceBlack, 128, 128, true);
            Display.Render();

            _timer = new Timer(Tick, null, 0, 1000);
        }

        public override void OnGesture(Gesture gesture, out bool handled)
        {
            handled = false;
        }

        private void Tick(object o)
        {
            Display.DrawLine(64, 64, _hourX, _hourY, true);
            Display.DrawLine(64, 64, _minuteX, _minuteY, true);
            Display.DrawLine(64, 64, _secondX, _secondY, true);

            _hourAngle = 90 - ((DateTime.Now.Hour % 12) * 60 + DateTime.Now.Minute) / 2;
            _hourX = 64 + (Int32)((Microsoft.SPOT.Math.Cos(_hourAngle) * 30) / 1000);
            _hourY = 64 - (Int32)((Microsoft.SPOT.Math.Sin(_hourAngle) * 30) / 1000);
            Display.DrawLine(64, 64, _hourX, _hourY, false);

            _minuteAngle = 90 - (DateTime.Now.Minute * 60 + DateTime.Now.Second) / 10;
            _minuteX = 64 + (Int32)((Microsoft.SPOT.Math.Cos(_minuteAngle) * 40) / 1000);
            _minuteY = 64 - (Int32)((Microsoft.SPOT.Math.Sin(_minuteAngle) * 40) / 1000);
            Display.DrawLine(64, 64, _minuteX, _minuteY, false);

            _secAngle = 90 - (DateTime.Now.Second * 6);
            _secondX = 64 + (Int32)((Microsoft.SPOT.Math.Cos(_secAngle) * 50) / 1000);
            _secondY = 64 - (Int32)((Microsoft.SPOT.Math.Sin(_secAngle) * 50) / 1000);

            Display.DrawLine(64, 64, _secondX, _secondY, false);
            Display.Render();
        }

    }
}
