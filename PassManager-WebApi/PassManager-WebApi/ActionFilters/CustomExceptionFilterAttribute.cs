using PassManager_WebApi.App_Start;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;

namespace PassManager_WebApi.ActionFilters
{
    public class CustomExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private Logging log = new Logging();
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            //get additional information about response msg
            string method = actionExecutedContext.Request.Method.Method;
            string fullPath = actionExecutedContext.Request.RequestUri.PathAndQuery;
            string msgForLog = $"An exception occured at action of type {method} on the path {fullPath}";
            //then log it
            log.Fatal(msgForLog, actionExecutedContext.Exception);

            //create a new response msg
            var response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent("An unhandled exception was thrown by service."),
                ReasonPhrase = "Internal Server Error."
            };
            actionExecutedContext.Response = response;
        }
    }
}