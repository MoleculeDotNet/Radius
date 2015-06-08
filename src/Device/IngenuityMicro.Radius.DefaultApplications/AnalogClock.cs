using System;
using System.Threading;
using Microsoft.SPOT;

using IngenuityMicro.Radius.Core;
using System.Collections;

namespace IngenuityMicro.Radius.DefaultApplications
{
    public class AnalogClock : RadiusApplication
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

        public AnalogClock()
        {
        }

        public override void Initialize(IAppHost host)
        {
            base.Initialize(host);
        }

        public override string UniqueName
        {
            get { return "IngenuityMicro.Radius.DefaultApplications.SimpleClock"; }
        }

        public override string DisplayName
        {
            get { return "Simple Clock"; }
        }

        public override void HandleAppMessage(int messageId, string method, Hashtable parms)
        {
        }

        private void Tick(object o)
        {
            if (!IsActiveApp)
                return;

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

        public override void NavigateAway()
        {
            _timer.Dispose();
            _timer = null;
        }

        public override void NavigateTo()
        {
            _timer = new Timer(Tick, null, 0, 1000);
        }
    }
}
