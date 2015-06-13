using System;
using Microsoft.SPOT;

using IngenuityMicro.Radius.Fonts;
using IngenuityMicro.Radius.Hardware;
using IngenuityMicro.Radius.Core;
using System.Collections;
using System.Threading;
using NetMF.IO;
using System.Reflection;
using System.IO;

namespace IngenuityMicro.Radius.AppHost
{
    public class AppHost : IAppHost
    {
        private readonly IFileSystem _fs;
        private readonly Hashtable _apps = new Hashtable();
        private readonly Stack _appStack = new Stack();
        private string _defaultApp = null;
        private IRadiusApplication _activeApp = null;
        private AppDomain _currentAppDomain = null;

        public AppHost()
        {
            _fs = (IFileSystem)DiContainer.Instance.Resolve(typeof(IFileSystem));
        }

        public void LaunchMainMenu()
        {
            PushAndSwitchTo("MainMenu");
        }

        public void PushAndSwitchTo(string appId)
        {
            if (_activeApp.UniqueName != appId)
            {
                _appStack.Push(_activeApp.UniqueName);
                // If launching the new app fails, switch back
                if (!SwitchTo(appId))
                {
                    //TODO: Alert the user
                    PopAndSwitch();
                }
            }
        }

        public void PopAndSwitch()
        {
            if (_appStack.Count == 0)
                return;

            var returnTo = (string)_appStack.Pop();
            if (!SwitchTo(returnTo))
            {
                //TODO: Alert the user and pop again or go back to default app
            }
        }

        public bool SwitchTo(string appId)
        {
            bool success = false;

            if (_activeApp==null || _activeApp.UniqueName != appId)
            {
                if (_activeApp != null)
                    _activeApp.NavigateAway();

                if (_currentAppDomain != null && _currentAppDomain != AppDomain.CurrentDomain)
                    AppDomain.Unload(_currentAppDomain);

                IRadiusApplication newApp = LoadAndInitialize(appId);

                if (newApp!=null)
                {
                    _activeApp = newApp;
                    _activeApp.NavigateTo();
                    success = true;
                }
            }
            return success;
        }

        //TODO: Separate the file name from app id so that app ids can be longer than 13 chars
        private IRadiusApplication LoadAndInitialize(string appId)
        {
            IRadiusApplication result = null;

            try
            {
                using (var stream = _fs.Open(appId + ".pe", FileMode.Open))
                {
                    byte[] assmbytes = new byte[stream.Length];
                    stream.Read(assmbytes, 0, (int)stream.Length);

                    AppDomain domain = null;
                    try
                    {
                        var assm = Assembly.Load(assmbytes);
                        domain = AppDomain.CreateDomain(appId);
                        var obj = domain.CreateInstanceAndUnwrap(assm.FullName, appId + ".Application");
                        var type = assm.GetType(appId + ".Application");
                        MethodInfo mi = type.GetMethod("InitializeFramework");
                        mi.Invoke(obj, null);

                        result = (IRadiusApplication)obj;
                    }
                    catch (Exception ex)
                    {
                        Debug.Print(ex.ToString());
                        result = null;
                    }
                }
            }
            catch (Exception exc)
            {
                Debug.Print(exc.ToString());
                result = null;
            }
            return result;
        }

        public IRadiusApplication ActiveApp
        {
            get { return _activeApp; }
        }


        public void Run()
        {
            // Add the protocol handler for the host environment (maybe should be loadable?)
            //AddApplication(new AppHostApp());

            // Launch the default watch-face app
            SwitchTo("AnalogClock");
            //SwitchTo("MainMenu");
            Thread.Sleep(Timeout.Infinite);
        }

        public void SerialDataReceived(string msg)
        {
            try
            {
                var jsonMsg = Json.Deserialize(msg);

                if (jsonMsg is Hashtable)
                {
                    var htMsg = (Hashtable)jsonMsg;
                    var target = (string)htMsg["app"];
                    var msgId = (int)htMsg["msgid"];
                    var method = (string)htMsg["method"];
                    var parms = (Hashtable)htMsg["parms"];

                    if (_apps.Contains(target))
                    {
                        ((RadiusApplication)_apps[target]).HandleAppMessage(msgId, method, parms);
                    }

                }
            }
            catch (Exception ex)
            {
                // all exceptions here are ignored - might be parsing errors or app errors, but either way, the show must go on
            }
        }

        public void Send(IRadiusMessage msg)
        {
            var text = msg.Serialize();
            var channel = (IPeerChannel)DiContainer.Instance.Resolve(typeof(IPeerChannel));
            if (channel!=null)
                channel.Send(text);
        }

        public string[] GetInstalledApps()
        {
            var len = _apps.Count;
            string[] result = new string[len];

            int i = 0;
            foreach (var item in _apps.Values)
            {
                result[i] = ((RadiusApplication)item).UniqueName;
                ++i;
            }

            return result;
        }
    }
}
