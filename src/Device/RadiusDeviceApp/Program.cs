using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using Microsoft.SPOT.Time;

using IngenuityMicro.Radius.AppHost;
using IngenuityMicro.Radius.Fonts;
using IngenuityMicro.Radius.Hardware;
using IngenuityMicro.Radius.DefaultApplications;

using PervasiveDigital.Diagnostics;

namespace RadiusDeviceApp
{
    public class Program
    {
        private static Audio.Buzzer _buzzer;
        public static Ble _ble;
        private static Sharp128 _display;
        private static Mpu9150 _mpu;
        private static AppHost _host;

        public static void Main()
        {
            Logger.MinimumSeverityToLog = Severity.Info;
#if DEBUG
            var listener = new DebugPrintListener();
            Logger.AddListener(listener);
#endif

            _display = new Sharp128();
            _buzzer = new Audio.Buzzer();

            _display.DrawBitmap(0, 0, Bmp.FaceBlack, 128, 128, true);
            _display.Render();

            //_i2CBus = new I2CDevice(null);
            //var padPress = new InterruptPort(Pin.PA4, true, Port.ResistorMode.PullUp, Port.InterruptMode.InterruptEdgeLow);
            //padPress.OnInterrupt += padPress_OnInterrupt;
            //_touch = new Mpr121Touch(_i2CBus);
            //_touch.configure();

            _ble = new Ble();
            _ble.DataReceived += _ble_DataReceived;

            _mpu = new Mpu9150(0x68, 400, 10, Cpu.Pin.GPIO_NONE);
            _mpu.Wake();
            _mpu.setFullScaleGyroRange(2);
            _mpu.setFullScaleAccelRange(2);

            //TODO: use some sort of IOC/DI to find these constructor args
            _host = new AppHost(_buzzer, _ble, _display, _mpu);
            _host.AddApplication(new AnalogClock(), true);
            _host.Run();
        }

        static void _ble_DataReceived(string val)
        {
            _host.SerialDataReceived(val);
        }
    }
}
