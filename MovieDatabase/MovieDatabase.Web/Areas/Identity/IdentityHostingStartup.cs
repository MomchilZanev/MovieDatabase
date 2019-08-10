using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(MovieDatabase.Web.Areas.Identity.IdentityHostingStartup))]
namespace MovieDatabase.Web.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}