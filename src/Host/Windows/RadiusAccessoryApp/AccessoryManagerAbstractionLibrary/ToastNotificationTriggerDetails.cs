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
    /// Class to hold Toast Notification Trigger details
    /// </summary>
    public class ToastNotificationTriggerDetails : IAccessoryNotificationTriggerDetails
    {
        internal ToastNotificationTriggerDetails(IAccessoryNotificationTriggerDetails commonDetails, bool suppress, string text1, string text2, string text3, string text4)
        {
            this.commonTriggerDetails = commonDetails;
            this.SuppressPopup = suppress;
            this.Text1 = text1;
            this.Text2 = text2;
            this.Text3 = text3;
            this.Text4 = text4;
        }

        private IAccessoryNotificationTriggerDetails commonTriggerDetails;

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

        public IAccessoryNotificationTriggerDetails CommonTriggerDetails
        {
            get { return commonTriggerDetails; }
        }

        public bool SuppressPopup { get; private set; }

        public string Text1 { get; private set; }

        public string Text2 { get; private set; }

        public string Text3 { get; private set; }

        public string Text4 { get; private set; }

        internal Object GetRawObject()
        {
            return (this.commonTriggerDetails as AccessoryNotificationTriggerDetails).RawNotificationObject;
        }
    }
}
