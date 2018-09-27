using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Login.Services.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace LoginAndScheduling.Services
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddDbContext<UserContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                   .AddCookie(options =>
                   {
                       options.Cookie.Expiration = TimeSpan.FromMinutes(2);

                   })
                   .AddJwtBearer(jwtBearerOptions =>
                   {
                       jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters()
                       {

                           ValidateActor = true,
                           ValidateAudience = true,
                           ValidateLifetime = true,
                           ClockSkew = TimeSpan.Zero,
                           ValidateIssuerSigningKey = true,
                           ValidIssuer = Configuration["Token:Issuer"],
                           ValidAudience = Configuration["Token:Audience"],
                           IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Token:Key"]))
                       };
                   });
            var corsBuilder = new CorsPolicyBuilder();
            corsBuilder.AllowAnyHeader();
            corsBuilder.AllowAnyMethod();
            corsBuilder.AllowAnyOrigin(); 
            corsBuilder.AllowCredentials();

            services.AddCors(options =>
            {
                options.AddPolicy("SiteCorsPolicy", corsBuilder.Build());
            });



        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseMvc();
            app.UseCors("SiteCorsPolicy");
        }
    }
}
