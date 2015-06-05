/**************************************************************************************/
//This is sample code provided under the Microsoft Limited Public License.
// Copyright (c) Microsoft Corporation. All rights reserved 
/**************************************************************************************/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage.Streams;

namespace Microsoft.Phone.AccessoryManager.AbstractionLayer
{
    /// <summary>
    /// Properties and control methods at Accessory Manager root level
    /// </summary>
    public sealed class AccessoryManager
    {
        #region private members
       
        static private bool _isAvailable;
        static private bool _isAvailableTested;

        //Adding to avoid the default compiler added public constructor
        private AccessoryManager() 
        { }
        /// <summary>
        /// Gets a value that indicates if Accessory Manager is available.
        /// </summary>
        /// <value>
        /// <c>true</c> if Accessory Manager is available; otherwise <c>false</c>.
        /// </value>
        /// <remarks>
        /// Accessory Manager is available with Windows Phone 8.1 GDR1 and later.
        /// </remarks>
        public static bool IsAvailable
        {
            get
            {
                if (!_isAvailableTested)
                {
                    try
                    {
                        var assemblyType = Type.GetType(BaseConstants.AccessoryMgrTypeName);
                        _isAvailable = assemblyType != null;
                    }
                    catch (Exception) { }
                    _isAvailableTested = true;
                }
                return _isAvailable;
            }
        }


        private static Object InvokeMethodByName(string methodName, Type[] paramsTypes, Object[] methodParams)
        {
            var assemblyType = Type.GetType(BaseConstants.AccessoryMgrTypeName);

            var methodInfo = assemblyType.GetRuntimeMethod(methodName, paramsTypes);
            Object returnValue = methodInfo.Invoke(null, methodParams);
            return returnValue;
        }

        private static Object GetPropertyValueByName(string propertyName)
        {
            var assemblyType = Type.GetType(BaseConstants.AccessoryMgrTypeName);
            PropertyInfo pInfo = assemblyType.GetRuntimeProperty(propertyName);
            Object retvalue = pInfo.GetValue(null);
            return retvalue;
        }

        /// <summary>
        /// We have to make a copy of the stream returned by AccessoryManager service
        /// This is avoid "Not enough storage" exception
        /// </summary>
        /// <param name="appId">Id of App to get icon</param>
        /// <returns>stream</returns>
        private async static Task<IRandomAccessStream> GetAppIconAsync(string appId)
        {
            InMemoryRandomAccessStream appIconStream = new InMemoryRandomAccessStream();
            IRandomAccessStreamReference retStream = retStream = InvokeMethodByName(AccessoryManagerStaticsConstants.GetAppIcon, new Type[] { typeof(string) }, new object[] { appId }) as IRandomAccessStreamReference;
            using (IRandomAccessStreamWithContentType iconStream = await retStream.OpenReadAsync())
            {
                Byte[] bytes = new Byte[iconStream.Size];
                await iconStream.AsStream().ReadAsync(bytes, 0, bytes.Length);

                using (var writer = new DataWriter(appIconStream))
                {
                    writer.WriteBytes(bytes);
                    await writer.StoreAsync();
                    await writer.FlushAsync();
                    writer.DetachStream();
                }
            }

            // DataWriter causes stream to point at the end after writing into to the stream. Point it to the start.
            appIconStream.Seek(0);

            GC.Collect();
            return appIconStream;
        }

        #endregion

        #region public members
        /// <summary>
        /// Registers an Accessory app with the Accessory Manager
        /// </summary>
        /// <returns>Trigger Id that should be used to Register DeviceManufacturerNotificationTrigger Background Task
        /// Same code:
        /// DeviceManufacturerNotificationTrigger trigger = new DeviceManufacturerNotificationTrigger("Microsoft.AccessoryManagement.Notification:" + triggerId, false);
        /// </returns>
        public static String RegisterAccessoryApp()
        {
            return InvokeMethodByName(AccessoryManagerStaticsConstants.RegisterAccessoryApp, new Type[0], null) as string;
        }


        /// <summary>
        /// Gets the next trigger detail with Accessory Manager
        /// </summary>
        public static IAccessoryNotificationTriggerDetails GetNextTriggerDetails()
        {
            Object triggerDetailsValue = InvokeMethodByName(AccessoryManagerStaticsConstants.GetNextTriggerDetails, new Type[0], null);
            IAccessoryNotificationTriggerDetails retTriggerDetails = null;
            if (triggerDetailsValue != null)
            {
                TypeInfo typeInfo = Type.GetType(BaseConstants.IAccessoryNotificationTriggerDetailsTypeName).GetTypeInfo();
                string notifTypeValue = typeInfo.GetDeclaredProperty(NotificationTriggerDetailsConstants.AccessoryNotificationType).GetValue(triggerDetailsValue).ToString();
                AccessoryNotificationType notifType = (AccessoryNotificationType)(Enum.Parse(typeof(AccessoryNotificationType), notifTypeValue, true));
                switch (notifType)
                {
                    case AccessoryNotificationType.Phone:
                        retTriggerDetails = ObjectFactory.CreatePhoneNotificationTriggerDetails(triggerDetailsValue);
                        break;
                    case AccessoryNotificationType.Email:
                        retTriggerDetails = ObjectFactory.CreateEmailNotificationTriggerDetails(triggerDetailsValue);
                        break;
                    case AccessoryNotificationType.Reminder:
                        retTriggerDetails = ObjectFactory.CreateReminderNotificationTriggerDetails(triggerDetailsValue);
                        break;
                    case AccessoryNotificationType.Alarm:
                        retTriggerDetails = ObjectFactory.CreateAlarmNotificationTriggerDetails(triggerDetailsValue);
                        break;
                    case AccessoryNotificationType.Toast:
                        retTriggerDetails = ObjectFactory.CreateToastNotificationTriggerDetails(triggerDetailsValue);
                        break;
                    case AccessoryNotificationType.AppUninstalled:
                        retTriggerDetails = ObjectFactory.CreateAppUninstallledNotificationTriggerDetails(triggerDetailsValue);
                        break;
                    case AccessoryNotificationType.Dnd:
                    case AccessoryNotificationType.DrivingMode:
                    case AccessoryNotificationType.BatterySaver:
                        retTriggerDetails = ObjectFactory.CreateNotificationTriggerDetails(triggerDetailsValue);
                        break;
                    case AccessoryNotificationType.Media:
                        retTriggerDetails = ObjectFactory.CreateMediaNotificationTriggerDetails(triggerDetailsValue);
                        break;
                    case AccessoryNotificationType.CortanaTile:
                        retTriggerDetails = ObjectFactory.CreateCortanaTileNotificationTriggerDetails(triggerDetailsValue);
                        break;
                    default:
                        break;
                }
            }
            return retTriggerDetails;
        }

        /// <summary>
        /// Sets the specified trigger detail as completed with Accessory Manager
        /// </summary>
        /// <param name="notificationTriggerDetails">Trigget detail to be marked as completed</param>
        public static void ProcessTriggerDetails(IAccessoryNotificationTriggerDetails notificationTriggerDetails)
        {
            Object rawObject = null;
            switch (notificationTriggerDetails.AccessoryNotificationType)
            {
                case AccessoryNotificationType.Phone:
                    PhoneNotificationTriggerDetails phoneObj = notificationTriggerDetails as PhoneNotificationTriggerDetails;
                    rawObject = phoneObj.GetRawObject();
                    break;
                case AccessoryNotificationType.Email:
                    EmailNotificationTriggerDetails emailObj = notificationTriggerDetails as EmailNotificationTriggerDetails;
                    rawObject = emailObj.GetRawObject();

                    break;
                case AccessoryNotificationType.Reminder:
                    ReminderNotificationTriggerDetails reminderObj = notificationTriggerDetails as ReminderNotificationTriggerDetails;
                    rawObject = reminderObj.GetRawObject();
                    break;
                case AccessoryNotificationType.Alarm:
                    AlarmNotificationTriggerDetails alarmObj = notificationTriggerDetails as AlarmNotificationTriggerDetails;
                    rawObject = alarmObj.GetRawObject();
                    break;
                case AccessoryNotificationType.Toast:
                    ToastNotificationTriggerDetails toastObj = notificationTriggerDetails as ToastNotificationTriggerDetails;
                    rawObject = toastObj.GetRawObject();
                    break;
                case AccessoryNotificationType.AppUninstalled:
                case AccessoryNotificationType.Dnd:
                case AccessoryNotificationType.DrivingMode:
                case AccessoryNotificationType.BatterySaver:
                    AccessoryNotificationTriggerDetails notifObj = notificationTriggerDetails as AccessoryNotificationTriggerDetails;
                    rawObject = notifObj.RawNotificationObject;
                    break;
                case AccessoryNotificationType.Media:
                    MediaControlsTriggerDetails mediaObj = notificationTriggerDetails as MediaControlsTriggerDetails;
                    rawObject = mediaObj.GetRawObject();
                    break;
                case AccessoryNotificationType.CortanaTile:
                    CortanaTileNotificationTriggerDetails cortanaObj = notificationTriggerDetails as CortanaTileNotificationTriggerDetails;
                    rawObject = cortanaObj.GetRawObject();
                    break;

                default:
                    break;
            }
            InvokeMethodByName(AccessoryManagerStaticsConstants.ProcessTriggerDetails, new Type[] { Type.GetType(BaseConstants.IAccessoryNotificationTriggerDetailsTypeName) }, new Object[] { rawObject });
        }

        /// <summary>
        /// Retrieves User consent value for this accessory application
        /// </summary>
        /// <returns>true/false - based on the user consent for this app</returns>
        public static bool GetUserConsent()
        {
            return bool.Parse(InvokeMethodByName(AccessoryManagerStaticsConstants.GetUserConsent, new Type[0], null).ToString());
        }


        /// <summary>
        /// Enable Notificatoins of types with the given mask
        /// Sample Code: 
        /// AccessoryManager.EnableNotificationTypes((int)NotificationType.Toast | (int)NotificationType.Phone |
        ///        (int)NotificationType.AppUninstalled | (int)NotificationType.Alarm | (int)NotificationType.Reminder | 
        ///         (int)NotificationType.Email | (int)NotificationType.Dnd | (int)NotificationType.Media
        ///        | (int)NotificationType.DrivingMode | (int)NotificationType.BatterySaver);
        /// </summary>
        /// <param name="type">Notification ast to enable. This overrides the existing mask for accessory application</param>
        public static void EnableAccessoryNotificationTypes(int notifMask)
        {
            InvokeMethodByName(AccessoryManagerStaticsConstants.EnableAccessoryNotificationTypes, new Type[] { typeof(int) }, new object[] { notifMask });         
        }

        /// <summary>
        ///Retrieves  the  Notification mask for the accessory app
        /// </summary>
        /// <returns>Current Notification mask for the app</returns>
        public static int GetEnabledAccessoryNotificationTypes()
        {
            return int.Parse(InvokeMethodByName(AccessoryManagerStaticsConstants.GetEnabledAccessoryNotificationTypes, new Type[0], new object[0]).ToString());
        }


        /// <summary>
        /// Retrieves the best possible list of toast capable apps installed on the Phone
        /// This list could be used in creating Application Selection Control Panel in the accessory app
        /// </summary>
        /// 
        /// <returns></returns>
        public static IDictionary<string, AppNotificationInfo> GetApps()
        {
            Type appNotificationType = Type.GetType(BaseConstants.AppInfoTypeName);
            var assemblyType = Type.GetType(BaseConstants.AccessoryMgrTypeName);
            MethodInfo methodInfo = assemblyType.GetRuntimeMethod(AccessoryManagerStaticsConstants.GetApps, new Type[0]);
            var returnValue = methodInfo.Invoke(null, null);
            Type retType = methodInfo.ReturnType;
            var property = retType.GetRuntimeProperty("Keys");
            var keys = (IEnumerable<string>)property.GetValue(returnValue);

            IDictionary<string, AppNotificationInfo> dictionary = new Dictionary<string, AppNotificationInfo>();
            foreach (string key in keys)
            {
                MethodInfo method = retType.GetRuntimeMethod("get_Item", new Type[] { typeof(string) });
                var appinfo = method.Invoke(returnValue, new object[] { key });
                string appId = appNotificationType.GetTypeInfo().GetDeclaredProperty(AppNotificationInfoConstants.Id).GetValue(appinfo) as string;
                string appName = appNotificationType.GetTypeInfo().GetDeclaredProperty(AppNotificationInfoConstants.Name).GetValue(appinfo) as string;

                dictionary.Add(key, new AppNotificationInfo(appId, appName));
            }

            GC.Collect();
            return dictionary;
        }

        /// <summary>
        /// Get Application Icon
        /// </summary>
        /// <param name="appId">Application Id</param>
        /// <returns></returns>
        public static IAsyncOperation<IRandomAccessStream> GetAppIcon(string appId)
        {
            return GetAppIconAsync(appId).AsAsyncOperation<IRandomAccessStream>();
        }

        /// <summary>
        /// Enable notifications for application
        /// </summary>
        /// <param name="appId">Application Id</param>
        public static void EnableNotificationsForApplication(string appId)
        {
            InvokeMethodByName(AccessoryManagerStaticsConstants.EnableNotificationsForApplication, new Type[] { typeof(string) }, new object[] { appId });  
        }

        /// <summary>
        /// Disables notifications for application
        /// For now thi just applies to Toast notifications
        /// </summary>
        /// <param name="appId">Application Id - Get this from GetApps method call</param>
        public static void DisableNotificationsForApplication(string appId)
        {
            InvokeMethodByName(AccessoryManagerStaticsConstants.DisableNotificationsForApplication, new Type[] { typeof(string) }, new object[] { appId });  
        }

        /// <summary>
        /// Disables all notifications types(Phone call, Toasts, Alarm, Reminder, E-mail etc.) for this accessory app
        /// </summary>
        public static void DisableAllAccessoryNotificationTypes()
        {
            InvokeMethodByName(AccessoryManagerStaticsConstants.DisableAllAccessoryNotificationTypes, new Type[0], new object[0]);  
        }      

        /// <summary>
        ///Accepts an incoming Phone call give an valid phoneCall Id
        ///Get the call id from PhoneNotificationTriggerDetail for an active incoming phone call
        /// </summary>
        /// <param name="phoneCallId">Phone call id to accept </param>
        public static void AcceptPhoneCall(uint phoneCallId)
        {
            InvokeMethodByName(AccessoryManagerStaticsConstants.AcceptPhoneCall, new Type[] { typeof(uint) }, new object[] { phoneCallId });  
        }

        /// <summary>
        ///End Phone call give a valid phoneCall Id
        ///Get the call id from PhoneNotificationTriggerDetail for an active incoming phone call
        /// </summary>
        /// <param name="phoneCallId">Phone call id to accept </param>
        public static void EndPhoneCall(uint phoneCallId)
        {
            InvokeMethodByName(AccessoryManagerStaticsConstants.EndPhoneCall, new Type[] { typeof(uint) }, new object[] { phoneCallId });
        }

        /// <summary>
        /// Accepts a Phone Call given a phone call id at a phone call audio endpoint
        /// </summary>
        /// <param name="phoneCallId">Phone Call id to accept</param>
        /// <param name="endPoint">Audio end point for the call once accepted</param>
        public static void AcceptPhoneCall(uint phoneCallId, PhoneCallAudioEndpoint endPoint)
        {
            InvokeMethodByName(AccessoryManagerStaticsConstants.AcceptPhoneCall, new Type[] { typeof(uint), typeof(PhoneCallAudioEndpoint) }, new object[] { phoneCallId, endPoint });
        }

        /// <summary>
        /// Accepts a Video Call
        /// </summary>
        /// <param name="phoneCallId">Phone Call id to accept</param>
        public static void AcceptPhoneCallWithVideo(uint phoneCallId)
        {
            InvokeMethodByName(AccessoryManagerStaticsConstants.AcceptPhoneCallWithVideo, new Type[] { typeof(uint) }, new object[] { phoneCallId });
        }

        /// <summary>
        /// Accepts a Video call given a call id and at a phone call audio endpoint
        /// </summary>
        /// <param name="phoneCallId">Call id to accept</param>
        /// <param name="endPoint">Audio end point</param>
        public static void AcceptPhoneCallWithVideo(uint phoneCallId, PhoneCallAudioEndpoint endPoint)
        {
            InvokeMethodByName(AccessoryManagerStaticsConstants.AcceptPhoneCallWithVideo, new Type[] { typeof(uint), typeof(PhoneCallAudioEndpoint) }, new object[] { phoneCallId, endPoint });
        }

        /// <summary>
        /// Rejects an incoming phone call followed by a text response to the caller
        /// </summary>
        /// <param name="phoneCallId">Call id to reject</param>
        /// <param name="textResponseID">Text response to send after rejecting the call</param>
        public static void RejectPhoneCall(uint phoneCallId, uint textResponseID)
        {
            InvokeMethodByName(AccessoryManagerStaticsConstants.RejectPhoneCall, new Type[] { typeof(uint), typeof(uint) }, new object[] { phoneCallId, textResponseID });
        }

        /// <summary>
        /// Rejects an incoming phone call
        /// </summary>
        /// <param name="phoneCallId">Call id to reject</param>
        public static void RejectPhoneCall(uint phoneCallId)
        {
            InvokeMethodByName(AccessoryManagerStaticsConstants.RejectPhoneCall, new Type[] { typeof(uint) }, new object[] { phoneCallId });   
        }

        /// <summary>
        /// Hold a call
        /// </summary>
        /// <param name="phoneCallId">Call id to hold</param>
        /// <param name="holdCall">true to hold, false to unhold</param>
        public static void HoldPhoneCall(uint phoneCallId, bool holdCall)
        {
            InvokeMethodByName(AccessoryManagerStaticsConstants.HoldPhoneCall, new Type[] { typeof(uint), typeof(bool) }, new object[] { phoneCallId, holdCall });
        }

        /// <summary>
        /// Make a phone call
        /// </summary>
        /// <param name="phoneLine">Phone line guid</param>
        /// <param name="phoneNumber">Phone number</param>
        public static void MakePhoneCall(Guid phoneLine, string phoneNumber)
        {
            InvokeMethodByName(AccessoryManagerStaticsConstants.MakePhoneCall, new Type[] { typeof(Guid), typeof(bool) }, new object[] { phoneLine, phoneNumber });
        }

        public static void MakePhoneCall(Guid phoneLine, string phoneNumber, PhoneCallAudioEndpoint endPoint)
        {
            InvokeMethodByName(AccessoryManagerStaticsConstants.MakePhoneCall, new Type[] { typeof(Guid), typeof(string), typeof(PhoneCallAudioEndpoint) }, new object[] { phoneLine, phoneNumber, endPoint });
        }

        /// <summary>
        /// Make a phone call with video
        /// </summary>
        /// <param name="phoneLine">Phone line guid</param>
        /// <param name="phoneNumber">Phone number</param>
        /// <param name="endPoint">Audio end point</param>
        public static void MakePhoneCallWithVideo(Guid phoneLine, string phoneNumber, PhoneCallAudioEndpoint endPoint)
        {
            InvokeMethodByName(AccessoryManagerStaticsConstants.MakePhoneCallWithVideo, new Type[] { typeof(Guid), typeof(string), typeof(PhoneCallAudioEndpoint) }, new object[] { phoneLine, phoneNumber, endPoint });
        }

        /// <summary>
        /// Make a phone call with video
        /// </summary>
        /// <param name="phoneLine">Phone line guid</param>
        /// <param name="phoneNumber">Phone number</param>
        public static void MakePhoneCallWithVideo(Guid phoneLine, string phoneNumber)
        {
            InvokeMethodByName(AccessoryManagerStaticsConstants.MakePhoneCallWithVideo, new Type[] { typeof(Guid), typeof(string) }, new object[] { phoneLine, phoneNumber });
        }

        /// <summary>
        /// Swap phone calls
        /// </summary>
        /// <param name="phoneCallIdToHold">Phone CallId To Hold</param>
        /// <param name="phoneCallIdOnHold">Phone CallId On Hold</param>
        public static void SwapPhoneCalls(uint phoneCallIdToHold, uint phoneCallIdOnHold)
        {
            InvokeMethodByName(AccessoryManagerStaticsConstants.SwapPhoneCalls, new Type[] { typeof(uint), typeof(uint) }, new object[] { phoneCallIdToHold, phoneCallIdOnHold });
        }

        /// <summary>
        /// Snoozes an active reminder for a given ammount of time
        /// </summary>
        /// <param name="reminderId">Remind to snooze - get this from IReminderNotificationTriggerDetails</param>
        /// <param name="timeSpan">Time period for snooze</param>
        public static void SnoozeReminder(Guid reminderId, TimeSpan timeSpan)
        {
            InvokeMethodByName(AccessoryManagerStaticsConstants.SnoozeReminder, new Type[] { typeof(Guid), typeof(TimeSpan) }, new object[] { reminderId, timeSpan });   
        }

        /// <summary>
        /// Snoozes an active reminder for default ammount of time
        /// </summary>
        /// <param name="reminderId">Reminder to snooze - get this from IReminderNotificationTriggerDetails</param>
        public static void SnoozeReminder(Guid reminderId)
        {
            InvokeMethodByName(AccessoryManagerStaticsConstants.SnoozeReminder, new Type[] { typeof(Guid) }, new object[] { reminderId });
        }

        /// <summary>
        /// Dismisses an active reminder 
        /// </summary>
        /// <param name="reminderId">Reminder to dismiss</param>
        public static void DismissReminder(Guid reminderId)
        {
            InvokeMethodByName(AccessoryManagerStaticsConstants.DismissReminder, new Type[] { typeof(Guid) }, new object[] { reminderId });
        }

        /// <summary>
        /// Snoozes an active alarm for default ammount of time
        /// </summary>
        /// <param name="alarmId">Alarm to snooze</param>
        public static void SnoozeAlarm(Guid alarmId)
        {
            InvokeMethodByName(AccessoryManagerStaticsConstants.SnoozeAlarm, new Type[] { typeof(Guid) }, new object[] { alarmId });
        }

        /// <summary>
        /// Snoozes an active alarm for specified time
        /// </summary>
        /// <param name="alarmId">Alarm to snooze</param>
        /// <param name="timeSpan">time to snooze</param>
        public static void SnoozeAlarm(Guid alarmId, TimeSpan timeSpan)
        {
            InvokeMethodByName(AccessoryManagerStaticsConstants.SnoozeAlarm, new Type[] { typeof(Guid), typeof(TimeSpan) }, new object[] { alarmId, timeSpan });
        }

        /// <summary>
        /// Dismisses an active Alarm
        /// </summary>
        /// <param name="alarmId"></param>
        public static void DismissAlarm(Guid alarmId)
        {
            InvokeMethodByName(AccessoryManagerStaticsConstants.DismissAlarm, new Type[] { typeof(Guid) }, new object[] { alarmId });
        }

        /// <summary>
        /// Retrieves current notification status for the given installed app to the accessory app
        /// i.e. for a notification from the given AppId whether the acceeory app would get notified or not
        /// </summary>
        /// <param name="appId">Application Id - Get this from GetApps API call</param>
        /// <returns>Notification state of the application for this accessory application</returns>
        public static bool IsNotificationEnabledForApplication(string appId)
        {
            var assemblyType = Type.GetType(BaseConstants.AccessoryMgrTypeName);

            var methodInfo = assemblyType.GetRuntimeMethod(AccessoryManagerStaticsConstants.IsNotificationEnabledForApplication, new Type[] { typeof(string) });
            bool returnValue = bool.Parse((methodInfo.Invoke(null, new Object[] { appId })).ToString());
            return returnValue;
        }

        /// <summary>
        /// Get Metadata for the current media on Phone
        /// </summary>
        /// <returns>Metadata of the media</returns>
        public static MediaMetadata GetMediaMetadata()
        {
            return ObjectFactory.CreateMediaMetadata(InvokeMethodByName(AccessoryManagerStaticsConstants.GetMediaMetadata, new Type[0], new Object[0]));
        }

        /// <summary>
        /// Invokes the playback command
        /// </summary>
        /// <param name="command"></param>
        public static void PerformMediaPlaybackCommand(PlaybackCommand command)
        {
            Type paramType = Type.GetType(BaseConstants.MediaPlaybackCommandsTypeName);
            Object paramObj = Enum.Parse(paramType, ((int)command).ToString());

            InvokeMethodByName(AccessoryManagerStaticsConstants.PerformMediaPlaybackCommand, new Type[] { paramType }, new object[] {  paramObj });
        }

        /// <summary>
        /// Gets the Phone line details for a phone line represented by phoneLine Guid
        /// </summary>
        /// <param name="phoneLine">Phne Line GUID</param>
        /// <returns>Phone line details</returns>
        public static PhoneLineDetails GetPhoneLineDetails(Guid phoneLine)
        {
            Object retObj = InvokeMethodByName(AccessoryManagerStaticsConstants.GetPhoneLineDetails, new Type[] { typeof(Guid) }, new Object[] { phoneLine });
            return ObjectFactory.CreatePhoneLineDetails(retObj);
        }

        #endregion

        #region properties

        /// <summary>
        /// Retrieves the value for Battery saver
        /// </summary>
        public static bool BatterySaverState
        {
            get
            {
                return bool.Parse(GetPropertyValueByName(AccessoryManagerStaticsConstants.BatterySaverState).ToString()); ;
            }
        }

        /// <summary>
        /// Retrieves the value for Quiet Hours(Do Not Disturb) on Phone
        /// </summary>
        public static bool DoNotDisturbEnabled
        {
            get
            {
                return bool.Parse(GetPropertyValueByName(AccessoryManagerStaticsConstants.DoNotDisturbEnabled).ToString());
            }
        }

        /// <summary>
        /// Retrieves value for driving mode on Phone
        /// </summary>
        public static bool DrivingModeEnabled
        {
            get
            {
                return bool.Parse(GetPropertyValueByName(AccessoryManagerStaticsConstants.DrivingModeEnabled).ToString());
            }
        }

        /// <summary>
        /// Retrieves the media playback capabilities
        /// </summary>
        public static PlaybackCapability MediaPlaybackCapabilities
        {
            get
            {                
                Object capsObj = GetPropertyValueByName(AccessoryManagerStaticsConstants.MediaPlaybackCapabilities);             
                return (PlaybackCapability)Enum.Parse(typeof(PlaybackCapability), capsObj.ToString());
            }
        }

        /// <summary>
        /// Retreives the current media playback status
        /// </summary>
        public static PlaybackStatus MediaPlaybackStatus
        {
            get
            {
                Object statusObj = GetPropertyValueByName(AccessoryManagerStaticsConstants.MediaPlaybackStatus);
                return (PlaybackStatus)Enum.Parse(typeof(PlaybackStatus), statusObj.ToString());
            }
        }

        public static bool PhoneMute
        {
            get
            {
                var assemblyType = Type.GetType(BaseConstants.AccessoryMgrTypeName);
                PropertyInfo propInfo = assemblyType.GetRuntimeProperty(AccessoryManagerStaticsConstants.PhoneMute);
                bool retValue = bool.Parse(propInfo.GetValue(null).ToString());
                return retValue;
            }
            set
            {
                var assemblyType = Type.GetType(BaseConstants.AccessoryMgrTypeName);
                PropertyInfo propInfo = assemblyType.GetRuntimeProperty(AccessoryManagerStaticsConstants.PhoneMute);
                propInfo.SetValue(null, value);
            }
        }

        public static PhoneCallAudioEndpoint PhoneCallAudioEndpoint
        {
            get
            {
                var assemblyType = Type.GetType(BaseConstants.AccessoryMgrTypeName);
                PropertyInfo propInfo = assemblyType.GetRuntimeProperty(AccessoryManagerStaticsConstants.PhoneCallAudioEndpoint);
                string endpointvalue = propInfo.GetValue(null).ToString();
                PhoneCallAudioEndpoint retValue = (PhoneCallAudioEndpoint)(Enum.Parse(PhoneCallAudioEndpoint.GetType(), endpointvalue));
                return retValue;
            }
            set
            {
                var assemblyType = Type.GetType(BaseConstants.AccessoryMgrTypeName);
                PropertyInfo propInfo = assemblyType.GetRuntimeProperty(AccessoryManagerStaticsConstants.PhoneCallAudioEndpoint);
                propInfo.SetValue(null, value);
            }
        }

        /// <summary>
        /// Retrieves list of Phone lines available on Phone inlucding Cellular and VoIP
        /// </summary>
        public static IReadOnlyList<PhoneLineDetails> PhoneLineDetails
        {
            get
            {
                var assemblyType = Type.GetType(BaseConstants.AccessoryMgrTypeName);
                PropertyInfo propInfo = assemblyType.GetRuntimeProperty(AccessoryManagerStaticsConstants.PhoneLineDetails);
                IReadOnlyList<Object> phoneLines = propInfo.GetValue(null) as IReadOnlyList<Object>;

                List<PhoneLineDetails> retPhoneLines = new List<PhoneLineDetails>();
                foreach (Object line in phoneLines)
                {
                    retPhoneLines.Add(ObjectFactory.CreatePhoneLineDetails(line));
                }
                return retPhoneLines;
            }
        }

        #endregion
    }
}
