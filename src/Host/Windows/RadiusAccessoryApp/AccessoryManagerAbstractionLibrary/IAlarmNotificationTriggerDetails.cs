/**************************************************************************************/
//This is sample code provided under the Microsoft Limited Public License.
// Copyright (c) Microsoft Corporation. All rights reserved 
/**************************************************************************************/
using System;

namespace Microsoft.Phone.AccessoryManager.AbstractionLayer
{
    public interface IAlarmNotificationTriggerDetails : IAccessoryNotificationTriggerDetails
    {
        Guid AlarmId { get; }

        ReminderState ReminderState { get; }

        DateTimeOffset Timestamp { get; }

        string Title { get; }
    }
}