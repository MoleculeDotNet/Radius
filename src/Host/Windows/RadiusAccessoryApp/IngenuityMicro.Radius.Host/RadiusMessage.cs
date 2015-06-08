using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace IngenuityMicro.Radius.Host
{
    public delegate void RadiusMessageReponseHandler(object sender, RadiusMessageResponse response);

    public class RadiusMessage
    {
        private string _target;
        private Dictionary<string,string> _parameters = new Dictionary<string,string>();
        private int _messageId;
        private static int _globalMessageId;

        public event RadiusMessageReponseHandler ResponseReceived;

        public RadiusMessage()
        {
            _messageId = Interlocked.Increment(ref _globalMessageId);
        }

        public string TargetAppId
        {
            get { return _target; }
            set { _target = value; }
        }

        public int MessageId { get { return _messageId; } }

        public string Method { get; set; }

        public Dictionary<string, string> Parameters
        {
            get { return _parameters; }
        }

        internal string Serialize()
        {
            var container = new { app = _target, msgid = _messageId, method = this.Method, parms = this.Parameters };
            return JsonConvert.SerializeObject(container);
        }

        internal void OnResponseReceived(object sender, RadiusMessageResponse response)
        {
            if (this.ResponseReceived != null)
                this.ResponseReceived(sender, response);
        }

    }
}
