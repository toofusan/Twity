using System;

namespace Twity.DataModels.StreamMessages
{
    [Serializable]
    public class WithheldContentNotice
    {
        public StatusWithheld status_withheld;
        public UserWithheld user_withheld;
    }
}
