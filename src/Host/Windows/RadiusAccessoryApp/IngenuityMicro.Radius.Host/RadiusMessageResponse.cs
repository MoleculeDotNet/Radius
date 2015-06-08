using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IngenuityMicro.Radius.Host
{
    public class RadiusMessageResponse
    {
        private readonly RadiusMessage _originalMsg;
        private readonly int _status;
        private readonly Dictionary<string, object> _responseArgs;

        internal RadiusMessageResponse(RadiusMessage msg, int status, Dictionary<string, object> responseArgs)
        {
            _originalMsg = msg;
            _status = status;
            _responseArgs = responseArgs;
        }

        public RadiusMessage OriginalMessage { get { return _originalMsg; } }
        public int Status { get { return _status; } }
        public Dictionary<string, object> ResultValues { get { return _responseArgs; } }
    }
}
