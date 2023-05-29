using System.Collections.Generic;
using System.Security.Claims;
using WebApiIdentity.Models;

namespace WebApiIdentity.Tokens
{
    public interface IJwtGenerator
    {
        string GenerateToken(List<Claim> claims);
    }
}
