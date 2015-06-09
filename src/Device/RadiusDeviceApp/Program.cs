using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using Microsoft.SPOT.Time;

using IngenuityMicro.Radius;
using IngenuityMicro.Radius.AppHost;
using IngenuityMicro.Radius.Fonts;
using IngenuityMicro.Radius.Hardware;
using IngenuityMicro.Radius.DefaultApplications;

using PervasiveDigital.Diagnostics;
using IngenuityMicro.Radius.Core;
using Microsoft.SPOT.IO;
using NetMF.IO;

namespace RadiusDeviceApp
{
    public class Program
    {
        private static Audio.Buzzer _buzzer;
        public static Ble _ble;
        private static Sharp128 _display;
        private static Mpu9150 _mpu;
        private static AppHost _host;
        private static I2CDevice _i2CBus;
        private static Mpr121Touch _touch;
        private static TinyFileSystem _tfs;
        private static InterruptPort _sw1 = new InterruptPort((Cpu.Pin)45, true, Port.ResistorMode.PullUp, Port.InterruptMode.InterruptEdgeBoth);
        private static InterruptPort _sw2 = new InterruptPort((Cpu.Pin)16, true, Port.ResistorMode.PullUp, Port.InterruptMode.InterruptEdgeBoth);
        //private static InterruptPort _sw3 = new InterruptPort(Pin.PA13, true, Port.ResistorMode.PullUp, Port.InterruptMode.InterruptEdgeLow);

        public static void Main()
        {
            Logger.MinimumSeverityToLog = Severity.Info;
#if DEBUG
            var listener = new DebugPrintListener();
            Logger.AddListener(listener);
#endif

            _sw1.OnInterrupt += _sw1_OnInterrupt;
            _sw2.OnInterrupt += _sw2_OnInterrupt;
            //_sw3.OnInterrupt += _sw3_OnInterrupt;

            _display = new Sharp128();
            //_buzzer = new Audio.Buzzer();

            _i2CBus = new I2CDevice(null);
            var padPress = new InterruptPort(Pin.PA4, true, Port.ResistorMode.PullUp, Port.InterruptMode.InterruptEdgeLow);
            padPress.OnInterrupt += padPress_OnInterrupt;
            _touch = new Mpr121Touch(_i2CBus);
            _touch.configure();

            _ble = new Ble();
            _ble.DataReceived += _ble_DataReceived;

            //TODO: The init fails here if the touch device is initialize - need to investigate why that is.
            //_mpu = new Mpu9150(0x68, 400, 10, Cpu.Pin.GPIO_NONE);
            //_mpu.Wake();
            //_mpu.setFullScaleGyroRange(2);
            //_mpu.setFullScaleAccelRange(2);

#if ENABLE_FILESYSTEM
            _tfs = Flash.SetUpTfs();
            if (!_tfs.CheckIfFormatted())
            {
                _tfs.Format(); // takes quite awhile
            }
            _tfs.Mount();
#endif

            //TODO: use some sort of IOC/DI to find these constructor args
            _host = new AppHost(_buzzer, _ble, _display, _mpu, _tfs);
            _host.AddApplication(new AnalogClock(), true);
            _host.AddApplication(new MenuApp());
            _host.AddApplication(new NotificationApp());
            _host.Run();
        }

        static void _sw1_OnInterrupt(uint data1, uint data2, DateTime time)
        {
            // Menu switch pressed - display the main menu
            _host.LaunchMainMenu();
        }

        static void _sw2_OnInterrupt(uint data1, uint data2, DateTime time)
        {
            // Dismiss button pressed
            bool handled;
            _host.ActiveApp.OnGesture(Gesture.Dismiss, out handled);
            if (!handled)
            {
                // The app didn't have anything it could dismiss. We should handle this at the framework level
                _host.PopAndSwitch();
            }
        }

        static void _sw3_OnInterrupt(uint data1, uint data2, DateTime time)
        {
        }

        static void padPress_OnInterrupt(uint data1, uint data2, DateTime time)
        {
            GestureManager.TouchDetected(_touch.GetPadTouch());
        }

        internal static void DispatchGesture(Gesture gesture)
        {
            bool handled;
            _host.ActiveApp.OnGesture(gesture, out handled);
            if (!handled)
            {
                // Handle according to host semantics
            }
        }

        static void _ble_DataReceived(string val)
        {
            _host.SerialDataReceived(val);
        }
    }
}
