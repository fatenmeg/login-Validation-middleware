using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ReLogin.Data;
using ReLogin.Models;
using System.Linq;

namespace ReLogin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly ITokenService _token;
        private readonly AppDbContext _appDbContext;
        private readonly IConfiguration _configuration;

        public AuthenticationController(ITokenService token, AppDbContext appDbContext, IConfiguration configuration)
        {
            _token = token;
            _appDbContext = appDbContext;
            _configuration = configuration;
        }
        [HttpPost("Authorize")]
        public IActionResult Authorize([FromBody]UserTokenService userTokenService )
        {
            var DbUser = _appDbContext.Users.FirstOrDefault
                  (x => x.UserName == userTokenService.UserName);
            if (DbUser == null)
            {
                return Ok("Incorrect credential");

            }
            if (DbUser.Password != userTokenService.Password)
            {
                return Ok("Incorrect credential");
            }
            userTokenService.UserId = DbUser.Id;
            return Ok(_token.SetToken(userTokenService));

        }

    }
}
