using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace Omaha_market.Middleware
{
    public class TokenToContextMiddleware
    {
        private readonly RequestDelegate _next;
        private string token { get; set; }

        public static IConfiguration _configuration { get; set; }
        public TokenToContextMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task Invoke(HttpContext context)
        {

            token = context.Session.GetString("token");

            if (token != null)
            {
                context.Request.Headers["Authorization"] = token;
            }

            await _next(context);
        }
    }
}
