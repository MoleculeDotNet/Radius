using System;
using Microsoft.SPOT;

namespace IngenuityMicro.Radius.AppHost
{
    public class Installer : IContainerInstaller
    {
        public void Install(Container container)
        {
            container.Register(typeof(IAppHost), typeof(AppHost)).AsSingleton();
        }
    }
}
