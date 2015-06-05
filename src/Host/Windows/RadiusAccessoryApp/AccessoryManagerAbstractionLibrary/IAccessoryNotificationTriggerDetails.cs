/**************************************************************************************/
//This is sample code provided under the Microsoft Limited Public License.
// Copyright (c) Microsoft Corporation. All rights reserved 
/**************************************************************************************/
using System;

namespace Microsoft.Phone.AccessoryManager.AbstractionLayer
{
    public interface IAccessoryNotificationTriggerDetails
    {
        string AppDisplayName { get; }

        string AppId { get; }

        AccessoryNotificationType AccessoryNotificationType { get; }

        bool StartedProcessing { get; set; }

        DateTimeOffset TimeCreated { get; }
    }
}
