using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twity.DataModels.StreamMessages
{
    [Serializable]
    public class Disconnect
    {
        public int code;
        public string stream_name;
        public string reason;
    }
}
