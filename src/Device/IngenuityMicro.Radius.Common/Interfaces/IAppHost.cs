using System;
using Microsoft.SPOT;

namespace IngenuityMicro.Radius
{
    public interface IAppHost
    {
        bool SwitchTo(string appOd);

        IRadiusApplication ActiveApp { get; }

        void Send(IRadiusMessage msg);

        string[] GetInstalledApps();
    }
}
