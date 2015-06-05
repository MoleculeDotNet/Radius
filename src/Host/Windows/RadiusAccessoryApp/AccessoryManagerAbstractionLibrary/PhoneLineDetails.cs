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
    public class PhoneLineDetails 
    {
        internal PhoneLineDetails(bool defaultOutgoingLine, String displayName, Guid lineId, String lineNumber, PhoneLineRegistrationState registrationState, uint voicemailCount)
        {
            this.DefaultOutgoingLine = defaultOutgoingLine;
            this.DisplayName = displayName;
            this.LineId = lineId;
            this.LineNumber = lineNumber;
            this.RegistrationState = registrationState;
            this.VoicemailCount = voicemailCount;
        }

        public bool DefaultOutgoingLine { get; private set; }

        public String DisplayName { get; private set; }

        public Guid LineId { get; private set; }

        public String LineNumber { get; private set; }

        public PhoneLineRegistrationState RegistrationState { get; private set; }

        public uint VoicemailCount { get; private set; }
    }
}
