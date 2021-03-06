using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using Microsoft.SPOT.Time;

using IngenuityMicro.Radius;
using IngenuityMicro.Radius.AppHost;
using IngenuityMicro.Radius.Fonts;
using IngenuityMicro.Radius.Hardware;

using IngenuityMicro.Radius.Core;
using NetMF.IO;

namespace RadiusDeviceApp
{
    public class Program
    {
        //private static Audio.Buzzer _buzzer;
        //private static Mpu9150 _mpu;
        private static AppHost _host;
        private static I2CDevice _i2CBus;
        private static Mpr121Touch _touch;
        private static InterruptPort _sw1 = new InterruptPort((Cpu.Pin)45, true, Port.ResistorMode.PullUp, Port.InterruptMode.InterruptEdgeBoth);
        private static InterruptPort _sw2 = new InterruptPort((Cpu.Pin)16, true, Port.ResistorMode.PullUp, Port.InterruptMode.InterruptEdgeBoth);
        //private static InterruptPort _sw3 = new InterruptPort(Pin.PA13, true, Port.ResistorMode.PullUp, Port.InterruptMode.InterruptEdgeLow);

        public static void Main()
        {
            Debug.EnableGCMessages(true);

            DiContainer.Instance.Install(
                new IngenuityMicro.Radius.Hardware.HostEnvironmentInstaller(),
                new IngenuityMicro.Radius.AppHost.Installer()
                );

            _sw1.OnInterrupt += _sw1_OnInterrupt;
            _sw2.OnInterrupt += _sw2_OnInterrupt;
            //_sw3.OnInterrupt += _sw3_OnInterrupt;

            //_buzzer = new Audio.Buzzer();

            _i2CBus = new I2CDevice(null);
            var padPress = new InterruptPort(Pin.PA4, true, Port.ResistorMode.PullUp, Port.InterruptMode.InterruptEdgeLow);
            padPress.OnInterrupt += padPress_OnInterrupt;
            _touch = new Mpr121Touch(_i2CBus);
            _touch.configure();

            //TODO: The init fails here if the touch device is initialize - need to investigate why that is.
            //_mpu = new Mpu9150(0x68, 400, 10, Cpu.Pin.GPIO_NONE);
            //_mpu.Wake();
            //_mpu.setFullScaleGyroRange(2);
            //_mpu.setFullScaleAccelRange(2);

            var fileSystem = (IFileSystem)DiContainer.Instance.Resolve(typeof(IFileSystem));
            fileSystem.Initialize();
            if (!fileSystem.IsFormatted)
            {
                fileSystem.Format(); // takes quite awhile
            }
            fileSystem.Mount();

            var channel = (IPeerChannel)DiContainer.Instance.Resolve(typeof(IPeerChannel));
            channel.DataReceived += channel_DataReceived;

            //TODO: use some sort of IOC/DI to find these constructor args
            _host = (AppHost)DiContainer.Instance.Resolve(typeof(IAppHost));
            _host.Run();
        }

        static void channel_DataReceived(string val)
        {
            _host.SerialDataReceived(val);
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
    }
}
