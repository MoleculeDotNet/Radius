/**************************************************************************************/
//This is sample code provided under the Microsoft Limited Public License.
// Copyright (c) Microsoft Corporation. All rights reserved 
/**************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Phone.AccessoryManager.AbstractionLayer
{    
    public  class PhoneCallDetails
    {
        internal PhoneCallDetails(PhoneCallDirection callDirection, uint callId, PhoneMediaType callMediaType, PhoneCallTransport callTransport, uint conferenceCallId, String contactName,
            DateTimeOffset endTime, Guid phoneLine, String phoneNumber, IReadOnlyList<TextResponse> presetTextResponses, DateTimeOffset startTime, PhoneCallState state)
        {
            this.CallDirection = callDirection;
            this.CallId = callId;
            this.CallMediaType = callMediaType;
            this.CallTransport = callTransport;
            this.ConferenceCallId = conferenceCallId;
            this.ContactName = contactName;
            this.EndTime = endTime;
            this.PhoneLine = phoneLine;
            this.PhoneNumber = phoneNumber;
            this.PresetTextResponses = presetTextResponses;
            this.StartTime = startTime;
            this.State = state;
        }

        public PhoneCallDirection CallDirection { get; private set; }

        public uint CallId { get; private set; }

        public PhoneMediaType CallMediaType { get; private set; }

        public PhoneCallTransport CallTransport { get; private set; }
         
        public uint ConferenceCallId { get; private set; }
        
        public String ContactName { get; private set; }
        
        public DateTimeOffset EndTime { get; private set; }
        
        public Guid PhoneLine { get; private set; }
        
        public String PhoneNumber { get; private set; }
        
        public IReadOnlyList<TextResponse> PresetTextResponses { get; private set; }

        public DateTimeOffset StartTime { get; private set; }

        public PhoneCallState State { get; private set; }
    }
}
