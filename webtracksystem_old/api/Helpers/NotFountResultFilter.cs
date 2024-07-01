using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Api
{
    public class NotFoundResultAttribute : ResultFilterAttribute
    {
        public override void OnResultExecuted(ResultExecutedContext context) {
            ObjectResult result = context.Result as ObjectResult;
            if(result != null && result.Value == null){
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
            }
        }
    }
}   