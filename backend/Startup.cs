using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using PManager.Core;
using PManager.Data;
using PManager.Cryptography;

namespace PManager
{
  public class Startup
  {
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddControllers();
      services.AddScoped<IDatabaseContext, DatabaseContext>();
      services.AddScoped<IQueryManager, QueryManager>();
      services.AddScoped<IEncryptor, Encryptor>();
    }

    public void Configure(IApplicationBuilder app)
    {
      app.UseRouting();
      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
    }
  }
}
