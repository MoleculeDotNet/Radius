/**************************************************************************************/
//This is sample code provided under the Microsoft Limited Public License.
// Copyright (c) Microsoft Corporation. All rights reserved 
/**************************************************************************************/

namespace Microsoft.Phone.AccessoryManager.AbstractionLayer
{
    public interface ICortanaTileNotificationTriggerDetails : IAccessoryNotificationTriggerDetails
    {
        string Content { get; }

        string EmphasizedText { get; }

        string LargeContent1 { get; }

        string LargeContent2 { get; }

        string NonWrappedSmallContent1 { get; }

        string NonWrappedSmallContent2 { get; }

        string NonWrappedSmallContent3 { get; }

        string NonWrappedSmallContent4 { get; }

        string Source { get; }

        string TileId { get; }
    }
}