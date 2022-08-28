using System.Text;
using AccountsViewer.API.Config;
using AccountsViewer.API.Models.Contexts;
using AccountsViewer.API.Reporting;
using AccountsViewer.API.Reporting.Interfaces;
using AccountsViewer.API.Repositories;
using AccountsViewer.API.Repositories.Interfaces;
using AccountsViewer.API.Services;
using AccountsViewer.API.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Custom configurations.
ConfigureServices(builder.Services);
ConfigureConfigs(builder.Services);
ConfigureJwtAuth(builder.Services);

builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("AccountsDB")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(policyBuilder =>
    policyBuilder
        .SetIsOriginAllowed(s => true)
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials());

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

void ConfigureServices(IServiceCollection services)
{
    services.AddScoped<IAuthService, AuthService>();
    services.AddScoped<IAccountService, AccountService>();
    services.AddScoped<IEntryService, EntryService>();
    services.AddScoped<ICryptoService, CryptoService>();
    services.AddScoped<IUnitOfWork, UnitOfWork>();
    services.AddScoped<IStatsReporter, StatsReporter>();
    services.AddScoped<IUserRepository, UserRepository>();
    services.AddScoped<IAccountRepository, AccountRepository>();
    services.AddScoped<IEntryRepository, EntryRepository>();
}

void ConfigureConfigs(IServiceCollection services)
{
    services.Configure<JwtConfig>(builder.Configuration.GetSection(JwtConfig.Property));
}

void ConfigureJwtAuth(IServiceCollection services)
{
    services
        .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(o =>
        {
            var jwtConfigs = builder.Configuration.GetSection(JwtConfig.Property).Get<JwtConfig>();

            o.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtConfigs.Issuer,
                ValidAudience = jwtConfigs.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfigs.Key)),
            };
        });
}