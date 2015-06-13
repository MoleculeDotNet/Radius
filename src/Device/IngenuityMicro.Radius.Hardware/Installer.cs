using System;
using Microsoft.SPOT;

namespace IngenuityMicro.Radius.Hardware
{
    public class HostEnvironmentInstaller : IContainerInstaller
    {
        public void Install(Container container)
        {
            container.Register(typeof(IPeerChannel), typeof(PeerChannel)).AsSingleton();
            container.Register(typeof(IFileSystem), typeof(FlashFileSystem)).AsSingleton();
        }
    }

    public class AppEnvironmentInstaller : IContainerInstaller
    {
        public void Install(Container container)
        {
            container.Register(typeof(IDisplay), typeof(Sharp128)).AsSingleton();
            container.Register(typeof(IFileSystem), typeof(FlashFileSystem)).AsSingleton();
        }
    }
}
