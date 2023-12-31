﻿using CompanyEmployees.Formatter.OutputFormatter;
using Contracts;
using LoggerService;
using Microsoft.EntityFrameworkCore;
using Repository;
using Service;
using Services.Contracts;

namespace CompanyEmployees.Extensions
{
  public static class ServiceExtensions
  {
    public static void ConfigureCors(this IServiceCollection services) =>
      services.AddCors(setupOptions =>
      {
        setupOptions.AddPolicy("CorsPolicy", builder =>
        {
          builder.AllowAnyOrigin();
          builder.AllowAnyMethod();
          builder.AllowAnyHeader();
        });
      });

    public static void ConfigureIISIntegration(this IServiceCollection services) =>
      services.Configure<IISOptions>(options =>
      {
      });

    public static void ConfigureLoggerService(this IServiceCollection services) =>
      services.AddSingleton<ILoggerManager, LoggerManager>();

    public static void ConfigureRepositoryManager(this IServiceCollection services) =>
      services.AddScoped<IRepositoryManager, RepositoryManager>();

    public static void ConfigureServiceManager(this IServiceCollection services) =>
      services.AddScoped<IServiceManager, ServiceManager>();

    public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration) =>
      services.AddDbContext<RepositoryContext>(options =>
        options.UseSqlServer(configuration.GetConnectionString("sqlConnection")));

    public static IMvcBuilder AddCustomCsvFormatter(this IMvcBuilder builder) =>
      builder.AddMvcOptions(options => options.OutputFormatters.Add(new CsvOutputFormatter()));
  }
}
