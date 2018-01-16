using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twity.DataModels.StreamMessages
{
    [Serializable]
    public class Warning
    {
        public string code;
        public string message;
        public int percent_full;
        public int user_id;
    }
}
