using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Ninject.Modules;

namespace IngenuityMicro.Radius.Host
{
    public class Module : NinjectModule
    {
        public override void Load()
        {
            this.Bind<RadiusDeviceEnumerator>().ToSelf().InSingletonScope();
        }
    }
}
