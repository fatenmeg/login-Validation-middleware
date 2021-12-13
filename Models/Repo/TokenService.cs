using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ReLogin.Data;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReLogin.Models.Repo
{
    public class TokenService : ITokenService
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMemoryCache _memoryCache;
        private readonly IConfiguration _configuration;
        public TokenService(AppDbContext appDbContext, IMemoryCache memoryCache, IConfiguration configuration)
        {
            _appDbContext = appDbContext;
            _memoryCache = memoryCache;
            _configuration = configuration;
        }


        public string GenerateToken(UserTokenService userTokenService)
        {
            var result = string.Empty;

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken("Faten",
              "Faten", null,
              expires: DateTime.Now.AddMinutes(5),
              signingCredentials: credentials);


            var dbToken = new Token
            {

                TokenValue = new JwtSecurityTokenHandler().WriteToken(token),
                LiveTime = token.ValidFrom,
                IsRevoked = false,
                ReplacedBy = userTokenService.OldToken

            };


            _appDbContext.Tokens.Add(dbToken);
            _appDbContext.SaveChanges();

            return dbToken.TokenValue;
        }

        


        public SetTokenResponse SetToken(UserTokenService userTokenService)
        {
            var result = new SetTokenResponse();
            try
            {
                var GenertedToken = GenerateToken(userTokenService);
                var options =
                new MemoryCacheEntryOptions()
                        .SetAbsoluteExpiration(
                              TimeSpan.FromMinutes(5));

                _memoryCache.Set(GenertedToken, "token", TimeSpan.FromMinutes(.5));

                result.Token = GenertedToken;
                result.Result = true;
            }
            catch (Exception ex)
            {

                throw;
            }
            return result;
        }


        public bool IsActiveToken(string key)
        {
            var result = _memoryCache.Get(key);
            if (result is not null)
            {
                return true;
            }
            return false;
        }
    }
}