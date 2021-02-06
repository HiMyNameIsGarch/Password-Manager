using PassManager_WebApi.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
            log.Info($"Action of type {actionType}, on Controller {controller} with the Full Path: {fullPath} just started at: {DateWhenActionStarted.ToString(TimeFormat)}");
        }
        public override void OnActionExecuted(HttpActionExecutedContext filterContext)
        {
            //get how long the action took and convert to miliseconds
            var actionTook = (DateTime.Now - DateWhenActionStarted).TotalMilliseconds.ToString();
            bool isSuccess = false;
            int statusCode = (int)HttpStatusCode.OK;
            string reasonPhrase = string.Empty;
            string msgToLog = $"Action of type {actionType}, on Controller {controller} with the Full Path: {fullPath} ";
            //get addtional information about the status call
            bool hadException = filterContext.Exception != null;
            if (!hadException)
            {
                isSuccess = filterContext.Response.IsSuccessStatusCode;
                statusCode = (int)filterContext.Response.StatusCode;
                reasonPhrase = filterContext.Response.ReasonPhrase;
                //build the msg
                msgToLog += $"{(isSuccess ? "was Success" : "wasn\'t Success")} with Status Code: {statusCode},\n--- Reason Phrase: {reasonPhrase} " +
                    $"and it took {actionTook} milliseconds to Complete!\n";
                //log it
                if (!isSuccess)
                    log.Warning(msgToLog);
                else
                    log.Info(msgToLog);
            }
            else
            {
                msgToLog += $"had an Exception and it took {actionTook} milliseconds to Complete!\n";
                log.Error(msgToLog);
            }
        }
    }
}