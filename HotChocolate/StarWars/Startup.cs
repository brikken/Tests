using System.Security.Claims;
using HotChocolate;
using HotChocolate.AspNetCore;
using HotChocolate.Subscriptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using StarWars.Data;
using StarWars.Types;
using Swashbuckle.AspNetCore.Swagger;

namespace StarWars
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
            });

            // Add the custom services like repositories etc ...
            services.AddSingleton<CharacterRepository>();
            services.AddSingleton<ReviewRepository>();

            services.AddSingleton<Query>();
            services.AddSingleton<Mutation>();
            services.AddSingleton<Subscription>();

            // Add in-memory event provider
            var eventRegistry = new InMemoryEventRegistry();
            services.AddSingleton<IEventRegistry>(eventRegistry);
            services.AddSingleton<IEventSender>(eventRegistry);

            // Add GraphQL Services
            services.AddGraphQL(sp => HotChocolate.Schema.Create(c =>
            {
                c.RegisterServiceProvider(sp);

                // Adds the authorize directive and
                // enables the authorization middleware.
                c.RegisterAuthorizeDirectiveType();

                c.RegisterQueryType<QueryType>();
                c.RegisterMutationType<MutationType>();
                c.RegisterSubscriptionType<SubscriptionType>();

                c.RegisterType<HumanType>();
                c.RegisterType<DroidType>();
                c.RegisterType<EpisodeType>();
            }));

            // Add Authorization Policies
            services.AddAuthorization(options =>
            {
                options.AddPolicy("HasCountry", policy =>
                    policy.RequireAssertion(context =>
                        context.User.HasClaim(c =>
                            (c.Type == ClaimTypes.Country))));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
            app.UseWebSockets();
            app.UseGraphQL(new QueryMiddlewareOptions() { Path = new Microsoft.AspNetCore.Http.PathString("/graphql") } );
            app.UseGraphiQL();
            app.UsePlayground();

            /*
            Note: comment app.UseGraphQL("/graphql"); and uncomment this
            section in order to simulare a user that has a country claim and
            passes the configured authorization rule.
            app.UseGraphQL(new QueryMiddlewareOptions
            {
                OnCreateRequest = (ctx, builder, ct) =>
                {
                    var identity = new ClaimsIdentity();
                    identity.AddClaim(new Claim(ClaimTypes.Country, "us"));
                    ctx.User.AddIdentity(identity);
                    return Task.CompletedTask;
                }
            });
            */
        }
    }
}
