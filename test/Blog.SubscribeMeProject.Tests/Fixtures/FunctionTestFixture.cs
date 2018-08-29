using Blog.SubscribeMeProject.Infrastructure.Repositories;
using Blog.SubscribeMeProject.Tests.Fixtures;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.SubscribeMeProject.Tests
{
    public class FunctionalTestFixture<TStartup> : WebApplicationFactory<Startup> where TStartup : class
    {
        public FunctionalTestFixture() { }
 
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            var serviceProvider = new ServiceCollection()
                   .BuildServiceProvider();
            
            builder.ConfigureServices(services =>
            {
                services.AddTransient<ISubscriptionRepository, FakeSubscriptionRepository>();
                services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

                // Build the service provider.
                var sp = services.BuildServiceProvider();
            });


        }
    }
}