/**************************************************************************************/
//This is sample code provided under the Microsoft Limited Public License.
// Copyright (c) Microsoft Corporation. All rights reserved 
/**************************************************************************************/ 

using System;

namespace Microsoft.Phone.AccessoryManager.AbstractionLayer
{
    /// <summary>
    /// All reflection resources needed for accessing Accessory Manager APIs
    /// Essentially all strings with various class in Windows.Phone.Notification.Management.AccessoryManager 
    /// </summary>
    #region BaseConstants
    public sealed class BaseConstants
    {
        public static String AccessoryMgrNameSpaceTypeName
        {
            get
            {
                return "Windows.Phone.Notification.Management";
            }
        }

        public static String AccessoryMgrTypeName
        {
            get
            {
                return AccessoryMgrNameSpaceTypeName + ".AccessoryManager" + WinRTTypeNameSuffix; 
            }
        }        

        public static String AppInfoTypeName
        {
            get
            {
                return BaseConstants.AccessoryMgrNameSpaceTypeName + ".IAppNotificationInfo" + WinRTTypeNameSuffix;
            }
        }

        public static String AccessoryNotificationTypeTypeName
        {
            get
            {
                return BaseConstants.AccessoryMgrNameSpaceTypeName + ".AccessoryNotificationType" + WinRTTypeNameSuffix;
            }
        }

        public static String IMediaMetaDataTypeName
        {
            get
            {
                return BaseConstants.AccessoryMgrNameSpaceTypeName + ".IMediaMetadata" + WinRTTypeNameSuffix;
            }
        }

        public static String ReminderStateTypeName
        {
            get
            {
                return BaseConstants.AccessoryMgrNameSpaceTypeName + ".ReminderState" + WinRTTypeNameSuffix;
            }
        }

        public static String PhoneCallDirectionTypeName
        {
            get
            {
                return BaseConstants.AccessoryMgrNameSpaceTypeName + ".PhoneCallDirection" + WinRTTypeNameSuffix;
            }
        }

        public static String PhoneMediaTypeName
        {
            get
            {
                return BaseConstants.AccessoryMgrNameSpaceTypeName + ".PhoneMediaType" + WinRTTypeNameSuffix;
            }
        }

        public static String PhoneCallTransportTypeName
        {
            get
            {
                return BaseConstants.AccessoryMgrNameSpaceTypeName + ".PhoneCallTransport" + WinRTTypeNameSuffix;
            }
        }

        public static String PhoneCallState
        {
            get
            {
                return BaseConstants.AccessoryMgrNameSpaceTypeName + ".PhoneCallState" + WinRTTypeNameSuffix;
            }
        }

        public static String PhoneLineRegistrationStateTypeName
        {
            get
            {
                return BaseConstants.AccessoryMgrNameSpaceTypeName + ".PhoneLineRegistrationState" + WinRTTypeNameSuffix;
            }
        }

        public static String MediaPlayBackStatus
        {
            get
            {
                return BaseConstants.AccessoryMgrNameSpaceTypeName + ".MediaPlaybackStatus" + WinRTTypeNameSuffix;
            }
        }

        public static String PlaybackStatus
        {
            get
            {
                return BaseConstants.AccessoryMgrNameSpaceTypeName + ".PlaybackStatus" + WinRTTypeNameSuffix;
            }
        }

        public static String MediaPlaybackCapabilitiesTypeName
        {
            get
            {
                return BaseConstants.AccessoryMgrNameSpaceTypeName + ".PlaybackCapability" + WinRTTypeNameSuffix;
            }
        }

        public static String IPhoneLineDetails
        {
            get
            {
                return BaseConstants.AccessoryMgrNameSpaceTypeName + ".IPhoneLineDetails" + WinRTTypeNameSuffix;
            }
        }

        public static String IPhoneCallDetails
        {
            get
            {
                return BaseConstants.AccessoryMgrNameSpaceTypeName + ".IPhoneCallDetails" + WinRTTypeNameSuffix;
            }
        }

        public static String ITextResponse
        {
            get
            {
                return BaseConstants.AccessoryMgrNameSpaceTypeName + ".ITextResponse" + WinRTTypeNameSuffix;
            }
        }

        #region NotificationTriggerDetailTypes 
        public static String IAccessoryNotificationTriggerDetailsTypeName
        {
            get
            {
                return BaseConstants.AccessoryMgrNameSpaceTypeName + ".IAccessoryNotificationTriggerDetails" + WinRTTypeNameSuffix;
            }
        }

        public static String ToastNotificationTypeName
        {
            get
            {
                return BaseConstants.AccessoryMgrNameSpaceTypeName + ".ToastNotificationTriggerDetails" + WinRTTypeNameSuffix;
            }
        }

        public static String AlarmNotificationTypeName
        {
            get
            {
                return BaseConstants.AccessoryMgrNameSpaceTypeName + ".AlarmNotificationTriggerDetails" + WinRTTypeNameSuffix;
            }
        }

        public static String EmailNotificationTypeName
        {
            get
            {
                return BaseConstants.AccessoryMgrNameSpaceTypeName + ".EmailNotificationTriggerDetails" + WinRTTypeNameSuffix;
            }
        }

        public static String PhoneNotificationTriggerDetailsTypeName
        {
            get
            {
                return BaseConstants.AccessoryMgrNameSpaceTypeName + ".PhoneNotificationTriggerDetails" + WinRTTypeNameSuffix;
            }
        }

        public static String PhoneNotificationTypeTypeName
        {
            get
            {
                return BaseConstants.AccessoryMgrNameSpaceTypeName + ".PhoneNotificationType" + WinRTTypeNameSuffix;
            }
        }

        public static String ReminderNotificationTypeName
        {
            get
            {
                return BaseConstants.AccessoryMgrNameSpaceTypeName + ".ReminderNotificationTriggerDetails" + WinRTTypeNameSuffix;
            }
        }

        public static String MediaNotificationTypeName
        {
            get
            {
                return BaseConstants.AccessoryMgrNameSpaceTypeName + ".MediaControlsTriggerDetails" + WinRTTypeNameSuffix;
            }
        }

        public static String MediaPlaybackCommandsTypeName
        {
            get
            {
                return BaseConstants.AccessoryMgrNameSpaceTypeName + ".PlaybackCommand" + WinRTTypeNameSuffix;
            }
        }

        public static String CortanaNotificationTypeName
        {
            get
            {
                return BaseConstants.AccessoryMgrNameSpaceTypeName + ".CortanaTileNotificationTriggerDetails" + WinRTTypeNameSuffix;
            }
        }
        
        #endregion

        public static String AccessoryTriggerQualifierPrefix
        {
            get
            {
                return "Microsoft.AccessoryManagement.Notification:";
            }
        }

        public static String WinRTTypeNameSuffix
        {
            get
            {
                return ", Windows, ContentType=WindowsRuntime";
            }
        }  
    }
    #endregion

    #region AccessoryManagerStatics
    public sealed class AccessoryManagerStaticsConstants
    {
        internal static string GetInstance = "GetInstance";
        internal static string GetUserConsent = "GetUserConsent";
        internal static string BatterySaverState = "BatterySaverState";
        internal static string DoNotDisturbEnabled = "DoNotDisturbEnabled";
        internal static string DrivingModeEnabled = "DrivingModeEnabled";
        internal static string MediaPlaybackCapabilities = "MediaPlaybackCapabilities";
        internal static string MediaPlaybackStatus = "MediaPlaybackStatus";
        internal static string GetNextTriggerDetails = "GetNextTriggerDetails";
        internal static string PhoneCallAudioEndpoint = "PhoneCallAudioEndpoint";
        internal static string PhoneLineDetails = "PhoneLineDetails";
        internal static string PhoneMute = "PhoneMute";
        internal static string AcceptPhoneCall = "AcceptPhoneCall";
        internal static string AcceptPhoneCallWithVideo = "AcceptPhoneCallWithVideo";
        internal static string DisableAllAccessoryNotificationTypes = "DisableAllAccessoryNotificationTypes";
        internal static string DisableNotificationsForApplication = "DisableNotificationsForApplication";
        internal static string DismissAlarm = "DismissAlarm";
        internal static string DismissReminder = "DismissReminder";
        internal static string EnableNotificationsForApplication = "EnableNotificationsForApplication";
        internal static string EnableAccessoryNotificationTypes = "EnableAccessoryNotificationTypes";
        internal static string EndPhoneCall = "EndPhoneCall";
        internal static string GetAppIcon = "GetAppIcon";
        internal static string GetApps = "GetApps";
        internal static string GetEnabledAccessoryNotificationTypes = "GetEnabledAccessoryNotificationTypes";
        internal static string GetMediaMetadata = "GetMediaMetadata";
        internal static string GetMediaPlaybackCapabilities = "GetMediaPlaybackCapabilities";
        internal static string GetPhoneLineDetails = "GetPhoneLineDetails";
        internal static string HoldPhoneCall = "HoldPhoneCall";
        internal static string IsNotificationEnabledForApplication = "IsNotificationEnabledForApplication";
        internal static string MakePhoneCall = "MakePhoneCall";
        internal static string MakePhoneCallWithVideo = "MakePhoneCallWithVideo";
        internal static string PerformMediaPlaybackCommand = "PerformMediaPlaybackCommand";
        internal static string ProcessTriggerDetails = "ProcessTriggerDetails";
        internal static string RegisterAccessoryApp = "RegisterAccessoryApp";
        internal static string RejectPhoneCall = "RejectPhoneCall";
        internal static string SnoozeAlarm = "SnoozeAlarm";
        internal static string SnoozeReminder = "SnoozeReminder";
        internal static string SwapPhoneCalls = "SwapPhoneCalls";
    }
    #endregion

    #region AppNotificationInfo
    public sealed class AppNotificationInfoConstants
    {
        public static string Id
        {
            get
            {
                return "Id";
            }
        }

        public static string Name
        {
            get
            {
                return "Name";
            }
        }
    }
    #endregion

    #region NotificationTriggerDetailsConstants
    public sealed class NotificationTriggerDetailsConstants
    {
        private static readonly String appDisplayNameStr = "AppDisplayName";

        public static String AppDisplayName
        {
            get
            {
                return appDisplayNameStr;
            }
        }

        private static readonly String appIdStr = "AppId";

        public static String AppId
        {
            get
            {
                return appIdStr;
            }
        }

        public static String AccessoryNotificationType
        {
            get
            {
                return "AccessoryNotificationType";
            }
        }

        private static readonly String startedProcessingStr = "StartedProcessing";

        public static String StartedProcessing
        {
            get
            {
                return startedProcessingStr;
            }
        }

        private static readonly String timeCreatedStr = "TimeCreated";

        public static String TimeCreated
        {
            get
            {
                return timeCreatedStr;
            }
        }
    }
    #endregion

    #region ToastTriggerDetailsConstants
    public sealed class ToastTriggerDetailsConstants
    {
        public static String SuppressPopup
        {
            get
            {
                return "SuppressPopup";
            }
        }

        public static String Text1
        {
            get
            {
                return "Text1";

            }
        }

        public static String Text2
        {
            get
            {
                return "Text2";

            }
        }


        public static String Text3
        {
            get
            {
                return "Text3";
            }
        }

        public static String Text4
        {
            get
            {
                return "Text4";
            }
        }
    }
    #endregion

    #region PhoneTriggerDetailsConstants
    public sealed class PhoneTriggerDetailsConstants
    {
         public static String CallDetails
        {
            get { return "CallDetails"; }
        }

        private static readonly String phoneLineChangedIdStr = "PhoneLineChangedId";

        public static String PhoneLineChangedId
        {
            get { return phoneLineChangedIdStr; }
        }

        private static readonly String phoneNotificationTypeStr = "PhoneNotificationType";

        public static String PhoneNotificationType
        {
            get { return phoneNotificationTypeStr; }
        }
      
    }
    #endregion

    #region ReminderTriggerDetailsConstants
    public sealed class ReminderTriggerDetailsConstants
    {
        public static string Appointment
        {
            get
            {
                return "Appointment";
            }
        }

        public static string Description
        {
            get
            {
                return "Description";
            }
        }

        public static string Details
        {
            get
            {
                return "Details";
            }
        }

        public static string ReminderId
        {
            get
            {
                return "ReminderId";
            }
        }

        public static string ReminderState
        {
            get
            {
                return "ReminderState";
            }
        }

        public static string Timestamp
        {
            get
            {
                return "Timestamp";
            }
        }

        public static string Title
        {
            get
            {
                return "Title";
            }
        }
    }
#endregion 

    #region mediametadata
    public sealed class MediaMetadataConstants
    {
        public static string Album
        {
            get
            {
                return "Album";
            }
        }

        public static string Artist
        {
            get
            {
                return "Artist";
            }
        }

        public static string Duration
        {
            get
            {
                return "Duration";
            }
        }

        public static string Subtitle
        {
            get
            {
                return "Subtitle";
            }
        }

        public static string Thumbnail
        {
            get
            {
                return "Thumbnail";
            }
        }

        public static string Title
        {
            get
            {
                return "Title";
            }
        }

        public static string Track
        {
            get
            {
                return "Track";
            }
        }
    }
    #endregion

    #region AlarmNotificationTriggerDetailsConstants
    public sealed class AlarmNotificationTriggerDetailsConstants
    {
        public static string AlarmId
        {
            get
            {
                return "AlarmId";
            }
        }

        public static string ReminderState
        {
            get
            {
                return "ReminderState";
            }
        }

        public static string Timestamp
        {
            get
            {
                return "Timestamp";
            }
        }

        public static string Title
        {
            get
            {
                return "Title";
            }
        }
    }
    #endregion 

    #region EmailNotificationTriggerDetails
    public sealed class EmailNotificationTriggerDetailsConstants
    {
        public static string AccountName
        {
            get
            {
                return "AccountName";
            }
        }

        public static string EmailMessage
        {
            get
            {
                return "EmailMessage";
            }
        }

        public static string ParentFolderName
        {
            get
            {
                return "ParentFolderName";
            }
        }

        public static string SenderAddress
        {
            get
            {
                return "SenderAddress";
            }
        }

        public static string SenderName
        {
            get
            {
                return "SenderName";
            }
        }

        public static string Timestamp
        {
            get
            {
                return "Timestamp";
            }
        }
    }
    #endregion
    
    #region  MediaControlTriggerDetailsConstants
    public sealed class MediaControlTriggerDetailsConstants
    {
        public static string PlaybackStatus
        {
            get
            {
                return "PlaybackStatus";
            }
        }

        public static string MediaMetadata
        {
            get
            {
                return "MediaMetadata";
            }
        }
    }
    #endregion

    #region PhoneLineDetailsConstants
    public sealed class PhoneLineDetailsConstants
    {
        public static string DefaultOutgoingLine
        {
            get
            {
                return "DefaultOutgoingLine";
            }
        }

        public static string DisplayName
        {
            get
            {
                return "DisplayName";
            }
        }

        public static string LineId
        {
            get
            {
                return "LineId";
            }
        }

        public static string LineNumber
        {
            get
            {
                return "LineNumber";
            }
        }

        public static string RegistrationState
        {
            get
            {
                return "RegistrationState";
            }
        }

        public static string VoicemailCount
        {
            get
            {
                return "VoicemailCount";
            }
        }
    }
    #endregion

    #region TextResponseConstants
    public sealed class TextResponseConstants
    {
        public static String Content
        {
            get { return "Content"; }
        }

        public static String Id
        {
            get { return "Id"; }
        }
    }
    #endregion

    #region PhoneCallDetailsConstants
    public sealed class PhoneCallDetailsConstants
    {
        public static String CallDirection
        {
            get { return "CallDirection"; }
        }

        public static String CallId
        {
            get { return "CallId"; }
        }

        public static String CallMediaType
        {
            get { return "CallMediaType"; }
        }

        public static String CallTransport
        {
            get { return "CallTransport"; }
        }

        public static String ConferenceCallId
        {
            get { return "ConferenceCallId"; }
        }

        public static String ContactName
        {
            get { return "ContactName"; }
        }

        public static String EndTime
        {
            get { return "EndTime"; }
        }

        public static String PhoneLine
        {
            get { return "PhoneLine"; }
        }

        public static String PhoneNumber
        {
            get { return "PhoneNumber"; }
        }

        public static String PresetTextResponses
        {
            get { return "PresetTextResponses"; }
        }

        public static String StartTime
        {
            get { return "StartTime"; }
        }

        public static String State
        {
            get { return "State"; }
        }
    }
    #endregion

    #region CortanaTileNotificationTriggerDetailsConstants
    public sealed class CortanaTileNotificationTriggerDetailsConstants
    {
        public static String Content
        {
            get
            {
                return "Content";
            }
        }

        public static String EmphasizedText
        {
            get
            {
                return "EmphasizedText";

            }
        }

        public static String LargeContent1
        {
            get
            {
                return "LargeContent1";

            }
        }


        public static String LargeContent2
        {
            get
            {
                return "LargeContent2";
            }
        }

        public static String NonWrappedSmallContent1
        {
            get
            {
                return "NonWrappedSmallContent1";
            }
        }

        public static String NonWrappedSmallContent2
        {
            get
            {
                return "NonWrappedSmallContent2";
            }
        }

        public static String NonWrappedSmallContent3
        {
            get
            {
                return "NonWrappedSmallContent3";
            }
        }

        public static String NonWrappedSmallContent4
        {
            get
            {
                return "NonWrappedSmallContent4";
            }
        }

        public static String Source
        {
            get
            {
                return "Source";
            }
        }

        public static String TileId
        {
            get
            {
                return "TileId";
            }
        }
    }
    #endregion
}
