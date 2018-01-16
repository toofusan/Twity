using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twity.DataModels.StreamMessages
{
    [Serializable]
    public class WithheldContentNotice
    {
        public StatusWithheld status_withheld;
        public UserWithheld user_withheld;
    }
}
