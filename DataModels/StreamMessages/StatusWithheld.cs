using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twity.DataModels.StreamMessages
{
    [Serializable]
    public class StatusWithheld
    {
        public long id;
        public long user_id;
        public string[] withheld_in_countries;
    }
}
