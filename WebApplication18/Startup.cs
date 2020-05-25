using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Owin;
using WebApplication18.DataAccess;
using WebApplication18.Helpers;
using WebApplication18.MiddleWares;

namespace WebApplication18
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
            var key = Encoding.ASCII.GetBytes(Configuration.GetSection("Appsettings:Token").Value);
            services.Configure<CloudinarySettings>(Configuration.GetSection("CloudinarySettings"));

    
            services.AddAutoMapper(typeof(Startup));
            /*services.AddCors(options =>
             {
                options.AddPolicy("CorsPolicy",
             builder => builder
                .AllowAnyOrigin()
                .AllowAnyHeader()
                );
            });*/
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                    builder.SetIsOriginAllowed(_ => true)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });

            services.AddScoped<IUserDal,EfUserDal>();
            services.AddScoped<IAuthRepositoryUserDal, EfAuthRepositoryUserDal>();
            services.AddScoped<IAuthRepositoryManagerDal, EfAuthRepositoryManagerDal>();
            services.AddScoped<IUserPhotoDal,EfUserPhotoDal>();
            //services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IMessageDal, EfMessageDal>();
            services.AddScoped<IManagerDal, EfManagerDal>();
            services.AddControllersWithViews().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false

                };
            });

            services.AddControllers();
            
            
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
    
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseRouting();
            app.UseCors();
            //app.UseCors(policyName: "CorsPolicy");
            //app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().AllowCredentials());
            //app.UseCors();
            //app.UseMiddleware<AuthenticationMiddleware>();
            app.UseAuthentication();
        
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
