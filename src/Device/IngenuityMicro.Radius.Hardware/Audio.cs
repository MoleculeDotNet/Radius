using System;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

namespace IngenuityMicro.Radius.Hardware
{
    public class Audio
    {
        public class Buzzer 
        {
            private readonly PWM _buzzer = new PWM((Cpu.PWMChannel)8, 1000, 0.5, false);
            public void Buzz(double duration = 1000, int sleep = 100)
            {
                _buzzer.Start();
                Thread.Sleep(sleep);
                _buzzer.Stop();
            }
        }
    }
}
