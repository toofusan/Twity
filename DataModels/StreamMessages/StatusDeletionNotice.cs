using System;

namespace Twity.DataModels.StreamMessages
{
    [Serializable]
    public class StatusDeletionNotice
    {
        public StatusDelete delete;
    }
}
