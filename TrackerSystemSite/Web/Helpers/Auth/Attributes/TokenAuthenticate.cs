using BusinessLayer;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;

namespace Web.Helpers.Auth
{
    public class TokenAuthenticate : Attribute, IAuthenticationFilter
    {
        private IAuthService _authService;
        public TokenAuthenticate()
        {
            IBusinessLogic logic = (IBusinessLogic)GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof (IBusinessLogic));
            _authService = new AuthService(logic);
        }
        

        private string scopeName;

        public bool AllowMultiple
        {
            get
            {
                return false;
            }
        }
        
        public TokenAuthenticate(Scopes scope)
        {
            scopeName = Enum.GetName(typeof(Scopes), scope);
        }
        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            try
            {

                HttpRequestMessage request = context.Request;
                AuthenticationHeaderValue authorization = request.Headers.Authorization;

                if (authorization == null)
                {
                    //ToDo: use HttpResponseMessage as on RequareHttps
                    context.ErrorResult = new AuthenticationFailureResult("Missing autorization header", request);
                    return;
                }
                if (authorization.Scheme != "Bearer")
                {
                    context.ErrorResult = new AuthenticationFailureResult("Invalid autorization scheme", request);
                    return;
                }
                if (String.IsNullOrEmpty(authorization.Parameter))
                {
                    context.ErrorResult = new AuthenticationFailureResult("Missing Token", request);
                    return;
                }

                ClaimsPrincipal principal = await _authService.ValidateTokenAsync(authorization.Parameter);
                if (principal!=null)
                {
                    if (!String.IsNullOrWhiteSpace(scopeName))
                    {
                        if (!principal.Claims.Where(c => c.Type == AuthService.ScopeClaim).Select(c => c.Value).Contains(scopeName))
                        {
                            context.ErrorResult = new ForbiddenResult("Access forbidden", request);
                        }
                    }
                }
                else
                {
                    context.ErrorResult = new AuthenticationFailureResult("Invalid Token", request);
                }
            }
            catch (Exception ex)
            {
                context.ErrorResult = new AuthenticationFailureResult("Exception: \n" + ex.Message, context.Request);
            }
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            var challenge = new AuthenticationHeaderValue("Bearer");
            context.Result = new AddChallengeOnUnauthorizedResult(challenge, context.Result);
            return Task.FromResult(0);
        }
    }
    
}