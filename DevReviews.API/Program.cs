using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Sinks.MSSqlServer;

namespace DevReviews.API
{
  public class Program
  {
    public static void Main(string[] args)
    {
      CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            // Comentado a inicialização do SERILOG
            // .ConfigureAppConfiguration((hostingContext, config) =>
            // {
            //   var settings = config.Build();
            //   Serilog.Log.Logger = new LoggerConfiguration().WriteTo
            //                                                 .MSSqlServer(settings.GetValue<string>("DevReviewsCN"),
            //                                                             sinkOptions: new MSSqlServerSinkOptions()
            //                                                             {
            //                                                               TableName = "Logs",
            //                                                               AutoCreateSqlTable = true
            //                                                             }).CreateLogger();
            // })
            // .UseSerilog()
            .ConfigureWebHostDefaults(webBuilder =>
            {
              webBuilder.UseStartup<Startup>();
            });
  }
}
