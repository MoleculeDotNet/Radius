using System;
using Microsoft.SPOT;

using IngenuityMicro.Radius.Hardware;

namespace IngenuityMicro.Radius.Core
{
    public interface IAppHost
    {
        void SwitchTo(RadiusApplication app);

        RadiusApplication ActiveApp { get; }

        Audio.Buzzer Buzzer { get; }
        
        Ble Bluetooth { get; }
        
        Sharp128 Display { get; }
        
        Mpu9150 Accelerometer { get; }
    }
}
