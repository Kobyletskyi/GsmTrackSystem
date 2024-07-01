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
    public class CookieTokenAuthenticate : Attribute, IAuthenticationFilter
    {
        private IAuthService _authService;
        public CookieTokenAuthenticate()
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
        
        public CookieTokenAuthenticate(Scopes scope)
        {
            scopeName = Enum.GetName(typeof(Scopes), scope);
        }
        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            try
            {
                HttpRequestMessage request = context.Request;
                string authorization = getCookie(request, "token");

                if (String.IsNullOrEmpty(authorization))
                {
                    context.ErrorResult = new AuthenticationFailureResult("Missing autorization cookie", request);
                    return;
                }

                ClaimsPrincipal principal = await _authService.ValidateTokenAsync(authorization);
                if (principal!=null)
                {
                    if (!String.IsNullOrWhiteSpace(scopeName))
                    {
                        if (!principal.Claims.Where(c => c.Type == AuthService.ScopeClaim).Select(c => c.Value).Contains(scopeName))
                        {
                            context.ErrorResult = new ForbiddenResult("Access forbidden", request);
                            return;
                        }
                    }
                    context.Principal = principal;
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

        private string getCookie(HttpRequestMessage request, string cookieName)
        {
            CookieHeaderValue cookie = request.Headers.GetCookies(cookieName).FirstOrDefault();
            if (cookie != null)
                return cookie[cookieName].Value;

            return null;
        }
    }
}