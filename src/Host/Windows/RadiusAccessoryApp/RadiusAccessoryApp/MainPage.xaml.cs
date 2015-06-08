using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Devices.Enumeration;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using Ninject;

using IngenuityMicro.Radius.Host;
using Windows.ApplicationModel.Background;
using Microsoft.Phone.AccessoryManager.AbstractionLayer;

namespace RadiusAccessoryApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            var enumerator = App.DiContainer.Get<RadiusDeviceEnumerator>();

            BackgroundAccessStatus bas = await BackgroundExecutionManager.RequestAccessAsync();
            //DisplayMessageOnUI("RequestAccessAsync returned " + bas);
            if (bas == BackgroundAccessStatus.Denied)
            {
                //DisplayMessageOnUI("Looks like many apps running in background, disable some for getting this enabled!");
                return;
            }

            foreach (BackgroundTaskRegistration value in BackgroundTaskRegistration.AllTasks.Values)
            {
                //DisplayMessageOnUI("Task " + value.TaskId + " " + value.Name + " is already registered.");
                value.Unregister(true);
            }

            RegisterAccessoryBackgroundTask();

            //Enable notification types that the accessory is interested in <-- Ideally this happens from UI
            AccessoryManager.EnableAccessoryNotificationTypes((int)AccessoryNotificationType.Toast | (int)AccessoryNotificationType.Phone |
                (int)AccessoryNotificationType.AppUninstalled | (int)AccessoryNotificationType.Alarm | (int)AccessoryNotificationType.Reminder |
                (int)AccessoryNotificationType.Email | (int)AccessoryNotificationType.Dnd | (int)AccessoryNotificationType.Media
                | (int)AccessoryNotificationType.DrivingMode | (int)AccessoryNotificationType.BatterySaver | (int)AccessoryNotificationType.CortanaTile);

            IDictionary<string, AppNotificationInfo> dictionary = AccessoryManager.GetApps();
            foreach (string str in dictionary.Keys)
            {
                try
                {
                    string appTitle = dictionary[str].Name;
                    string appId = dictionary[str].Id;
                    if (false == AccessoryManager.IsNotificationEnabledForApplication(appId))
                    {
                        AccessoryManager.EnableNotificationsForApplication(appId);
                    }
                }
                catch (Exception exo)
                {
                    Debug.WriteLine(exo.Message); //handle any exception while enabling apps for notifications
                }
            }
        }

        private void RegisterAccessoryBackgroundTask()
        {
            BackgroundTaskBuilder builder = new BackgroundTaskBuilder();
            builder.Name = "TestBackGroundTask";
            builder.TaskEntryPoint = "RadiusTaskLib.RadiusBackgroundTask"; //make sure to have same entry in the appxmanifest
            string triggerId = AccessoryManager.RegisterAccessoryApp();
            DeviceManufacturerNotificationTrigger trigger = new DeviceManufacturerNotificationTrigger("Microsoft.AccessoryManagement.Notification:" + triggerId, false);
            builder.SetTrigger(trigger);
            BackgroundTaskRegistration registration = builder.Register();
            //DisplayMessageOnUI("Registered background task " + registration.TaskId + ", " + registration.Name);
            registration.Completed += registration_Completed;
        }

        void registration_Completed(BackgroundTaskRegistration sender, BackgroundTaskCompletedEventArgs args)
        {
        }
    }
}
