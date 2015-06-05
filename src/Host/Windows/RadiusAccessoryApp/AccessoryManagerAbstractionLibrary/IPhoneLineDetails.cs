/**************************************************************************************/
//This is sample code provided under the Microsoft Limited Public License.
// Copyright (c) Microsoft Corporation. All rights reserved 
/**************************************************************************************/
using System;

namespace Microsoft.Phone.AccessoryManager.AbstractionLayer
{
    public interface IPhoneLineDetails
    {
        bool DefaultOutgoingLine { get; }

        string DisplayName { get; }

        Guid LineId { get; }

        string LineNumber { get; }

        PhoneLineRegistrationState RegistrationState { get; }

        uint VoicemailCount { get; }
    }
}