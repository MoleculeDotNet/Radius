using System;
using Microsoft.SPOT;

namespace IngenuityMicro.Radius
{
    public interface IAppHost
    {
        void SwitchTo(IRadiusApplication app);

        IRadiusApplication ActiveApp { get; }

        void Send(IRadiusMessage msg);

        string[] GetInstalledApps();
    }
}
