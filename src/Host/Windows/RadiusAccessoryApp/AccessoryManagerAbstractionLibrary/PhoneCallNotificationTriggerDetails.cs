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
    /// <summary>
    /// Class to hold Phone Notification Trigger details
    /// </summary>
    public class PhoneNotificationTriggerDetails : IAccessoryNotificationTriggerDetails
    {
        internal PhoneNotificationTriggerDetails(IAccessoryNotificationTriggerDetails commonDetails, PhoneCallDetails callDetails, Guid phoneLineChangedId, PhoneNotificationType phoneNotificationType)
        {
            this.commontriggerDetails = commonDetails;
            this.CallDetails = callDetails;
            this.PhoneLineChangedId = phoneLineChangedId;
            this.PhoneNotificationType = phoneNotificationType;
        }

        private readonly IAccessoryNotificationTriggerDetails commontriggerDetails;

        public string AppDisplayName
        {
            get
            {
                return commontriggerDetails.AppDisplayName;
            }
        }

        public string AppId
        {
            get
            { 
                return commontriggerDetails.AppId;
            }
        }

        public AccessoryNotificationType AccessoryNotificationType
        {
            get
            {
                return commontriggerDetails.AccessoryNotificationType;
            }
        }

        public bool StartedProcessing
        {
            get
            {               
                return commontriggerDetails.StartedProcessing;
            }
            set
            {               
                commontriggerDetails.StartedProcessing = value;
            }
        }

        public DateTimeOffset TimeCreated
        {
            get
            {                
                return commontriggerDetails.TimeCreated;
            }
        }

        public IAccessoryNotificationTriggerDetails CommonTriggerDetails
        {
            get { return commontriggerDetails; }           
        }

        public PhoneCallDetails CallDetails { get; private set; }

        public Guid PhoneLineChangedId { get; private set; }

        public PhoneNotificationType PhoneNotificationType { get; private set; }

        internal Object GetRawObject()
        {
            return (this.commontriggerDetails as AccessoryNotificationTriggerDetails).RawNotificationObject;
        }
        
    }
}
