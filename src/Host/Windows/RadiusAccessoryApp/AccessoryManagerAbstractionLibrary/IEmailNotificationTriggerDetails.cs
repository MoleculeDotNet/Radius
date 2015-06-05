/**************************************************************************************/
//This is sample code provided under the Microsoft Limited Public License.
// Copyright (c) Microsoft Corporation. All rights reserved 
/**************************************************************************************/

using System;
using Windows.ApplicationModel.Email;

namespace Microsoft.Phone.AccessoryManager.AbstractionLayer
{
    public interface IEmailNotificationTriggerDetails : IAccessoryNotificationTriggerDetails
    {
        string AccountName { get; }

        EmailMessage EmailMessage { get; }

        string ParentFolderName { get; }

        string SenderAddress { get; }

        string SenderName { get; }

        DateTimeOffset Timestamp { get; }
    }
}