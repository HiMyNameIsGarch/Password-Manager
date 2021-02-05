using PassManager_WebApi.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace PassManager_WebApi.ActionFilters
{
    public class LoggingAtribute : ActionFilterAttribute
    {
        private const string TimeFormat = "HH:mm:ss:FFFF";
        private string controller;
        private string actionType;
        private string fullPath;
        private DateTime DateWhenActionStarted;
        private Logging log = new Logging();
        public LoggingAtribute()
        {

        }
        public override void OnActionExecuting(HttpActionContext filterContext)
        {
            //get current time
            DateWhenActionStarted = DateTime.Now;
            //get additional information
            controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            actionType = filterContext.Request.Method.Method;
            fullPath = filterContext.Request.RequestUri.PathAndQuery;
            //log
            log.Info($"Action of type {actionType}, on controller {controller} with the full path: {fullPath} just started at: {DateWhenActionStarted.ToString(TimeFormat)}");
        }
        public override void OnActionExecuted(HttpActionExecutedContext filterContext)
        {
            //get how long the action took, convert to miliseconds and multiply by -1 to get a positive number
            var actionTook = ((DateWhenActionStarted - DateTime.Now).TotalMilliseconds * -1).ToString();
            //get addtional information about the status call
            bool isSuccess = filterContext.Response.IsSuccessStatusCode;
            string statusCode = filterContext.Response.StatusCode.ToString();
            string reasonPhrase = filterContext.Response.ReasonPhrase;
            bool hadException = filterContext.Exception != null;
            //build the msg
            string msgToLog = $"Action of type {actionType}, on Controller {controller} with the Full Path: {fullPath} " +
                $"{(isSuccess ? "was success" : "wasn\'t success")} with Status Code: {statusCode},\n\tReason Phrase: {reasonPhrase}, it " +
                $"{(hadException ? "had an exception" : "hadn't an exception")} and it took {actionTook} Milliseconds to complete!\n";
            //log it
            if (!isSuccess)
                log.Warning(msgToLog);
            else
                log.Info(msgToLog);
        }
    }
}