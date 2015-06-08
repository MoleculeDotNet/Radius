/**************************************************************************************/
//This is sample code provided under the Microsoft Limited Public License.
// Copyright (c) Microsoft Corporation. All rights reserved 
/**************************************************************************************/
using System;
using System.Reflection;

namespace Microsoft.Phone.AccessoryManager.AbstractionLayer
{
    internal class AccessoryNotificationTriggerDetails : IAccessoryNotificationTriggerDetails
    {
        internal AccessoryNotificationTriggerDetails(string appdisplayname, string appid, AccessoryNotificationType notificationtype, bool startedprocessing, DateTimeOffset timecreated, Object rawData)
        {
            AppDisplayName = appdisplayname;
            AppId = appid;
            AccessoryNotificationType = notificationtype;
            RawNotificationObject = rawData;
            StartedProcessing = startedprocessing;
            TimeCreated = timecreated;
        }

        public string AppDisplayName { get; private set; }

        public string AppId { get; private set; }

        internal Object RawNotificationObject { get; private set; }

        public AccessoryNotificationType AccessoryNotificationType { get; private set; }

        public bool StartedProcessing
        { 
            get
            {
                Type notificationType = Type.GetType(BaseConstants.IAccessoryNotificationTriggerDetailsTypeName);
                return bool.Parse(notificationType.GetTypeInfo().GetDeclaredProperty(NotificationTriggerDetailsConstants.StartedProcessing).ToString());
            }
            set
            {
                Type notificationType = Type.GetType(BaseConstants.IAccessoryNotificationTriggerDetailsTypeName);
                notificationType.GetTypeInfo().GetDeclaredProperty(NotificationTriggerDetailsConstants.StartedProcessing).SetValue(RawNotificationObject, value);
            }
        }

        public DateTimeOffset TimeCreated { get; private set; }
    }
}