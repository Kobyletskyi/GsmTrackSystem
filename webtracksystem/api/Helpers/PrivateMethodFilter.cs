using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Api
{
    public class PrivateMethodFilter : ActionFilterAttribute
    {
        public string UserIdParamName = "id";
        public PrivateMethodFilter(string paramName){
            UserIdParamName = paramName;
        }

        protected PrivateMethodFilter()
        {
        }

        // public override void OnActionExecuted(ActionExecutedContext context)
        // {
        //     throw new NotImplementedException();
        // }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var userId = context.ActionArguments[UserIdParamName];
            var token = context.HttpContext.Request.Headers;
            var tokenUsetId = /*decode*/ token;
            if(/*userId && tokenUsetId && */userId != tokenUsetId){
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            }
        }
    }
}   