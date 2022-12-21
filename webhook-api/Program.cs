using System.Configuration;
using System.Net;
using System.Security.Cryptography.Xml;
using Microsoft.EntityFrameworkCore;
using Polly.Contrib.WaitAndRetry;
using Polly;
using webhook_api.Models;
using webhook_api.Services;
using System.Reflection.PortableExecutable;
using webhook_api.Context;
using System;
using System.Text.Json.Serialization;
using webhook_api.Interfaces;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Text.Json;
using FluentAssertions.Common;
using Newtonsoft.Json;

namespace webhook_api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();


            builder.Services.AddControllersWithViews()
                .AddJsonOptions(options =>
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles
                );

            builder.Services.AddDbContext<WebhookDBContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("ConStr")));
            //builder.Services.AddHostedService<WebhookBackgroundService>();
            builder.Services.AddHttpClient<IWebhookService, WebhookService>(client => { });

            builder.Services.AddScoped<IDatabaseInterface, RealDatabase>();
            builder.Services.AddScoped<IWebhookService, WebhookService>();
            builder.Services.AddScoped<IHeaderMapper, HeaderMapper>();
            builder.Services.AddScoped<IWebhookStatusMapper, WebhookStatusMapper>();
            builder.Services.AddScoped<IWebhookConfigurationMapper, WebhookConfigurationMapper>();





            builder.Services.AddSwaggerGen();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();
            app.Run();
        }
    }
}