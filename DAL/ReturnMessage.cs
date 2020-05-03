using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppCore31
{
    public struct ReturnMessage
    {
        public ReturnMessage(string msg, string error)
        {
            Message = msg;
            Error = error;
        }
        public string Message { get; set; }
        public string Error { get; set; }
    }
}
