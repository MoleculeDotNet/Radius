using System;
using Microsoft.SPOT;

namespace IngenuityMicro.Radius
{
    public static class DiContainer
    {
        private static readonly Container _instance;

        static DiContainer()
        {
            _instance = new Container();
        }

        public static Container Instance { get { return _instance; } }
    }
}
