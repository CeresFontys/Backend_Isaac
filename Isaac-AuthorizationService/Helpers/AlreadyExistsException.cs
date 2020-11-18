using System;
using System.Runtime.Serialization;

namespace Isaac_AuthorizationService.Services
{
    [Serializable]
    internal class AlreadyExistsException : Exception
    {
        public AlreadyExistsException()
        {
        }

        public AlreadyExistsException(string message)
                : base(message)
        {
        }

        public AlreadyExistsException(string message, Exception inner)
                : base(message, inner)
        {
        }
    }
}