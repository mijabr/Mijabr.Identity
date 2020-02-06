// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using IdentityServer4;
using IdentityServer4.Quickstart.UI;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProxyKit;

namespace Mijabr.Identity
{
    public class Startup
    {
        public IWebHostEnvironment Environment { get; }
        public IConfiguration Configuration { get; }

        public Startup(IWebHostEnvironment environment, IConfiguration configuration)
        {
            Environment = environment;
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var host = System.Environment.GetEnvironmentVariable("host") ?? "localtest.me";
            Console.WriteLine($"Using host {host}");

            services.AddProxy();

            services.AddControllersWithViews();

            services.AddCors(options =>
            {
                options.AddPolicy("CORS",
                    b => b
                        .SetIsOriginAllowed(origin => origin.EndsWith(host))
                        .AllowAnyMethod()
                        .AllowCredentials()
                        .AllowAnyHeader()
                        .Build());
            });

            // configures IIS out-of-proc settings (see https://github.com/aspnet/AspNetCore/issues/14882)
            services.Configure<IISOptions>(iis =>
            {
                iis.AuthenticationDisplayName = "Windows";
                iis.AutomaticAuthentication = false;
            });

            // configures IIS in-proc settings
            services.Configure<IISServerOptions>(iis =>
            {
                iis.AuthenticationDisplayName = "Windows";
                iis.AutomaticAuthentication = false;
            });

            var builder = services.AddIdentityServer(options => 
            {
                options.IssuerUri = "http://identity";
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
            })
                .AddTestUsers(TestUsers.Users);

            // in-memory, code config
            builder.AddInMemoryIdentityResources(Config.Ids);
            builder.AddInMemoryApiResources(Config.Apis);
            builder.AddInMemoryClients(Config.Clients);

            // or in-memory, json config
            //builder.AddInMemoryIdentityResources(Configuration.GetSection("IdentityResources"));
            //builder.AddInMemoryApiResources(Configuration.GetSection("ApiResources"));
            //builder.AddInMemoryClients(Configuration.GetSection("clients"));

            // not recommended for production - you need to store your key material somewhere secure
            builder.AddDeveloperSigningCredential();

            services.AddAuthentication()
                .AddGoogle(options =>
                {
                    options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;

                    // register your IdentityServer with Google at https://console.developers.google.com
                    // enable the Google+ API
                    // set the redirect URI to http://localhost:5000/signin-google
                    options.ClientId = "copy client ID from Google here";
                    options.ClientSecret = "copy client secret from Google here";
                });
        }

        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("CORS");

            app.UseStaticFiles();
            app.UseRouting();
            app.UseIdentityServer();
            app.UseAuthorization();

            //app.Use(async (context, next) =>
            //{
            //    if (context.Request.Path.Value.Contains("/api/"))
            //    {
            //        var auth = context.RequestServices.GetService<IAuthorizationService>();
            //        auth.AuthorizeAsync(context.User,)
            //    }

            //    await next();
            //});

            app.UseEndpoints(endpoints => endpoints.MapDefaultControllerRoute());

            app.MapWhen(context => context.Request.Path.Equals("/"), home =>
            {
                home.RunProxy(context => context
                    .ForwardTo("http://home/home/")
                    .AddXForwardedHeaders()
                    .Send());
            });

            app.Map("/home", home =>
            {
                home.RunProxy(context => context
                    .ForwardTo("http://home/home/")
                    .AddXForwardedHeaders()
                    .Send());
            });

            app.Map("/scrabble", home =>
            {
                home.RunProxy(context => context
                    .ForwardTo("http://scrabble/scrabble/")
                    .AddXForwardedHeaders()
                    .Send());
            });
        }
    }
}