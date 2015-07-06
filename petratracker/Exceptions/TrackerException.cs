using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace petratracker.Exceptions
{
    #region TrackerUser Exceptions
    
    [Serializable]
    class TrackerUserNotFoundException : ApplicationException
    {
        public TrackerUserNotFoundException() { }
        public TrackerUserNotFoundException(string message) : base(message) { }
        public TrackerUserNotFoundException(string message, System.Exception inner) { }
        protected TrackerUserNotFoundException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
    
    [Serializable]
    class TrackerUserInvalidPasswordException : ApplicationException
    {
        public TrackerUserInvalidPasswordException() { }
        public TrackerUserInvalidPasswordException(string message) : base(message) { }
        public TrackerUserInvalidPasswordException(string message, System.Exception inner) { }
        protected TrackerUserInvalidPasswordException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    #endregion

    #region DB Exceptions

    [Serializable]
    class TrackerDBNotSetupException : ApplicationException
    {
        public TrackerDBNotSetupException() { }
        public TrackerDBNotSetupException(string message) : base(message) { }
        public TrackerDBNotSetupException(string message, System.Exception inner) { }
        protected TrackerDBNotSetupException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    class TrackerDBConnectionException : ApplicationException
    {
        public TrackerDBConnectionException() { }
        public TrackerDBConnectionException(string message) : base(message) { }
        public TrackerDBConnectionException(string message, System.Exception inner) { }
        protected TrackerDBConnectionException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
    
    #endregion
}
