/**************************************************************************************/
//This is sample code provided under the Microsoft Limited Public License.
// Copyright (c) Microsoft Corporation. All rights reserved 
/**************************************************************************************/
using System;
using System.Collections.Generic;

namespace Microsoft.Phone.AccessoryManager.AbstractionLayer
{
    public interface IPhoneCallDetails
    {
        PhoneCallDirection CallDirection { get; }

        uint CallId { get; }

        PhoneMediaType CallMediaType { get; }

        PhoneCallTransport CallTransport { get; }

        uint ConferenceCallId { get; }

        string ContactName { get; }

        DateTimeOffset EndTime { get; }

        Guid PhoneLine { get; }

        string PhoneNumber { get; }

        IReadOnlyList<ITextResponse> PresetTextResponses { get; }

        DateTimeOffset StartTime { get; }

        PhoneCallState State { get; }
    }
}