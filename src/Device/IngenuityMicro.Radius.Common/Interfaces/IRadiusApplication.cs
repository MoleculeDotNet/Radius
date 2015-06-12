using System;
using Microsoft.SPOT;
using System.Collections;

namespace IngenuityMicro.Radius
{
    public interface IRadiusApplication
    {
        void Initialize();

        string UniqueName { get; }

        string DisplayName { get; }

        void HandleAppMessage(int messageId, string method, Hashtable parms);

        bool IsVisible { get; }

        bool IsActiveApp { get; }

        void NavigateAway();

        void NavigateTo();

        void OnGesture(Gesture gesture, out bool handled);
    }
}
