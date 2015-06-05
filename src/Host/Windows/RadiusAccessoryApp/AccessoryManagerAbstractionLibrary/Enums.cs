/**************************************************************************************/
//This is sample code provided under the Microsoft Limited Public License.
// Copyright (c) Microsoft Corporation. All rights reserved 
/**************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Phone.AccessoryManager.AbstractionLayer
{
    public enum AccessoryNotificationType
    {
        None = 0,
        Phone = 1,
        Email = 2,
        Reminder = 4,
        Alarm = 8,
        Toast = 16,
        AppUninstalled = 32,
        Dnd = 64,
        DrivingMode = 128,
        BatterySaver = 256,
        Media = 512,
        CortanaTile = 1024
    }

    public enum ReminderState
    {
        Active,
        Snoozed,
        Dismissed
    }

    #region Phone Calls
    public enum PhoneNotificationType
    {
        NewCall = 0,
        CallChanged = 1,
        LineChanged = 2,
        PhoneCallAudioEndpointChanged = 3,
        PhoneMuteChanged = 4
    }

    public enum PhoneCallDirection
    {
        Incoming = 0,
        Outgoing = 1
    }

    public enum PhoneMediaType
    {
        AudioOnly = 0,
        AudioVideo = 1
    }

    public enum PhoneCallTransport
    {
        Cellular = 0,
        Voip = 1
    }

    public enum PhoneCallState
    {
        Unknown = 0,
        Ringing = 1,
        Talking = 2,
        Held = 3,
        Ended = 4
    }

    public enum CallType
    {
        Normal = 0,
        Video = 1,
        VoIP = 2
    }

    public enum PhoneLineRegistrationState
    {
        Disconnected = 0,
        Home = 1,
        Roaming = 2
    }

    public enum PhoneCallAudioEndpoint
    {
        Default = 0,
        Speaker = 1,
        Handsfree = 2
    }
    #endregion

    #region Media
    public enum PlaybackCapability
    {
        None = 0,
        Play = 0x1,
        Pause = 0x2,
        Stop = 0x4,
        Record = 0x8,
        FastForward = 0x10,
        Rewind = 0x20,
        Next = 0x40,
        Previous = 0x80,
        ChannelUp = 0x100,
        ChannelDown = 0x200
    }

    public enum PlaybackCommand
    {

        Play,
        Pause,
        Stop,
        Record,
        FastForward,
        Rewind,
        Next,
        Previous,
        ChannelUp,
        ChannelDown
    }

    public enum PlaybackStatus
    {
        None,
        TrackChanged,
        Stopped,
        Playing,
        Paused
    }
    #endregion 
}