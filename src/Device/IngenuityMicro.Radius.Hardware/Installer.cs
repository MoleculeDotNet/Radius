using System;
using Microsoft.SPOT;

namespace IngenuityMicro.Radius.Hardware
{
    public class Installer : IContainerInstaller
    {
        public void Install(Container container)
        {
            container.Register(typeof(IDisplay), typeof(Sharp128)).AsSingleton();
            container.Register(typeof(IPeerChannel), typeof(PeerChannel)).AsSingleton();
        }
    }
}
