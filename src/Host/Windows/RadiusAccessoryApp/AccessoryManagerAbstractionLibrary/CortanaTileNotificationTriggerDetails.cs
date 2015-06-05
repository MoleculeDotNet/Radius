/**************************************************************************************/
//This is sample code provided under the Microsoft Limited Public License.
// Copyright (c) Microsoft Corporation. All rights reserved 
/**************************************************************************************/

using System;
namespace Microsoft.Phone.AccessoryManager.AbstractionLayer
{
    public sealed class CortanaTileNotificationTriggerDetails : ICortanaTileNotificationTriggerDetails
    {
        internal CortanaTileNotificationTriggerDetails(IAccessoryNotificationTriggerDetails commonDetails, string content, string emText, string large1, string large2, string nonwrapped1, string nonwrapped2
            ,string nonwrapped3, string nonwrapped4, string source, string tileId)
        {
            this.commonDetails = commonDetails;
            this.Content = content;
            this.EmphasizedText = emText;
            this.LargeContent1 = large1;
            this.LargeContent2 = large2;
            this.NonWrappedSmallContent1 = nonwrapped1;
            this.NonWrappedSmallContent2 = nonwrapped2;
            this.NonWrappedSmallContent3 = nonwrapped3;
            this.NonWrappedSmallContent4 = nonwrapped4;
            this.Source = source;
            this.TileId = tileId;
        }
        
        internal Object GetRawObject()
        {
            return (this.commonDetails as AccessoryNotificationTriggerDetails).RawNotificationObject;
        }
        
        private IAccessoryNotificationTriggerDetails commonDetails;

        public AccessoryNotificationType AccessoryNotificationType
        {
            get
            {
                return commonDetails.AccessoryNotificationType;
            }
        }

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

        public string Content {  get; private set; }

        public string EmphasizedText { get; private set; }

        public string LargeContent1 { get; private set;  }

        public string LargeContent2 { get; private set; }

        public string NonWrappedSmallContent1 { get; private set; }

        public string NonWrappedSmallContent2 { get; private set; }

        public string NonWrappedSmallContent3 { get; private set; }

        public string NonWrappedSmallContent4 { get; private set; }

        public string Source { get; private set; }

        public string TileId { get; private set; }
        public bool StartedProcessing
        {
            get
            {
                return commonDetails.StartedProcessing;
            }
            set
            {
                commonDetails.StartedProcessing = true;
            }
        }

        public DateTimeOffset TimeCreated
        {
            get
            {
                return commonDetails.TimeCreated;
            }
        }
    }
}