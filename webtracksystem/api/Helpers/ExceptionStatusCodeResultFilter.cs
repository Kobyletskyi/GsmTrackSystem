using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Api
{
    public class ExceptionStatusCodeResultFilter : ResultFilterAttribute
    {
        public override void OnResultExecuted(ResultExecutedContext context)
        {
            if(context.Exception != null)
            {
                //https://books.google.com.ua/books?id=iEQWDQAAQBAJ&pg=PA598&lpg=PA598&dq=ResultFilterAttribute+on+exception&source=bl&ots=tKSN9kG7gj&sig=0j4hzPKzPJxWWUIdO0tjejRyBUI&hl=uk&sa=X&ved=0ahUKEwjpg9zGl6DWAhVrP5oKHTgmCkcQ6AEIXDAH#v=onepage&q=ResultFilterAttribute%20on%20exception&f=false
                context.HttpContext.Response.StatusCode = 500;
                context.HttpContext.Response.WriteAsync("error");
            }
            //base.OnResultExecuted(context);
        }
    }
}   