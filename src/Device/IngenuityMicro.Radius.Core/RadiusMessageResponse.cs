using System;
using Microsoft.SPOT;
using System.Collections;
using System.Text;

namespace IngenuityMicro.Radius.Core
{
    public class RadiusMessageResponse : IRadiusMessage
    {
        private int _msgId;
        private Hashtable _parms = new Hashtable();

        public RadiusMessageResponse(int msgId)
        {
            _msgId = msgId;
            this.Status = 0;
        }

        public int Status { get; set; }

        public Hashtable Parameters { get { return _parms; } }

        public string Serialize()
        {
            StringBuilder result = new StringBuilder();

            result.Append("{\"replyTo\":");
            result.Append(_msgId.ToString());
            result.Append(",\"status\":");
            result.Append(this.Status.ToString());
            result.Append(",\"result\":");
            result.Append(Json.Serialize(_parms));
            result.Append("}\r\n");

            return result.ToString();
        }
    }
}
