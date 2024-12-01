using Microsoft.AspNetCore.Http;
using System.Net;

namespace UserAPI.CustomException
{
    public class CustomException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }
        public CustomException(string message, HttpStatusCode statusCode) : base(message)
        {
            StatusCode = statusCode;
        }
    }
    public class UnAuthorizedException: CustomException
    {
        public UnAuthorizedException(string message):base(message, HttpStatusCode.Unauthorized)
        {

        }
    }
}
