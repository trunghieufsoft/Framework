using System;
using Asset.Common.ViewModel;
using Asset.Common.Enumerations;

namespace Asset.Common.Exceptions
{
    public class ExceptionError : Exception
    {
        public ErrorModel Error { get; set; }

        public ExceptionError()
        {
            Error = new ErrorModel();
        }

        public ExceptionError(ErrorCodeEnum error)
        {
            Error = new ErrorModel { ErrorCode = (int)error, Message = error.ToString() };
        }

        public ExceptionError(ErrorCodeEnum error, dynamic data)
        {
            Error = new ErrorModel { ErrorCode = (int)error, Message = error.ToString(), Data = data };
        }
    }
}
