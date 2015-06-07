using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IngenuityMicro.Radius.Host
{
    public delegate void RadiusMessageReponseHandler(object sender, Dictionary<string,string> args);

    public class RadiusMessage
    {
        private string _target;
        private Dictionary<string,string> _parameters;
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

        public Dictionary<string, string> Parameters
        {
            get { return _parameters; }
        }

        internal string Serialize()
        {
            StringBuilder body = new StringBuilder();
            if (_parameters.Count == 0)
                body.Append("#");
            else
            {
                foreach (var item in _parameters)
                {
                    body.Append(string.Format("{0}|{1}#", item.Key, item.Value));
                }
            }
            return string.Format("#{0}#{1}#{2}", _target, _messageId, body.ToString());
        }

        internal void OnResponseReceived(object sender, Dictionary<string, string> responseArgs)
        {
            if (this.ResponseReceived != null)
                this.ResponseReceived(sender, responseArgs);
        }

    }
}
