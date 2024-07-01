using System;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Constraints;

namespace Api
{
    public class MeRouteConstraint : IRouteConstraint 
    {
        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            object value;
            if (values.TryGetValue(routeKey, out value) && value.Equals("me"))
            {
                return true;
            }
            var intConstraint = new IntRouteConstraint();
            return intConstraint.Match(httpContext, route, routeKey, values, routeDirection);
        }
    }
    public class MyResourceAttribute : Attribute, IFilterFactory
    {
        private string userIdParamName = "userId";
        public MyResourceAttribute(string paramName){
            userIdParamName = paramName;
        }
        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            return new MyResourceFilter(userIdParamName);
        }

        public class MyResourceFilter : IResourceFilter
        {
            private string userIdParamName = "userId";
            public MyResourceFilter(string paramName){
                userIdParamName = paramName;
            }
            public void OnResourceExecuted(ResourceExecutedContext context)
            { }

            public void OnResourceExecuting(ResourceExecutingContext context)
            {
                object value;
                if (context.RouteData.Values.TryGetValue(userIdParamName, out value) && value.Equals("me"))
                {
                    context.RouteData.Values[userIdParamName] = context.HttpContext.User.Identity.Name;
                }
            }
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }        
}   