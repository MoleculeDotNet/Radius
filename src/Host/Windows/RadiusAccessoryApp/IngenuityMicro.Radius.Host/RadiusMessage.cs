using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace IngenuityMicro.Radius.Host
{
    public abstract class RadiusMessage
    {
        private static int _globalMessageId;
        private readonly string _target;
        private readonly string _method;
        private readonly int _messageId;
        private Dictionary<string, object> _parameters = new Dictionary<string, object>();

        public RadiusMessage(string targetApp, string method)
        {
            _target = targetApp;
            _method = method;
            _messageId = Interlocked.Increment(ref _globalMessageId);
        }

        public DateTime SentTime { get; set; }

        public string TargetAppId
        {
            get { return _target; }
        }

        public int MessageId { get { return _messageId; } }

        public string Method { get { return _method; } }

        public Dictionary<string, object> Parameters
        {
            get { return _parameters; }
        }

        public string Serialize()
        {
            var container = new { app = _target, msgid = _messageId, method = this.Method, parms = this.Parameters };
            return JsonConvert.SerializeObject(container);
        }

        public virtual void OnResponseReceived(RadiusMessageResponse response, out bool handled)
        {
            handled = true;
        }

        public virtual void OnTimeout()
        {
        }
    }
}
