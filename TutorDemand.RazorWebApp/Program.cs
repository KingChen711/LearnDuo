using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TutorDemand.Business;
using TutorDemand.Business.Abstractions;
using TutorDemand.Data;
using TutorDemand.Data.Entities;
using TutorDemand.Data.Mappings;
using TutorDemand.Data.Utils;
using TutorDemand.RazorWebApp.Extensions;
using TutorDemand.RazorWebApp.Models;

var builder = WebApplication.CreateBuilder(args);

// Config appsettings
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
builder.Services.Configure<DefaultData>(builder.Configuration.GetSection("DefaultData"));

// Auto Mapper
var mapperConfig = new MapperConfiguration(mc => { mc.AddProfile<ProfilesMapper>(); });
builder.Services.AddSingleton(mapperConfig.CreateMapper());

// Add services to the container.
builder.Services.AddRazorPages()
        .AddRazorPagesOptions(options =>
        {
            options.Conventions.AddPageRoute("/Index", "");
        });

builder.Services.AddControllers();
builder.Services.ConfigureSqlContext(builder.Configuration);
builder.Services.ConfigureBusinesses();
builder.Services.RegisterMapsterConfiguration();

var app = builder.Build();

// Hook into application lifetime events and trigger only application fully started
app.Lifetime.ApplicationStarted.Register(async () =>
{
// Database Initialiser
    await app.InitializeDatabaseAsync();
});


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();

app.Run();