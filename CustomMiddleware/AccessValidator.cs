using Microsoft.AspNetCore.Http;
using ReLogin.Data;
using ReLogin.Models;
using System.Linq;
using System.Threading.Tasks;

namespace ReLogin.CustomMiddleware
{
    public class AccessValidator : IMiddleware
    {
        private readonly ITokenService _tokenService;
        private readonly AppDbContext _appDbContext;
        
        const string LOGINURL = "/api/Authentication/Authorize";

        public AccessValidator(ITokenService tokenService,AppDbContext appDbContext)
        {
            _tokenService = tokenService;
            _appDbContext = appDbContext;
    
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
         
            if (context is null)
            {
                throw new System.ArgumentNullException(nameof(context));
            }

            var TokenFromHeader = context.Request.Headers["key"].ToString();
            var currentPath = context.Request.Path.ToString().ToLower();

            if (LOGINURL.ToLower() == currentPath)
            {
                await next(context);
                return;
            }
            var isActive = _tokenService.IsActiveToken(TokenFromHeader);

            if (isActive)
            {
                await next(context);
                return;
            }
            else
            {
                if (CheckDb(TokenFromHeader, context))
                {
                    var setTokenResult = _tokenService.SetToken(new UserTokenService { TokenValue = TokenFromHeader });
                    if (setTokenResult.Result)
                    {
                        context.Response.Headers["key"] = setTokenResult.Token;
                        await next(context);
                        return;
                    }
                    else
                    {
                        context.Response.StatusCode = 401;
                        return;
                    }

                }
                else
                {
                    context.Response.StatusCode = 401;
                    return;
                }
            }
        }

        public bool CheckDb(string TokenFromHeader, HttpContext context)
        {
            if (string.IsNullOrEmpty(TokenFromHeader))
                return false;


            var dbToken = _appDbContext.Tokens.FirstOrDefault(x => x.TokenValue == TokenFromHeader);
            if (dbToken is null)
            {
                context.Response.StatusCode = 401;
                return false;
                
            }
            if (dbToken is not null && dbToken.IsRevoked)
            {
                context.Response.StatusCode = 401;
                return false;

            }
            return true;
        }

       
    }
}
