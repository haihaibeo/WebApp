using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppCore31
{
    public struct ReturnMessage
    {
        /// <summary>
        /// Create a return message to the client
        /// </summary>
        /// <param name="msg">Messages</param>
        /// <param name="error">Errors</param>
        public ReturnMessage(object msg, object error)
        {
            Message = msg;
            Error = error;
        }
        public object Message { get; set; }
        public object Error { get; set; }
    }
}
