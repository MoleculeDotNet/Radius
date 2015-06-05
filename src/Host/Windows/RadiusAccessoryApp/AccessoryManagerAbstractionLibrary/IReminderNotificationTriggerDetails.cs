/**************************************************************************************/
//This is sample code provided under the Microsoft Limited Public License.
// Copyright (c) Microsoft Corporation. All rights reserved 
/**************************************************************************************/

using System;
using Windows.ApplicationModel.Appointments;

namespace Microsoft.Phone.AccessoryManager.AbstractionLayer
{
    public interface IReminderNotificationTriggerDetails : IAccessoryNotificationTriggerDetails
    {
        Appointment Appointment { get; }

        string Description { get; }

        string Details { get; }

        Guid ReminderId { get; }

        ReminderState ReminderState { get; }

        DateTimeOffset Timestamp { get; }

        string Title { get; }
    }
}