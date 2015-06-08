using System;
using System.Collections.Generic;
using Windows.ApplicationModel.Email;
using System.Diagnostics;
using System.Linq;
using Windows.ApplicationModel.Background;
using Windows.Storage;
using Microsoft.Phone.AccessoryManager.AbstractionLayer;
using IngenuityMicro.Radius.Host;
using Ninject;
using IngenuityMicro.Radius.Host.Messages;

namespace RadiusTaskLib
{
    public sealed class RadiusBackgroundTask : IBackgroundTask
    {
        private static IKernel _kernel;
        internal static IKernel Kernel
        {
            get
            {
                if (_kernel==null)
                {
                    _kernel = new StandardKernel(new IngenuityMicro.Radius.Host.Module());
                }
                return _kernel;
            }
        }

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            BackgroundTaskDeferral deferral = taskInstance.GetDeferral();
            AccessoryManufacturerAPIs accessoryManufacturerAPIs = new AccessoryManufacturerAPIs();
            try
            {
                IAccessoryNotificationTriggerDetails notificationTriggerDetails = AccessoryManager.GetNextTriggerDetails();
                while (notificationTriggerDetails != null)
                // get rid of this while loop in case you would like to process one notificaiton at a time. 
                //You have to use ProcessingTriggerDetailsCompleted to indicate that you are done with this instance of the trigger
                {

                    switch (notificationTriggerDetails.AccessoryNotificationType)
                    {
                        case AccessoryNotificationType.Toast:
                            ToastNotificationTriggerDetails toast = notificationTriggerDetails as ToastNotificationTriggerDetails;
                            accessoryManufacturerAPIs.HandleToastTriggerDetails(toast);
                            break;

                        case AccessoryNotificationType.Phone:
                            PhoneNotificationTriggerDetails phoneCallTriggerDetails = notificationTriggerDetails as PhoneNotificationTriggerDetails;
                            accessoryManufacturerAPIs.HandlePhoneTriggerDetails(phoneCallTriggerDetails);
                            break;

                        case AccessoryNotificationType.Alarm:
                            AlarmNotificationTriggerDetails alarmTriggerDetails = notificationTriggerDetails as AlarmNotificationTriggerDetails;
                            accessoryManufacturerAPIs.HandleAlarmTriggerDetails(alarmTriggerDetails);
                            break;

                        case AccessoryNotificationType.AppUninstalled:
                            IAccessoryNotificationTriggerDetails appUninstallTriggerDetails = notificationTriggerDetails as IAccessoryNotificationTriggerDetails;
                            accessoryManufacturerAPIs.HandleAppUninstallTriggerDetails(appUninstallTriggerDetails);
                            break;

                        case AccessoryNotificationType.Email:
                            EmailNotificationTriggerDetails emailDetails = notificationTriggerDetails as EmailNotificationTriggerDetails;
                            accessoryManufacturerAPIs.HandleEmailTriggerDetails(emailDetails);
                            break;

                        case AccessoryNotificationType.Reminder:
                            ReminderNotificationTriggerDetails reminderDetails = notificationTriggerDetails as ReminderNotificationTriggerDetails;
                            accessoryManufacturerAPIs.HandleReminderTriggerDetails(reminderDetails);
                            break;

                        case AccessoryNotificationType.Dnd:
                            IAccessoryNotificationTriggerDetails dndChangeTriggerDetails = notificationTriggerDetails as IAccessoryNotificationTriggerDetails;
                            accessoryManufacturerAPIs.HandleDoNotDisturbModeChangeTriggerDetails(dndChangeTriggerDetails);
                            break;

                        case AccessoryNotificationType.BatterySaver:
                            IAccessoryNotificationTriggerDetails batterySaverChangeTriggerDetails = notificationTriggerDetails as IAccessoryNotificationTriggerDetails;
                            accessoryManufacturerAPIs.HandleBatterySaverModeChangeTriggerDetails(batterySaverChangeTriggerDetails);
                            break;

                        case AccessoryNotificationType.DrivingMode:
                            IAccessoryNotificationTriggerDetails drivingModeChangeTriggerDetails = notificationTriggerDetails as IAccessoryNotificationTriggerDetails;
                            accessoryManufacturerAPIs.DrivingModeChangeTriggerDetails(drivingModeChangeTriggerDetails);
                            break;
                        case AccessoryNotificationType.Media:
                            MediaControlsTriggerDetails mediaControlTriggerDetails = notificationTriggerDetails as MediaControlsTriggerDetails;
                            accessoryManufacturerAPIs.HandleMediaControlTriggerDetails(mediaControlTriggerDetails);

                            //One can perform commands like this based on actions from the accessory device
                            //AccessoryManager.PerformMediaPlaybackCommand(PlaybackCommands.Pause);
                            break;
                        case AccessoryNotificationType.CortanaTile:
                            CortanaTileNotificationTriggerDetails cortanaDetails = notificationTriggerDetails as CortanaTileNotificationTriggerDetails;
                            accessoryManufacturerAPIs.HandleCortanaTileUpdateTriggerDetails(cortanaDetails);
                            break;
                        default:
                            Debug.WriteLine("Not supported notification type");
                            break;
                    }

                    //TODO: Make connection and send the data to the accessory

                    //Mark the trigger details as completed processing
                    AccessoryManager.ProcessTriggerDetails(notificationTriggerDetails);

                    notificationTriggerDetails = AccessoryManager.GetNextTriggerDetails();
                }
            }
            catch (Exception exp)
            {
                Debug.WriteLine("Exception while processing the trigger details " + exp.Message);
            }
            deferral.Complete();
        }
    }

    public sealed class AccessoryManufacturerAPIs
    {
        ApplicationDataContainer adc = ApplicationData.Current.LocalSettings;
        int index = 0;

        public void LogMessage(string message)
        {
            Debug.WriteLine(message);
            adc.Values[(index++).ToString()] = message;
        }

        public void HandleToastTriggerDetails(ToastNotificationTriggerDetails toast)
        {
            LogMessage("Got toast from :" + toast.AppDisplayName + " AppId: " + toast.AppId + " Header: " + toast.Text1 + " Body: " + toast.Text2 + " IsGhost:" + toast.SuppressPopup + " at: " + toast.TimeCreated);
            //send the toast data to the accessory device
        }

        public void HandlePhoneTriggerDetails(PhoneNotificationTriggerDetails phoneCallTriggerDetails)
        {
            var radius = RadiusBackgroundTask.Kernel.Get<RadiusDeviceEnumerator>();

            string callMsg = "Got Phone Notification for : Phone Notification Type :" + phoneCallTriggerDetails.PhoneNotificationType + ",";
            switch (phoneCallTriggerDetails.PhoneNotificationType)
            {
                case PhoneNotificationType.NewCall:
                case PhoneNotificationType.CallChanged:
                    if (phoneCallTriggerDetails.CallDetails.CallDirection == PhoneCallDirection.Incoming)
                    {
                        var msg = new IncomingCallNotification();
                        msg.PhoneNumber = phoneCallTriggerDetails.CallDetails.PhoneNumber;
                        msg.ContactName = phoneCallTriggerDetails.CallDetails.ContactName;
                        msg.CallId = (int)phoneCallTriggerDetails.CallDetails.CallId;
                        radius.Send(msg);
                    }

                    //Based on input from the device use the following methods to take action on the call
                    //Accept Call
                    //AccessoryManager.GetInstance().AcceptPhoneCall(phoneCall.CallDetails.CallId);

                    //Reject With SMS
                    //IReadOnlyList<ITextResponse> listResponses = phoneCall.CallDetails.PresetTextResponses;
                    //string response = listResponses[0].Content;
                    //AccessoryManager.GetInstance().RejectPhoneCall(phoneCall.CallDetails.CallId, listResponses[0].Id);

                    //Rejct Call
                    //AccessoryManager.GetInstance().RejectPhoneCall(phoneCall.CallDetails.CallId);
                    break;

                case PhoneNotificationType.PhoneCallAudioEndpointChanged:
                case PhoneNotificationType.PhoneMuteChanged:
                    break;

                case PhoneNotificationType.LineChanged:
                    //Guid phoneLineId = phoneCallTriggerDetails.PhoneLineChangedId;
                    //callMsg += "Line Change Id: " + phoneCallTriggerDetails.PhoneLineChangedId + ",";
                    //PhoneLineDetails linedetails = AccessoryManager.GetPhoneLineDetails(phoneLineId);
                    //callMsg += linedetails.DefaultOutgoingLine + "," + linedetails.DisplayName + "," + linedetails.LineNumber + "," + linedetails.RegistrationState + "," + linedetails.VoicemailCount;

                    break;
            }
        }

        public void HandleAlarmTriggerDetails(AlarmNotificationTriggerDetails alarmTriggerDetails)
        {
            LogMessage("Alarm triggered :  Title: " + alarmTriggerDetails.Title + "  ReminderState: " + alarmTriggerDetails.ReminderState.ToString() + "  TimeStamp:" + alarmTriggerDetails.Timestamp);
            if (alarmTriggerDetails.ReminderState == ReminderState.Active)
            {
                AccessoryManager.DismissAlarm(alarmTriggerDetails.AlarmId);
            }
        }

        public void HandleAppUninstallTriggerDetails(IAccessoryNotificationTriggerDetails appUninstallTriggerDetails)
        {
            //This can be used to handle the icons\tiles on the accessory device e.g. in case FACEBOOK app gets uninstalled remove the tile on the wearable(/watch)
            LogMessage("Got Application Uninstall Trigger for App: " + appUninstallTriggerDetails.AppDisplayName + " AppId: " + appUninstallTriggerDetails.AppId + " at" + appUninstallTriggerDetails.TimeCreated);
        }

        public void HandleReminderTriggerDetails(ReminderNotificationTriggerDetails reminderDetails)
        {
            string details = reminderDetails.Details;
            string subject = reminderDetails.Title;
            string location = "";
            if (reminderDetails.Appointment != null)
            {
                location = reminderDetails.Appointment.Location;
            }
            string timeStamp = reminderDetails.Timestamp.ToString();
            //fetch more data from the appointment object e.g. start time, end time, duration etc.

            LogMessage("Got Reminder trigger from App :" + reminderDetails.AppDisplayName + " Subject :" + subject + " Description: " + reminderDetails.Description +
                "  Details: " + details + " With Id: " + reminderDetails.ReminderId +
                " Reminder State: " + reminderDetails.ReminderState + " TimeStamp: " + timeStamp);

            //if(reminderDetails.ReminderState == ReminderState.Active)
            //{
            //    AccessoryManager.DismissReminder(reminderDetails.ReminderId);
            //}
        }

        public void HandleDoNotDisturbModeChangeTriggerDetails(IAccessoryNotificationTriggerDetails dndChangeTriggerDetails)
        {
            string timeStamp = dndChangeTriggerDetails.TimeCreated.ToString();
            string dndStateMsg = (AccessoryManager.DoNotDisturbEnabled) ? "DND Enabled" : "DND disabled";
            LogMessage("Got trigger for : " + dndStateMsg + " at " + timeStamp);
        }

        public void HandleBatterySaverModeChangeTriggerDetails(IAccessoryNotificationTriggerDetails batterySaverChangeTriggerDetails)
        {
            string timeStamp = batterySaverChangeTriggerDetails.TimeCreated.ToString();
            string batterySaverMsg = (AccessoryManager.BatterySaverState) ? "BatterySaver Enabled" : "BatterySaver disabled";
            LogMessage("Got trigger for : " + batterySaverMsg + " at " + timeStamp);
        }

        public void DrivingModeChangeTriggerDetails(IAccessoryNotificationTriggerDetails drivingModeChangeTriggerDetails)
        {
            string timeStamp = drivingModeChangeTriggerDetails.TimeCreated.ToString();
            string drivingModeMsg = (AccessoryManager.DrivingModeEnabled) ? "Driving Mode Enabled" : "Driving Mode disabled";
            LogMessage("Got trigger for : " + drivingModeMsg + " at " + timeStamp);
        }

        public void HandleEmailTriggerDetails(EmailNotificationTriggerDetails emailDetails)
        {
            string message = "Email,";
            string appName = emailDetails.AppDisplayName;
            string appId = emailDetails.AppId;
            string emailFrom = emailDetails.SenderName;
            string emailFromAddr = emailDetails.SenderAddress;

            message += appName + "," + appId + "," + emailFrom + "(" + emailFromAddr + "),";

            EmailMessage emailMessage = emailDetails.EmailMessage;
            string emailBody = emailMessage.Body;
            string subject = emailMessage.Subject;
            if (emailBody.Length > 20)
            {
                emailBody = emailBody.Substring(0, 20); //logging 20 characters, one can request for more
                //This could be implemented as a control button reponse from the accessory to get full text of the mail
            }
            message += emailBody + "," + subject + ",";

            IList<EmailAttachment> attachmentList = emailMessage.Attachments;
            string attachmentStr = "";
            if (attachmentList != null)
            {
                foreach (EmailAttachment attachment in attachmentList)
                {
                    string filename = attachment.FileName;
                    var data = attachment.Data;
                    attachmentStr += filename + ",";
                }
            }
            message += attachmentStr + ",";

            IList<EmailRecipient> toList = emailMessage.To;
            string toStr = "";
            foreach (var toItem in toList)
            {
                string address = toItem.Address;
                string name = toItem.Name;
                toStr += name + "<" + address + ">;";
            }
            message += toStr + ",";

            IList<EmailRecipient> ccList = emailMessage.CC;
            string ccStr = "";
            foreach (var ccItem in ccList)
            {
                string address = ccItem.Address;
                string name = ccItem.Name;
                ccStr += name + "<" + address + ">;";
            }
            message += ccStr + ",";

            IList<EmailRecipient> bccList = emailMessage.Bcc;
            string bccStr = "";
            foreach (var bccItem in bccList)
            {
                string address = bccItem.Address;
                string name = bccItem.Name;
                bccStr += name + "<" + address + ">;";
            }
            message += bccStr + ",";

            string emailTimeStamp = emailDetails.Timestamp.ToString();
            message += emailTimeStamp;


            string timeCreated = emailDetails.TimeCreated.ToString();
            LogMessage("Got Email trigger with content: " + message + " at" + timeCreated);
        }

        internal void HandleMediaControlTriggerDetails(MediaControlsTriggerDetails mediaTriggerDetails)
        {
            IMediaMetadata mediaMetadata = AccessoryManager.GetMediaMetadata();
            PlaybackStatus status = mediaTriggerDetails.PlaybackStatus;
            DateTimeOffset timecreated = mediaTriggerDetails.TimeCreated;

            LogMessage("Got Media trigger for :" + status + " With metadata as: " + mediaMetadata.Album + "," + mediaMetadata.Artist + "," + mediaMetadata.Duration + "," + mediaMetadata.Subtitle
            + "," + mediaMetadata.Title + "," + mediaMetadata.Track + " at " + timecreated);
        }

        internal void HandleCortanaTileUpdateTriggerDetails(CortanaTileNotificationTriggerDetails cortanaDetails)
        {
            throw new NotImplementedException();
        }
    }
}
