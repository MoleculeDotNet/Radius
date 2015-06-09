using System;
using Microsoft.SPOT;
using System.Threading;
using IngenuityMicro.Radius.Core;

namespace RadiusDeviceApp
{
    public static class GestureManager
    {
        private const int MaxDelay = 100;
        private const int Period = 25;
        private static long _lastTick;
        private static int[] _seq = new int[10];
        private static int _idx = 0;
        private static Timer _timer;

        static GestureManager()
        {
            _timer = new Timer(timer_Tick, null, 0, Period);
        }

        private static void timer_Tick(object obj)
        {
            var now = Environment.TickCount;
            if (_lastTick != 0 && (now - _lastTick > MaxDelay))
            {
                if (_idx > 2)
                {
                    var sig = ComputeGestureSignature();
                    switch (sig)
                    {
                        case 1077: // 8 9 10 - down stroke on left
                            Program.DispatchGesture(Gesture.PageUp);
                            break;
                        case 1317: // 10 9 8 - up stroke on left
                            Program.DispatchGesture(Gesture.PageDown);
                            break;
                        case 13:
                            Program.DispatchGesture(Gesture.PageRight);
                            break;
                        case 253:
                            Program.DispatchGesture(Gesture.PageLeft);
                            break;
                        default:
                            Debug.Print("Unknown gesture sig : " + sig);
                            break;
                    }
                }

                _lastTick = 0;
                _idx = 0;
                Array.Clear(_seq, 0, _seq.Length);
                return;
            }
        }

        private static int ComputeGestureSignature()
        {
            // base 11!
            int accum = 0;

            // smooth out the data by removing items that change the direction detected in the first two elements
            var diff = _seq[0] - _seq[1];

            int i = 1;
            while (i < _idx)
            {
                // this will remove changes in direction and duplicates that arose from previous deletions
                if ((_seq[i - 1] - _seq[i]) != diff)
                {
                    for (var k = i; k < _idx - 1; ++k)
                    {
                        _seq[k] = _seq[k + 1];
                    }
                    _idx--;
                }
                else
                    ++i;
            }
            if (_idx < 3)
                return 0;

            int iAddCount = 0;
            for (int j = 0; j < _idx; ++j)
            {
                accum = (accum * 11) + _seq[j];
                if (++iAddCount == 5) // max sequence length
                    break;
            }

            return accum;
        }

        public static void TouchDetected(int pad)
        {
            if (pad == -1)
                return;

            _lastTick = Environment.TickCount;
            if (_idx==_seq.Length)
            {
                // shift, saving only the most recent values
                for (var i = 0; i < _seq.Length-1; ++i)
                    _seq[i] = _seq[i + 1];
                --_idx;
            }
            // don't record repeats
            if (_idx==0 || _seq[_idx-1]!=pad)
                _seq[_idx++] = pad;
        }
    }
}
