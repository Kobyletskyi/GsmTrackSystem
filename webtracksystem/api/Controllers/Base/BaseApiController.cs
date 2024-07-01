using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Api.Models;
using Api.Providers;
using Api.Providers.Auth;
using BusinessLayer;
using BusinessLayer.Models;
using BusinessLayer.Transformation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Api.Controllers
{
    //[ExceptionStatusCodeResultFilter]
    public abstract class BaseApiController : Controller
    {
        protected readonly ILogger logger;
        protected readonly IBusinessLogic logic;

        protected readonly IUrlHelper urlHelper;
        protected readonly ITypeHelperService typeHelperService;

        protected int? AuthUserId {
            get {
                int id;
                if(int.TryParse(User.Identity.Name, out id)){
                    return id; 
                }
                return null;
            }
        }
        public BaseApiController(ILoggerFactory loggerFactory, IBusinessLogic logic, IUrlHelper urlHelper, 
        ITypeHelperService typeHelperService)
        {
            this.logger = loggerFactory.CreateLogger<AuthController>();
            this.logic = logic;
            this.urlHelper = urlHelper;
            this.typeHelperService = typeHelperService;
        }

        protected string CreateResourceUri(string routeName, CollectionParameters parameters, ResourceUriType type){
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return urlHelper.Link(routeName, 
                        new { 
                            orderBy = parameters.OrderBy,
                            fields = parameters.Fields,
                            pageNumber = parameters.PageNumber - 1,
                            pageSize = parameters.PageSize });
                case ResourceUriType.NextPage:
                    return urlHelper.Link(routeName, 
                        new { 
                            orderBy = parameters.OrderBy,
                            fields = parameters.Fields,
                            pageNumber = parameters.PageNumber + 1,
                            pageSize = parameters.PageSize });
                default:
                    return urlHelper.Link(routeName, 
                        new { 
                            orderBy = parameters.OrderBy,
                            fields = parameters.Fields,
                            pageNumber = parameters.PageNumber,
                            pageSize = parameters.PageSize });
            }
        }
        public void AddPaginationMetadata<T>(PagedList<T> data, CollectionParameters parameters, string routeName){
            var prevPage = data.HasPrevious ? CreateResourceUri(routeName, parameters, ResourceUriType.PreviousPage) : null;
            var nextPage = data.HasNext ? CreateResourceUri(routeName, parameters, ResourceUriType.NextPage) : null;

            var paginationMetadata = new {
                //orderBy = parameters.OrderBy,
                totalCount = data.TotalCount,
                pageSize = data.PageSize,
                curentPage = data.CurrentPage,
                totalPages = data.TotalPages,
                previousPageLink = prevPage,
                nextPageLink = nextPage
            };
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationMetadata));
        }
    }

    [Authorize(AuthenticationSchemes = "Cookies,Bearer")]
    public abstract class AllAuthApiController : BaseApiController
    {
        public AllAuthApiController(ILoggerFactory loggerFactory, IBusinessLogic logic,
            IUrlHelper urlHelper, 
            ITypeHelperService typeHelperService)
            : base(loggerFactory, logic, urlHelper, typeHelperService)
        { }
    }

    [Authorize(AuthenticationSchemes = "Cookies")]
    public abstract class CookieAuthApiController : BaseApiController
    {
        public CookieAuthApiController(ILoggerFactory loggerFactory, IBusinessLogic logic,
            IUrlHelper urlHelper, 
            ITypeHelperService typeHelperService)
            : base(loggerFactory, logic, urlHelper, typeHelperService)
        { }
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    public abstract class BearerAuthController : BaseApiController
    {
        public BearerAuthController(ILoggerFactory loggerFactory, IBusinessLogic logic,
            IUrlHelper urlHelper, 
            ITypeHelperService typeHelperService)
            : base(loggerFactory, logic, urlHelper, typeHelperService)
        { }
    }
}