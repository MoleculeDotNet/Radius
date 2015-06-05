/**************************************************************************************/
//This is sample code provided under the Microsoft Limited Public License.
// Copyright (c) Microsoft Corporation. All rights reserved 
/**************************************************************************************/
using System;

namespace Microsoft.Phone.AccessoryManager.AbstractionLayer
{
    /// <summary>
    /// Class to hold Media Notification Trigger details
    /// </summary>
    public class MediaControlsTriggerDetails : IMediaControlsTriggerDetails
    {
        private IAccessoryNotificationTriggerDetails commonDetails;

        internal MediaControlsTriggerDetails(IAccessoryNotificationTriggerDetails commondetails, PlaybackStatus status, MediaMetadata mediametadata)
        {
            this.commonDetails = commondetails;
            this.PlaybackStatus = status;
            this.MediaMetadata = mediametadata;
        }

        public IMediaMetadata MediaMetadata { get; private set; }

        public PlaybackStatus PlaybackStatus { get; private set; }

        public string AppDisplayName
        {
            get
            {
                return commonDetails.AppDisplayName;
            }
        }

        public string AppId
        {
            get
            {
                return commonDetails.AppId;
            }
        }

        public AccessoryNotificationType AccessoryNotificationType
        {
            get
            {
                return commonDetails.AccessoryNotificationType;
            }
        }

        public bool StartedProcessing
        {
            get
            {
                return commonDetails.StartedProcessing;
            }
            set
            {
                commonDetails.StartedProcessing = value;
            }
        }

        public DateTimeOffset TimeCreated
        {
            get
            {
                return commonDetails.TimeCreated;
            }
        }

        internal Object GetRawObject()
        {
            return (this.commonDetails as AccessoryNotificationTriggerDetails).RawNotificationObject;
        }
    }
}