/**************************************************************************************/
//This is sample code provided under the Microsoft Limited Public License.
// Copyright (c) Microsoft Corporation. All rights reserved 
/**************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Email;

namespace Microsoft.Phone.AccessoryManager.AbstractionLayer
{
    /// <summary>
    /// Class to hold Email Notification Trigger details
    /// </summary>
    public sealed class EmailNotificationTriggerDetails: IEmailNotificationTriggerDetails
    {
        private readonly IAccessoryNotificationTriggerDetails commondetails;

        public EmailNotificationTriggerDetails(IAccessoryNotificationTriggerDetails commondetails, string accountname, EmailMessage emailmessage, 
            string foldername, string senderaddress, string sendername, DateTimeOffset timestamp)
        {
            this.commondetails = commondetails;
            this.AccountName = accountname;
            this.ParentFolderName = foldername;
            this.EmailMessage = emailmessage;
            this.SenderAddress = senderaddress;
            this.SenderName = sendername;
            this.Timestamp = timestamp;
        }

        public string AccountName { get; private set; }

        public EmailMessage EmailMessage { get; private set; }

        public string ParentFolderName { get; private set; }

        public string SenderAddress { get; private set; }

        public string SenderName { get; private set; }

        public DateTimeOffset Timestamp { get; private set; }

        public string AppDisplayName { get; private set; }

        public string AppId { get; private set; }

        public AccessoryNotificationType AccessoryNotificationType
        {
            get
            {
                return commondetails.AccessoryNotificationType;
            }
        }

        public bool StartedProcessing
        {
            get
            {
                return commondetails.StartedProcessing;
            }
            set
            {
                commondetails.StartedProcessing = value;
            }
        }

        public DateTimeOffset TimeCreated
        {
            get
            {
                return commondetails.TimeCreated;
            }
        }

        internal Object GetRawObject()
        {
            return (this.commondetails as AccessoryNotificationTriggerDetails).RawNotificationObject;
        }
    }
}
