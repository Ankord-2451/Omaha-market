using Microsoft.EntityFrameworkCore;
using System.Text;
using Omaha_market.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Omaha_market.Middleware;
using Stripe;

namespace Omaha_market
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddControllersWithViews();
            services.AddRazorPages();
            //session
            services.AddDistributedMemoryCache();
            services.AddSession();
            
            services.AddMvc();
            services.AddHttpClient();

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
              { 
                 x.RequireHttpsMetadata = true;
                 x.SaveToken = true;
                 x.TokenValidationParameters = new TokenValidationParameters
                 {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:key"])),
                    ValidateAudience = false,
                   ValidateIssuer = false
                 };

              });
            services.Configure<StripeSettings>(Configuration.GetSection("Stripe"));
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                 app.UseHsts();
            }

            StripeConfiguration.ApiKey=Configuration.GetSection("Stripe")["SecretKey"];

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            //session
            app.UseSession();

            app.UseMiddleware<TokenToContextMiddleware>();
           
           
            app.UseAuthentication();
            app.UseAuthorization();
           

            app.UseEndpoints(endpoints =>
            {
               endpoints.MapControllerRoute(
                  name: "default",
                  pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
