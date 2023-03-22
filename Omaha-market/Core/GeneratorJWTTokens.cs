using Omaha_market.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omaha_market.Core
{
    public class GeneratorJWTTokens
    {
        public static IConfiguration configuration { get; set; }

        public GeneratorJWTTokens(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

        public string GenerateJWTToken(AccountModel account)
        {
            var Tokenhendler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(configuration["JWT:key"]);
            var descriptor = new SecurityTokenDescriptor
            {
               Claims= new Dictionary<string, object>
               {
                   {"Role",account.Role},
                   {"ID",account.ID.ToString()}
               },
                Audience = account.ID.ToString(),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };
            var token = Tokenhendler.CreateToken(descriptor);
            return Tokenhendler.WriteToken(token);
        }

    }
}
