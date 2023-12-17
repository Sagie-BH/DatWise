using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public enum LogLevel
    {
        INFO,
        DEBUG,
        ERROR,
        EXCEPTION
    }
    public enum OperationOutcome
    {
        Success,
        Created,
        NotFound,
        BadRequest,
        Unauthorized,
        Forbidden,
        InternalError
    }
}
