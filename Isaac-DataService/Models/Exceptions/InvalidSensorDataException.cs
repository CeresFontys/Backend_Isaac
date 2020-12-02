using System;

namespace Isaac_DataService.Services
{
    public class InvalidSensorDataException : Exception
    {
        public InvalidSensorDataException()
        {
        }

        public InvalidSensorDataException(string message)
            : base(message)
        {
        }

        public InvalidSensorDataException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}