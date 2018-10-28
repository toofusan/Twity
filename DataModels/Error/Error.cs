using System;

namespace Twity.DataModels.Errors
{
    [Serializable]
    public class Error
    {
        public int code;
        public string message;
    }
}