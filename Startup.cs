using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ProjectionBug
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services
                // In order to use the ASP.NET Core routing we need to add the routing services.
                .AddRouting()

                // Next we are adding our GraphQL server configuration.
                // We can host multiple named GraphQL server configurations
                // that can be exposed on different routes.
                .AddGraphQLServer()

                    // The query types are split into two classes,
                    // by splitting the types into several class we can organize 
                    // our query fields by topics and also am able to test
                    // them separately.
                    .AddQueryType()
                        .AddTypeExtension<Queries>()

                    .AddProjections();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // in order to expose our GraphQL schema we need to map the GraphQL server 
            // to a specific route. By default it is mapped onto /graphql.
            app
                .UseWebSockets()
                .UseRouting()
                .UseEndpoints(endpoint => endpoint.MapGraphQL());
        }
    }
}