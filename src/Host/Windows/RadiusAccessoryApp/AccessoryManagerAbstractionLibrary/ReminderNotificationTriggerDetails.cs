/**************************************************************************************/
//This is sample code provided under the Microsoft Limited Public License.
// Copyright (c) Microsoft Corporation. All rights reserved 
/**************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Appointments;

namespace Microsoft.Phone.AccessoryManager.AbstractionLayer
{
    /// <summary>
    /// Class to hold Reminder Notification Trigger details
    /// </summary>
    public class ReminderNotificationTriggerDetails : IAccessoryNotificationTriggerDetails
    {
        #region Public properties

        public string AppDisplayName
        {
            get
            {
                return commonTriggerDetails.AppDisplayName;
            }
        }

        public string AppId
        {
            get
            {
                return commonTriggerDetails.AppId;
            }
        }

        public AccessoryNotificationType AccessoryNotificationType
        {
            get
            {
                return commonTriggerDetails.AccessoryNotificationType;
            }
        }

        public bool StartedProcessing
        {
            get
            {
                return commonTriggerDetails.StartedProcessing;
            }
            set
            {
                commonTriggerDetails.StartedProcessing = value;
            }
        }

        public DateTimeOffset TimeCreated
        {
            get
            {
                return commonTriggerDetails.TimeCreated;
            }
        }

       
        #endregion

        #region public properties
        private readonly IAccessoryNotificationTriggerDetails commonTriggerDetails;

        public IAccessoryNotificationTriggerDetails CommonTriggerDetails
        {
            get
            {                
                return commonTriggerDetails;
            }
            
        }

        public Windows.ApplicationModel.Appointments.Appointment Appointment { get; private set; }

        public string Description { get; private set; }

        public string Details { get; private set; }

        public Guid ReminderId { get; private set; }

        public ReminderState ReminderState { get; private set; }

        public DateTimeOffset Timestamp { get; private set; }

        public string Title { get; private set; }

        internal ReminderNotificationTriggerDetails(IAccessoryNotificationTriggerDetails commonDetails, Windows.ApplicationModel.Appointments.Appointment appointment,
            string description, string details, Guid reminderId, ReminderState reminderState, DateTimeOffset timeStamp, string title)
        {
            this.commonTriggerDetails = commonDetails;
            this.Appointment = appointment;
            this.Description = description;
            this.Details = details;
            this.ReminderId = reminderId;
            this.ReminderState = reminderState;
            this.Timestamp = timeStamp;
            this.Title = title;
        }

        internal Object GetRawObject()
        {
            return (this.commonTriggerDetails as AccessoryNotificationTriggerDetails).RawNotificationObject;
        }

        #endregion
    }
}
