using Discount.API.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Discount.API {
  public class Program {
    public static void Main(string[] args) {
      CreateHostBuilder(args).Build()
                             .MigrateDatabase<Program>()
                             .Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) => Host.CreateDefaultBuilder(args)
                                                                       .ConfigureWebHostDefaults(webBuilder => {
                                                                         webBuilder.UseStartup<Startup>();
                                                                       });
  }
}
