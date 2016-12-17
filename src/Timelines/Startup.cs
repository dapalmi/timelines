using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using Timelines.Automapper;
using Timelines.Domain;
using Timelines.Domain.Event;
using Timelines.Domain.Person;
using Timelines.Domain.Relationship;
using Timelines.Persistence;
using Timelines.Service;
using Timelines.ViewModels;

namespace Timelines
{
    public class Startup
    {
        private IHostingEnvironment _env;
        public Startup(IHostingEnvironment env)
        {
            _env = env;
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<TimelinesContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DevelopmentDatabase")));

            services.AddIdentity<IdentityUser, IdentityRole>(config =>
                {
                    config.User.RequireUniqueEmail = true;
                    config.Cookies.ApplicationCookie.LoginPath = "/auth/login";
                    config.Cookies.ApplicationCookie.Events = new CookieAuthenticationEvents()
                    {
                        OnRedirectToLogin = async ctx =>
                        {
                            if (ctx.Request.Path.StartsWithSegments("/api") && ctx.Response.StatusCode == 200)
                            {
                                ctx.Response.StatusCode = 401;
                            }
                            else
                            {
                                ctx.Response.Redirect(ctx.RedirectUri);
                            }
                            await Task.Yield();
                        }
                    };
                })
                .AddEntityFrameworkStores<TimelinesContext>();

            services.AddTransient<TimelinesContextSeedData>();

            services.AddScoped<EventRepository>();
            services.AddScoped<PersonRepository>();
            services.AddScoped<RelationshipRepository>();

            services.AddScoped<PersonService>();
            services.AddScoped<EventService>();
            services.AddScoped<RelationshipService>();
            services.AddScoped<TimelineService>();

            // Add framework services.
            services.AddMvc(config =>
                {
                    if (_env.IsProduction())
                    {
                        config.Filters.Add(new RequireHttpsAttribute());
                    }
                })
                .AddJsonOptions(config =>
                {
                    config.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, TimelinesContextSeedData seeder)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            Mapper.Initialize(config =>
            {
                config.CreateMap<Event, EventViewModel>().ReverseMap();
                config.CreateMap<Person, PersonViewModel>().ReverseMap();
                config.CreateMap<Relationship, RelationshipViewModel>().ReverseMap();
                config.CreateMap<Person, TimelineViewModel>()
                    .ForMember(t => t.Events, conf => conf.ResolveUsing<TimelineEventsCustomResolver>())
                    .ForMember(t => t.Parents, conf => conf.MapFrom(p => p.RelatedPersonRelationships
                        .Where(rpr => rpr.RelationshipType == RelationshipType.Child)
                        .Select(rpr => rpr.PersonId)))
                    .ForMember(t => t.Children, conf => conf.MapFrom(p => p.RelatedPersonRelationships
                        .Where(rpr => rpr.RelationshipType == RelationshipType.Parent)
                        .Select(rpr => rpr.PersonId)))
                    .ForMember(t => t.Spouse, conf => conf.MapFrom(p => p.RelatedPersonRelationships
                        .Where(rpr => rpr.RelationshipType == RelationshipType.Spouse)
                        .Select(rpr => rpr.PersonId)));
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseIdentity();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            seeder.EnsureSeedData().Wait();
        }
    }
}
