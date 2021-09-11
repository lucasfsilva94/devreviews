using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DevReviews.API.Persistence;
using DevReviews.API.Persistence.Repositories;
using DevReviews.API.Profiles;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Interfaces;
using Microsoft.OpenApi.Models;

namespace DevReviews.API
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
      var connectionString = Configuration.GetValue<string>("DevReviewsCN");

      // Comentado o uso do Azure SQL Server para utilizar o banco de dados In-Memory
      //services.AddDbContext<DevReviewsDbContext>(o => o.UseSqlServer(connectionString));
      services.AddDbContext<DevReviewsDbContext>(opt => opt.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()),
                                                                                 ServiceLifetime.Singleton,
                                                                                 ServiceLifetime.Singleton);

      services.AddScoped<IProductRepository, ProductRepository>();
      services.AddAutoMapper(typeof(ProductProfile));
      services.AddControllers();
      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
          Title = "DevReviews",
          Description = "Aplicação responsável por gerenciar avaliações de produtos de um e-commerce.",
          Version = "v1",
          Contact = new OpenApiContact
          {
            Name = "Lucas Faria",
            Email = "lucasfsilva94@hotmail.com",
            Url = new Uri("https://github.com/lucasfsilva94")
          },
          Extensions = new Dictionary<string, IOpenApiExtension>
          {
            {"x-logo", new OpenApiObject
              {
                  {"url", new OpenApiString("https://user-images.githubusercontent.com/22107794/132960600-bf0a778b-9b48-40b5-8614-4294fcc4ed32.png")},
                  {"altText", new OpenApiString("DevReviews API")}
              }
            }
          }
        });

        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        c.IncludeXmlComments(xmlPath, true);
        c.EnableAnnotations();
      });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseSwagger();
      app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DevReviews"));
      app.UseReDoc(c =>
      {
        c.DocumentTitle = "DevReviews API - Documentation";
        c.SpecUrl = "/swagger/v1/swagger.json";
      });

      app.UseHttpsRedirection();

      app.UseRouting();

      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
    }
  }
}
