/**************************************************************************************/
//This is sample code provided under the Microsoft Limited Public License.
// Copyright (c) Microsoft Corporation. All rights reserved 
/**************************************************************************************/

namespace Microsoft.Phone.AccessoryManager.AbstractionLayer
{
    public interface IToastNotificationTriggerDetails : IAccessoryNotificationTriggerDetails
    {
        bool SuppressPopup { get; }

        string Text1 { get; }

        string Text2 { get; }

        string Text3 { get; }

        string Text4 { get; }
    }
}