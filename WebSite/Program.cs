
using Application.Services;
using Application.Services.Users.Commands.AddUser;
using Application.UnitOfWork;
using Domain.IRepositories;
using Domain.Validations;
using HealthChecks.UI.Client;
using Infrastructure.Repositories;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Tokens;
using Persistence;
using Prometheus;
using System.Reflection;
using System.Text;
using WebSite.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllersWithViews();
//Database
string Connection = builder.Configuration.GetConnectionString("sqlServer");
builder.Services.AddSqlServer<DbContextEF>(Connection);

builder.Services.AddHealthChecks()
    .AddSqlServer(builder.Configuration.GetConnectionString("sqlServer"));

builder.Services.AddSignalR();


builder.Services.AddAuthentication(Option =>
{
    Option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    Option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    Option.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(ConfigOption =>
{
    ConfigOption.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidIssuer = builder.Configuration["JWTConfig:issuer"],
        ValidAudience = builder.Configuration["JWTConfig:audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWTConfig:Key"])),
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true
    };
    ConfigOption.SaveToken = true;
    ConfigOption.Events = new JwtBearerEvents()
    {
        OnAuthenticationFailed = context =>
        {
            return Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
            return Task.CompletedTask;
        },
        OnChallenge = context =>
        {
            return Task.CompletedTask;
        },
        OnMessageReceived = context =>
        {
            return Task.CompletedTask;
        },
        OnForbidden = context =>
        {
            return Task.CompletedTask;
        },
    };
});

builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddScoped<IUnitOfWork, DbContextEF>();
builder.Services.AddScoped<IUserRepositorie , UserRepositorie>();
builder.Services.AddScoped<IBankAccountRepositorie, BankAccountRepositorie>();
builder.Services.AddScoped<IBankSafeRepositorie, BankSafeRepositorie>();
builder.Services.AddScoped<IUserAndNumberOfShareRepositorie, UserAndNumberOfShareRepositorie>();
builder.Services.AddScoped<IBankSafeTransactionsRepositorie, BankSafeTransactionsRepositorie>();
builder.Services.AddScoped<IBankSafeDocumentRepositorie, BankSafeDocumentRepositorie>();
builder.Services.AddScoped<ILoanRepositorie, LoanRepositorie>();
builder.Services.AddScoped<IChatRoomRepositorie, ChatRoomRepositorie>();


builder.Services.RegisterApplication();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    });

    app.UseRouting();
    app.UseEndpoints(endpoints =>
    {
        endpoints.MapHealthChecks("/health", new HealthCheckOptions
        {
            Predicate = _ => true,
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });
        endpoints.MapHub<SiteChatHub>("/chathub");
        endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
    });
}
app.UseMetricServer();
app.UseHttpMetrics();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
