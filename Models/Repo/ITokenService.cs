using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ReLogin.Models;

namespace ReLogin.Models
{
  public  interface ITokenService
    {
        string GenerateToken(UserTokenService userToken);
        bool IsActiveToken(string key);

        SetTokenResponse SetToken(UserTokenService userToken);
    }
}
