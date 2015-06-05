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
    /// Class to hold Alarm Notification Trigger details
    /// </summary>
    public sealed class AlarmNotificationTriggerDetails: IAlarmNotificationTriggerDetails
    {
        internal AlarmNotificationTriggerDetails(IAccessoryNotificationTriggerDetails commonDetails, Guid guid, ReminderState reminderState, DateTimeOffset dateTimeOffset, string title)
        {
            this.CommonTriggerDetails = commonDetails;
            this.AlarmId = guid;
            this.ReminderState = reminderState;
            this.Timestamp = dateTimeOffset;
            this.Title = title;
        }

        #region public properties
        public string AppDisplayName
        {
            get
            {
                return CommonTriggerDetails.AppDisplayName;
            }
        }

        public string AppId
        {
            get
            {
                return CommonTriggerDetails.AppId;
            }
        }

        public AccessoryNotificationType AccessoryNotificationType
        {
            get
            {
                return CommonTriggerDetails.AccessoryNotificationType;
            }
        }

        public bool StartedProcessing
        {
            get
            {
                return CommonTriggerDetails.StartedProcessing;
            }
            set
            {
                CommonTriggerDetails.StartedProcessing = value;
            }
        }

        public DateTimeOffset TimeCreated
        {
            get
            {
                return CommonTriggerDetails.TimeCreated;
            }
        }

      
        #endregion

        #region public properties

        public IAccessoryNotificationTriggerDetails CommonTriggerDetails { get; private set; }

        public Guid AlarmId { get; private set; }

        public ReminderState ReminderState { get; private set; }

        public string Title { get; private set; }

        public DateTimeOffset Timestamp { get; private set; }

        #endregion

        internal Object GetRawObject()
        {
            return (this.CommonTriggerDetails as AccessoryNotificationTriggerDetails).RawNotificationObject;
        }
    }
}
