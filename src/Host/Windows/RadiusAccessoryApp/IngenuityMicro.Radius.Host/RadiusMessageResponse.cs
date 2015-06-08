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
        private readonly Dictionary<string, string> _responseArgs;

        internal RadiusMessageResponse(RadiusMessage msg, Dictionary<string, string> responseArgs)
        {
            _originalMsg = msg;
            _responseArgs = responseArgs;
        }

        public RadiusMessage OriginalMessage { get { return _originalMsg; } }
        public Dictionary<string, string> ResponseValues { get { return _responseArgs; } }
    }
}
