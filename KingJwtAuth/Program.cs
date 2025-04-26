using KingJwtAuth.Context;
using KingJwtAuth.Services;
using KingJwtAuth.Services.UserLogs;
using KingJwtAuth.Services.UserRefreshToken;
using KingJwtAuth.Services.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<UserRefreshTokenService>();
builder.Services.AddScoped<UserLogsService>();
builder.Services.AddScoped<UsersSuspiciousService>();


builder.Services.AddScoped<IDataBaseContext, DataBaseContext>();
var conStr = builder.Configuration.GetConnectionString("localhost");
builder.Services.AddEntityFrameworkSqlServer().AddDbContext<DataBaseContext>(x => x.UseSqlServer(conStr));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
