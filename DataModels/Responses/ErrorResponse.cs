using System;
using Twity.DataModels.Errors;

namespace Twity.DataModels.Responses
{
    [Serializable]
    public class ErrorResponse
    {
        public Error[] errors;
    }
}