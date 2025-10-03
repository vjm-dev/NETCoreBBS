using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.WebEncoders;
using NetCoreBBS;
using NetCoreBBS.Entities;
using NetCoreBBS.Infrastructure;
using NetCoreBBS.Infrastructure.Repositories;
using NetCoreBBS.Interfaces;
using NetCoreBBS.Middleware;
using NLog;
using NLog.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Unicode;

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

try
{
    logger.Debug("init main");

    var builder = WebApplication.CreateBuilder(args);

    // Configure services
    builder.Services.AddDbContext<DataContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

    builder.Services.AddIdentity<User, IdentityRole>(options =>
    {
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
    }).AddEntityFrameworkStores<DataContext>().AddDefaultTokenProviders();

    // Add framework services
    builder.Services.AddControllersWithViews();

    // Configure dependency inyection
    builder.Services.AddScoped<IRepository<TopicNode>, Repository<TopicNode>>();
    builder.Services.AddScoped<ITopicRepository, TopicRepository>();
    builder.Services.AddScoped<ITopicReplyRepository, TopicReplyRepository>();
    builder.Services.AddScoped<IUserServices, UserServices>();
    builder.Services.AddScoped<UserServices>();
    builder.Services.AddMemoryCache();

    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("Admin", policy =>
            policy.RequireClaim("Admin", "Allowed"));
    });

    // Configure encoding
    builder.Services.Configure<WebEncoderOptions>(options =>
    {
        options.TextEncoderSettings = new TextEncoderSettings(UnicodeRanges.All);
    });

    // Configure logging
    builder.Logging.ClearProviders();
    builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
    builder.Host.UseNLog();

    var app = builder.Build();

    // Configure HTTP pipeline
    app.UseRequestIPMiddleware();

    // Initialize database
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<DataContext>();
        db.Database.Migrate();
        if (!db.TopicNodes.Any())
        {
            db.TopicNodes.AddRange(GetTopicNodes());
            db.SaveChanges();
        }
    }

    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }

    app.UseStatusCodePages();
    app.UseStaticFiles();
    app.UseRouting();
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllerRoute(
        name: "areaRoute",
        pattern: "{area:exists}/{controller}/{action}",
        defaults: new { action = "Index" });

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    app.Run();
}
catch (Exception ex)
{
    logger.Error(ex, "Stopped program because of exception");
    throw;
}
finally
{
    LogManager.Shutdown();
}

IEnumerable<TopicNode> GetTopicNodes()
{
    return new List<TopicNode>()
    {
        new TopicNode() { Name=".NET Core", NodeName="", ParentId=0, Order=1, CreateOn=DateTime.UtcNow },
        new TopicNode() { Name=".NET Core", NodeName="netcore", ParentId=1, Order=1, CreateOn=DateTime.UtcNow },
        new TopicNode() { Name="ASP.NET Core", NodeName="aspnetcore", ParentId=1, Order=1, CreateOn=DateTime.UtcNow }
    };
}
