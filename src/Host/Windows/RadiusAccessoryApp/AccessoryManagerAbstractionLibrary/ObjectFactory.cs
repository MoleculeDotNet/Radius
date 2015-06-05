/**************************************************************************************/
//This is sample code provided under the Microsoft Limited Public License.
// Copyright (c) Microsoft Corporation. All rights reserved 
/**************************************************************************************/

using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Storage.Streams;
using Windows.ApplicationModel.Email;
using Windows.ApplicationModel.Appointments;

namespace Microsoft.Phone.AccessoryManager.AbstractionLayer
{
    public sealed class ObjectFactory
    {
        #region Public Static methods

        /// <summary>
        /// Used internally to create
        /// Notification Trigger details object.
        /// </summary>
        /// <param name="triggerDetails">INotificationTriggerDetails object</param>
        /// <returns>IAccessoryNotificationTriggerDetails</returns>
        public static IAccessoryNotificationTriggerDetails CreateNotificationTriggerDetails(/*INotificationTriggerDetails*/Object triggerDetails)
        {
            Type notificationType = Type.GetType(BaseConstants.IAccessoryNotificationTriggerDetailsTypeName);

            var property = notificationType.GetTypeInfo().GetDeclaredProperty(NotificationTriggerDetailsConstants.AppDisplayName);
            var appDisplayName = property.GetValue(triggerDetails) as string;

            property = notificationType.GetTypeInfo().GetDeclaredProperty(NotificationTriggerDetailsConstants.AppId);
            string appId = property.GetValue(triggerDetails) as string;

            string notifTypeValue = notificationType.GetTypeInfo().GetDeclaredProperty(NotificationTriggerDetailsConstants.AccessoryNotificationType).GetValue(triggerDetails).ToString();
            AccessoryNotificationType notifType = (AccessoryNotificationType)(Enum.Parse(typeof(AccessoryNotificationType), notifTypeValue, true));

            property = notificationType.GetTypeInfo().GetDeclaredProperty(NotificationTriggerDetailsConstants.StartedProcessing);
            bool startedProcessing = bool.Parse(property.GetValue(triggerDetails).ToString());

            property = notificationType.GetTypeInfo().GetDeclaredProperty(NotificationTriggerDetailsConstants.TimeCreated);
            DateTimeOffset timeCreated = DateTimeOffset.Parse(property.GetValue(triggerDetails).ToString());

            IAccessoryNotificationTriggerDetails commonDetails = new AccessoryNotificationTriggerDetails(appDisplayName, appId, notifType, startedProcessing, timeCreated, triggerDetails);         
            return commonDetails;
        }

        /// <summary>
        /// Used internally to create
        /// Toast Notification Trigger details object.
        /// </summary>
        /// <param name="toastDetails">ToastNotificationTriggerDetails object</param>
        /// <returns>ToastNotificationTriggerDetails object</returns>
        public static ToastNotificationTriggerDetails CreateToastNotificationTriggerDetails(/*ToastNotificationTriggerDetails*/Object toastDetails)
        {
            Type notificationType = Type.GetType(BaseConstants.ToastNotificationTypeName);
            IAccessoryNotificationTriggerDetails commonDetails = CreateNotificationTriggerDetails(toastDetails);

            var property = notificationType.GetTypeInfo().GetDeclaredProperty(ToastTriggerDetailsConstants.SuppressPopup);
            bool supressPopup = bool.Parse(property.GetValue(toastDetails).ToString());

            property = notificationType.GetTypeInfo().GetDeclaredProperty(ToastTriggerDetailsConstants.Text1);
            string text1 = property.GetValue(toastDetails) as string;

            property = notificationType.GetTypeInfo().GetDeclaredProperty(ToastTriggerDetailsConstants.Text2);
            string text2 = property.GetValue(toastDetails) as string;

            property = notificationType.GetTypeInfo().GetDeclaredProperty(ToastTriggerDetailsConstants.Text3);
            string text3 = property.GetValue(toastDetails) as string;

            property = notificationType.GetTypeInfo().GetDeclaredProperty(ToastTriggerDetailsConstants.Text4);
            string text4 = property.GetValue(toastDetails) as string;

            ToastNotificationTriggerDetails details = new ToastNotificationTriggerDetails(commonDetails, supressPopup, text1, text2, text3, text4);
            return details;
        }

        /// <summary>
        /// Used internally to create
        /// Toast Notification Trigger details object.
        /// </summary>
        /// <param name="alarmDetails">IAlarmNotificationTriggerDetails object</param>
        /// <returns>AlarmNotificationTriggerDetails object</returns>
        public static AlarmNotificationTriggerDetails CreateAlarmNotificationTriggerDetails(/*IAlarmNotificationTriggerDetails*/Object alarmDetails)
        {
            IAccessoryNotificationTriggerDetails commonDetails = CreateNotificationTriggerDetails(alarmDetails);

            Type alarmNotificationType = Type.GetType(BaseConstants.AlarmNotificationTypeName);

            string notifTypeValue = alarmNotificationType.GetTypeInfo().GetDeclaredProperty(AlarmNotificationTriggerDetailsConstants.ReminderState).GetValue(alarmDetails).ToString();
            ReminderState state = (ReminderState)(Enum.Parse(Type.GetType(BaseConstants.ReminderStateTypeName), notifTypeValue, true));
            
            var property = alarmNotificationType.GetTypeInfo().GetDeclaredProperty(AlarmNotificationTriggerDetailsConstants.AlarmId);
            Guid alarmId = Guid.Parse(property.GetValue(alarmDetails).ToString());

            property = alarmNotificationType.GetTypeInfo().GetDeclaredProperty(AlarmNotificationTriggerDetailsConstants.Timestamp);
            DateTimeOffset timeStamp = DateTimeOffset.Parse(property.GetValue(alarmDetails).ToString());

            property = alarmNotificationType.GetTypeInfo().GetDeclaredProperty(AlarmNotificationTriggerDetailsConstants.Title);
            string title = property.GetValue(alarmDetails) as string;

            AlarmNotificationTriggerDetails details = new AlarmNotificationTriggerDetails(commonDetails, alarmId, state, timeStamp, title);
            return details;
        }

        /// <summary>
        /// Used internally to create
        /// Phone Notification Trigger details object.
        /// </summary>
        /// <param name="triggerDetails"></param>
        /// <returns>PhoneNotificationTriggerDetails object</returns>
        public static PhoneNotificationTriggerDetails CreatePhoneNotificationTriggerDetails(Object triggerDetails)
        {
            IAccessoryNotificationTriggerDetails commonDetails = ObjectFactory.CreateNotificationTriggerDetails(triggerDetails);

            TypeInfo phoneTypeInfo = Type.GetType(BaseConstants.PhoneNotificationTriggerDetailsTypeName).GetTypeInfo();

            var property = phoneTypeInfo.GetDeclaredProperty(PhoneTriggerDetailsConstants.CallDetails);
            Object callDetailsObj = property.GetValue(triggerDetails);
            PhoneCallDetails callDetails = (callDetailsObj == null) ? null : ObjectFactory.CreateCallDetails(callDetailsObj);

            property = phoneTypeInfo.GetDeclaredProperty(PhoneTriggerDetailsConstants.PhoneLineChangedId);
            Guid phonelinechangeid = Guid.Parse(property.GetValue(triggerDetails).ToString());

            property = phoneTypeInfo.GetDeclaredProperty(PhoneTriggerDetailsConstants.PhoneNotificationType);
            string notificationtypevalue = property.GetValue(triggerDetails).ToString();
            PhoneNotificationType phonenotificationtype = (PhoneNotificationType)(Enum.Parse(Type.GetType(BaseConstants.PhoneNotificationTypeTypeName), notificationtypevalue, true));
            
            PhoneNotificationTriggerDetails phoneTriggerDetails = new PhoneNotificationTriggerDetails(commonDetails, callDetails, phonelinechangeid, phonenotificationtype);
            return phoneTriggerDetails;
        }

        /// <summary>
        /// Used internally to create
        /// Phone cll details details object.
        /// </summary>
        /// <param name="callDetailsObj">IPhoneCallDetails object</param>
        /// <returns>PhoneCallDetails object</returns>
        public static PhoneCallDetails CreateCallDetails(object callDetailsObj)
        {
            TypeInfo callDetailsTypeInfo = Type.GetType(BaseConstants.IPhoneCallDetails).GetTypeInfo();

            var property = callDetailsTypeInfo.GetDeclaredProperty(PhoneCallDetailsConstants.CallDirection);
            string calldirectionvalue = property.GetValue(callDetailsObj).ToString();
            PhoneCallDirection phonecalldirection = (PhoneCallDirection)(Enum.Parse(Type.GetType(BaseConstants.PhoneCallDirectionTypeName), calldirectionvalue, true));

            property = callDetailsTypeInfo.GetDeclaredProperty(PhoneCallDetailsConstants.CallId);
            uint callid = uint.Parse(property.GetValue(callDetailsObj).ToString());

            property = callDetailsTypeInfo.GetDeclaredProperty(PhoneCallDetailsConstants.CallMediaType);
            string mediavalue = property.GetValue(callDetailsObj).ToString();
            PhoneMediaType callmediatype = (PhoneMediaType)(Enum.Parse(Type.GetType(BaseConstants.PhoneMediaTypeName), mediavalue, true));

            property = callDetailsTypeInfo.GetDeclaredProperty(PhoneCallDetailsConstants.CallTransport);
            string transportvalue = property.GetValue(callDetailsObj).ToString();
            PhoneCallTransport calltransport = (PhoneCallTransport)(Enum.Parse(Type.GetType(BaseConstants.PhoneCallTransportTypeName), transportvalue, true));

            property = callDetailsTypeInfo.GetDeclaredProperty(PhoneCallDetailsConstants.ConferenceCallId);
            uint confcallid = uint.Parse(property.GetValue(callDetailsObj).ToString());

            property = callDetailsTypeInfo.GetDeclaredProperty(PhoneCallDetailsConstants.ContactName);
            string contactname = property.GetValue(callDetailsObj).ToString();

            property = callDetailsTypeInfo.GetDeclaredProperty(PhoneCallDetailsConstants.EndTime);
            DateTimeOffset endtime = DateTimeOffset.Parse(property.GetValue(callDetailsObj).ToString());

            property = callDetailsTypeInfo.GetDeclaredProperty(PhoneCallDetailsConstants.PhoneLine);
            Guid phonelineid = Guid.Parse(property.GetValue(callDetailsObj).ToString());

            property = callDetailsTypeInfo.GetDeclaredProperty(PhoneCallDetailsConstants.PhoneNumber);
            string phonenumber = property.GetValue(callDetailsObj).ToString();

            TypeInfo textResponseTypeInfo = Type.GetType(BaseConstants.ITextResponse).GetTypeInfo();
            List<TextResponse> presetTextResponses = new List<TextResponse>();
            property = callDetailsTypeInfo.GetDeclaredProperty(PhoneCallDetailsConstants.PresetTextResponses);
            var textresponses = (IEnumerable<object>)property.GetValue(callDetailsObj, null);
            foreach (var responseObj in textresponses)
            {
                uint id = uint.Parse(textResponseTypeInfo.GetDeclaredProperty(TextResponseConstants.Id).GetValue(responseObj).ToString());
                string content = textResponseTypeInfo.GetDeclaredProperty(TextResponseConstants.Content).GetValue(responseObj).ToString();
                TextResponse response = new TextResponse(id, content);
                presetTextResponses.Add(response);
            }

            property = callDetailsTypeInfo.GetDeclaredProperty(PhoneCallDetailsConstants.StartTime);
            DateTimeOffset startTime = DateTimeOffset.Parse(property.GetValue(callDetailsObj).ToString());

            property = callDetailsTypeInfo.GetDeclaredProperty(PhoneCallDetailsConstants.State);
            string callstatevalue = property.GetValue(callDetailsObj).ToString();
            PhoneCallState state = (PhoneCallState)(Enum.Parse(Type.GetType(BaseConstants.PhoneCallState), callstatevalue, true));

            PhoneCallDetails callDetails = new PhoneCallDetails(phonecalldirection, callid, callmediatype,
                                                                calltransport, confcallid, contactname, endtime, phonelineid, phonenumber, presetTextResponses, startTime, state);

            return callDetails;
        }

        /// <summary>
        /// Used internally to create
        /// Email Notification Trigger details object.
        /// </summary>
        /// <param name="triggerDetailsValue">IEmailNotificationTrigger object</param>
        /// <returns>EmailNotificationTrigger object</returns>
        public static EmailNotificationTriggerDetails CreateEmailNotificationTriggerDetails(Object triggerDetailsValue)
        {
            IAccessoryNotificationTriggerDetails commonDetails = ObjectFactory.CreateNotificationTriggerDetails(triggerDetailsValue);
            TypeInfo emailTypeInfo = Type.GetType(BaseConstants.EmailNotificationTypeName).GetTypeInfo();

            var property = emailTypeInfo.GetDeclaredProperty(EmailNotificationTriggerDetailsConstants.AccountName);
            string accoutname = property.GetValue(triggerDetailsValue).ToString();

            property = emailTypeInfo.GetDeclaredProperty(EmailNotificationTriggerDetailsConstants.EmailMessage);
            EmailMessage emailmessage = property.GetValue(triggerDetailsValue) as EmailMessage;

            property = emailTypeInfo.GetDeclaredProperty(EmailNotificationTriggerDetailsConstants.ParentFolderName);
            string foldername = property.GetValue(triggerDetailsValue).ToString();

            property = emailTypeInfo.GetDeclaredProperty(EmailNotificationTriggerDetailsConstants.SenderAddress);
            string senderaddress = property.GetValue(triggerDetailsValue).ToString();

            property = emailTypeInfo.GetDeclaredProperty(EmailNotificationTriggerDetailsConstants.SenderName);
            string sendername = property.GetValue(triggerDetailsValue).ToString();

            property = emailTypeInfo.GetDeclaredProperty(EmailNotificationTriggerDetailsConstants.Timestamp);
            DateTimeOffset timestamp = DateTimeOffset.Parse(property.GetValue(triggerDetailsValue).ToString());

            EmailNotificationTriggerDetails emailDetails = new EmailNotificationTriggerDetails(commonDetails, accoutname, emailmessage, foldername, senderaddress, sendername, timestamp);
            return emailDetails;
        }

        /// <summary>
        /// Used internally to create
        /// Reminder Notification Trigger details object.
        /// </summary>
        /// <param name="triggerDetailsValue">IReminderNotificationTrigger object</param>
        /// <returns>ReminderNotificationTrigger object</returns>
        public static ReminderNotificationTriggerDetails CreateReminderNotificationTriggerDetails(Object triggerDetailsValue)
        {
            IAccessoryNotificationTriggerDetails commonDetails = ObjectFactory.CreateNotificationTriggerDetails(triggerDetailsValue);
            TypeInfo reminderTypeInfo = Type.GetType(BaseConstants.ReminderNotificationTypeName).GetTypeInfo();

            var property = reminderTypeInfo.GetDeclaredProperty(ReminderTriggerDetailsConstants.Appointment);
            Appointment appointment = property.GetValue(triggerDetailsValue) as Appointment;

            property = reminderTypeInfo.GetDeclaredProperty(ReminderTriggerDetailsConstants.Description);
            string description = property.GetValue(triggerDetailsValue).ToString();

            property = reminderTypeInfo.GetDeclaredProperty(ReminderTriggerDetailsConstants.Details);
            string details = property.GetValue(triggerDetailsValue).ToString();

            property = reminderTypeInfo.GetDeclaredProperty(ReminderTriggerDetailsConstants.ReminderId);
            Guid reminderid = Guid.Parse(property.GetValue(triggerDetailsValue).ToString());

            string notifTypeValue = reminderTypeInfo.GetDeclaredProperty(ReminderTriggerDetailsConstants.ReminderState).GetValue(triggerDetailsValue).ToString();
            ReminderState reminderState = (ReminderState)(Enum.Parse(Type.GetType(BaseConstants.ReminderStateTypeName), notifTypeValue, true));

            property = reminderTypeInfo.GetDeclaredProperty(ReminderTriggerDetailsConstants.Timestamp);
            DateTime timeStamp = DateTime.Parse(property.GetValue(triggerDetailsValue).ToString());

            property = reminderTypeInfo.GetDeclaredProperty(ReminderTriggerDetailsConstants.Title);
            string title = property.GetValue(triggerDetailsValue).ToString();

            ReminderNotificationTriggerDetails reminderDetails = new ReminderNotificationTriggerDetails(commonDetails, appointment, description, details,
                reminderid, reminderState, timeStamp, title);
            return reminderDetails;
        }

        /// <summary>
        /// Used internally to create
        /// App uninstall Notification Trigger details object.
        /// </summary>
        /// <param name="triggerDetailsValue">Trigger details object</param>
        /// <returns>IAccessoryNotificationTriggerDetails object</returns>
        public static IAccessoryNotificationTriggerDetails CreateAppUninstallledNotificationTriggerDetails(Object triggerDetailsValue)
        {
            IAccessoryNotificationTriggerDetails triggerDetails = ObjectFactory.CreateNotificationTriggerDetails(triggerDetailsValue);
            return triggerDetails;
        }

        /// <summary>
        /// Used internally to create
        /// Media Notification Trigger details object.
        /// </summary>
        /// <param name="triggerDetailsValue">IMediaNotificationTrigger object</param>
        /// <returns>IMediaControlsTriggerDetails object</returns>
        public static IMediaControlsTriggerDetails CreateMediaNotificationTriggerDetails(Object triggerDetailsValue)
        {                        
            TypeInfo mediaTypeInfo = Type.GetType(BaseConstants.MediaNotificationTypeName).GetTypeInfo();
            IAccessoryNotificationTriggerDetails commonDetails = ObjectFactory.CreateNotificationTriggerDetails(triggerDetailsValue);

            var property = mediaTypeInfo.GetDeclaredProperty(MediaControlTriggerDetailsConstants.PlaybackStatus);
            string playbackstatusvalue = property.GetValue(triggerDetailsValue).ToString();
            PlaybackStatus status = (PlaybackStatus)(Enum.Parse(Type.GetType(BaseConstants.PlaybackStatus), playbackstatusvalue, true));
            
            property = mediaTypeInfo.GetDeclaredProperty(MediaControlTriggerDetailsConstants.MediaMetadata);
            var metadataVal = property.GetValue(triggerDetailsValue);
            MediaMetadata mediametadata = metadataVal == null ? null : ObjectFactory.CreateMediaMetadata(metadataVal);

            IMediaControlsTriggerDetails mediaTriggerDetails = new MediaControlsTriggerDetails(commonDetails, status, mediametadata);
            return mediaTriggerDetails;
        }

        /// <summary>
        /// Used internally to create
        /// Cortana Notification Trigger details object.
        /// </summary>
        /// <param name="triggerDetailsValue">CortanaTileNotificationTrigger object</param>
        /// <returns>ICortanaTileNotificationTrigger object</returns>
        public static ICortanaTileNotificationTriggerDetails CreateCortanaTileNotificationTriggerDetails(Object triggerDetailsValue)
        {
            TypeInfo coratanaTypeInfo = Type.GetType(BaseConstants.CortanaNotificationTypeName).GetTypeInfo();
            IAccessoryNotificationTriggerDetails commonDetails = ObjectFactory.CreateNotificationTriggerDetails(triggerDetailsValue);

            var property = coratanaTypeInfo.GetDeclaredProperty(CortanaTileNotificationTriggerDetailsConstants.Content);
            string contentvalue = property.GetValue(triggerDetailsValue).ToString();

            property = coratanaTypeInfo.GetDeclaredProperty(CortanaTileNotificationTriggerDetailsConstants.EmphasizedText);
            string emphasizedTextvalue = property.GetValue(triggerDetailsValue).ToString();

            property = coratanaTypeInfo.GetDeclaredProperty(CortanaTileNotificationTriggerDetailsConstants.LargeContent1);
            string largeContent1value = property.GetValue(triggerDetailsValue).ToString();

            property = coratanaTypeInfo.GetDeclaredProperty(CortanaTileNotificationTriggerDetailsConstants.LargeContent2);
            string largeContent2value = property.GetValue(triggerDetailsValue).ToString();

            property = coratanaTypeInfo.GetDeclaredProperty(CortanaTileNotificationTriggerDetailsConstants.NonWrappedSmallContent1);
            string nonWrappedSmallContent1value = property.GetValue(triggerDetailsValue).ToString();

            property = coratanaTypeInfo.GetDeclaredProperty(CortanaTileNotificationTriggerDetailsConstants.NonWrappedSmallContent2);
            string nonWrappedSmallContent2value = property.GetValue(triggerDetailsValue).ToString();

            property = coratanaTypeInfo.GetDeclaredProperty(CortanaTileNotificationTriggerDetailsConstants.NonWrappedSmallContent3);
            string nonWrappedSmallContent3value = property.GetValue(triggerDetailsValue).ToString();

            property = coratanaTypeInfo.GetDeclaredProperty(CortanaTileNotificationTriggerDetailsConstants.NonWrappedSmallContent4);
            string nonWrappedSmallContent4value = property.GetValue(triggerDetailsValue).ToString();

            property = coratanaTypeInfo.GetDeclaredProperty(CortanaTileNotificationTriggerDetailsConstants.Source);
            string sourcevalue = property.GetValue(triggerDetailsValue).ToString();

            property = coratanaTypeInfo.GetDeclaredProperty(CortanaTileNotificationTriggerDetailsConstants.TileId);
            string tileIdvalue = property.GetValue(triggerDetailsValue).ToString();

            ICortanaTileNotificationTriggerDetails cortanaTriggerDetails = new CortanaTileNotificationTriggerDetails(commonDetails, contentvalue, emphasizedTextvalue, largeContent1value, largeContent2value, nonWrappedSmallContent1value, nonWrappedSmallContent2value, nonWrappedSmallContent3value, nonWrappedSmallContent4value, sourcevalue, tileIdvalue);
            return cortanaTriggerDetails;
        }

        #endregion

        #region internal methods

        internal static MediaMetadata CreateMediaMetadata(/*IMediaMetadata*/ Object mediaMetaData)
        {
            TypeInfo mediaMetadataType = Type.GetType(BaseConstants.IMediaMetaDataTypeName).GetTypeInfo();

            var property = mediaMetadataType.GetDeclaredProperty(MediaMetadataConstants.Album);
            string album = property.GetValue(mediaMetaData).ToString();

            property = mediaMetadataType.GetDeclaredProperty(MediaMetadataConstants.Artist);
            string artist = property.GetValue(mediaMetaData).ToString();

            property = mediaMetadataType.GetDeclaredProperty(MediaMetadataConstants.Duration);
            TimeSpan duration = TimeSpan.Parse(property.GetValue(mediaMetaData).ToString());

            property = mediaMetadataType.GetDeclaredProperty(MediaMetadataConstants.Subtitle);
            string subtitle = property.GetValue(mediaMetaData).ToString();

            property = mediaMetadataType.GetDeclaredProperty(MediaMetadataConstants.Thumbnail);
            IRandomAccessStreamReference thumbnail = property.GetValue(mediaMetaData) as IRandomAccessStreamReference;

            property = mediaMetadataType.GetDeclaredProperty(MediaMetadataConstants.Title);
            string title = property.GetValue(mediaMetaData).ToString();

            property = mediaMetadataType.GetDeclaredProperty(MediaMetadataConstants.Track);
            uint track = uint.Parse(property.GetValue(mediaMetaData).ToString());

            MediaMetadata mediadata = new MediaMetadata(album, artist, duration, subtitle, thumbnail, title, track);
            return mediadata;
        }

        internal static PlaybackCapability GetMediaPlaybackCapabilities(/* int */ Object capabilityData)
        {
            return (PlaybackCapability)Enum.Parse(typeof(PlaybackCapability), capabilityData.ToString());
        }

        internal static PhoneLineDetails CreatePhoneLineDetails(Object line)
        {
            TypeInfo lineTypeInfo = Type.GetType(BaseConstants.IPhoneLineDetails).GetTypeInfo();

            var property = lineTypeInfo.GetDeclaredProperty(PhoneLineDetailsConstants.DefaultOutgoingLine);
            bool isOutgoingLine = bool.Parse(property.GetValue(line).ToString());

            property = lineTypeInfo.GetDeclaredProperty(PhoneLineDetailsConstants.DisplayName);
            string displayName = property.GetValue(line).ToString();

            property = lineTypeInfo.GetDeclaredProperty(PhoneLineDetailsConstants.LineId);
            Guid lineid = Guid.Parse(property.GetValue(line).ToString());

            property = lineTypeInfo.GetDeclaredProperty(PhoneLineDetailsConstants.LineNumber);
            string lineNumber = property.GetValue(line).ToString();

            property = lineTypeInfo.GetDeclaredProperty(PhoneLineDetailsConstants.RegistrationState);
            string registrationstatevalue = property.GetValue(line).ToString();
            PhoneLineRegistrationState registrationstate = (PhoneLineRegistrationState)(Enum.Parse(Type.GetType(BaseConstants.PhoneLineRegistrationStateTypeName), registrationstatevalue, true));

            property = lineTypeInfo.GetDeclaredProperty(PhoneLineDetailsConstants.VoicemailCount);
            uint voicemailcount = uint.Parse(property.GetValue(line).ToString());

            return new PhoneLineDetails(isOutgoingLine, displayName, lineid, lineNumber, registrationstate, voicemailcount);
        }

        #endregion

        #region Public Methods for Unit Testing
        public static IAccessoryNotificationTriggerDetails CreateNotificationTriggerDetails(string appdisplayname, string appid, AccessoryNotificationType type, bool startedprocessing, DateTimeOffset timestamp, Object rawdata)
        {
            return new AccessoryNotificationTriggerDetails(appdisplayname, appid, type, startedprocessing, timestamp, rawdata);
        }

        public static ToastNotificationTriggerDetails CreateToastNotificationTriggerDetails(IAccessoryNotificationTriggerDetails triggerDetails, bool suppresspopup, string text1, string text2, string text3, string text4)
        {
            return new ToastNotificationTriggerDetails(triggerDetails, suppresspopup, text1, text2, text3, text4);
        }

        public static AlarmNotificationTriggerDetails CreateAlarmNotificationTriggerDetails(IAccessoryNotificationTriggerDetails triggerDetails, Guid alarmId, ReminderState reminderState, DateTimeOffset timestamp, string title)
        {
            return new AlarmNotificationTriggerDetails(triggerDetails, alarmId, reminderState, timestamp, title);
        }

        public static TextResponse CreateTextResponse(uint id, string content)
        {
            return new TextResponse(id, content);
        }

        public static PhoneCallDetails CreatePhoneCallDetails(PhoneCallDirection callDirection, uint callId, PhoneMediaType callMediaType, PhoneCallTransport callTransport, uint conferenceCallId, string contactName, DateTimeOffset endTime, Guid phoneLine, string phoneNumber, IReadOnlyList<TextResponse> presetTextResponses, DateTimeOffset startTime, PhoneCallState state)
        {
            return new PhoneCallDetails(callDirection, callId, callMediaType, callTransport, conferenceCallId, contactName, endTime, phoneLine, phoneNumber, presetTextResponses, startTime, state);
        }

        public static PhoneNotificationTriggerDetails CreatePhoneNotificationTriggerDetails(IAccessoryNotificationTriggerDetails triggerDetails, PhoneCallDetails callDetails, Guid phonelineChangedId, PhoneNotificationType phoneNotificationType)
        {
            return new PhoneNotificationTriggerDetails(triggerDetails, callDetails, phonelineChangedId, phoneNotificationType);
        }

        public static PhoneLineDetails CreatePhoneLineDetails(bool defaultOutgoingLine, string displayName, Guid lineId, string lineNumber, PhoneLineRegistrationState registrationState, uint voicemailCount)
        {
            return new PhoneLineDetails(defaultOutgoingLine, displayName, lineId, lineNumber, registrationState, voicemailCount);
        }

        #endregion 
    }
}
