using Dapper_Crud.Models;
using Dapper_Crud.Repositories;
using Dapper_Crud.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
    options.Filters.Add(new AuthorizeFilter(policy));
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "JWT Auth Demo", Version = "v1" });

    // ?? Define the security scheme
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter JWT with Bearer into field (e.g., 'Bearer eyJhbGci...')",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    // ?? Require token for [Authorize] endpoints
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSingleton<IDbConnection>(sp => new SqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ICountryRepo, CountryRepository>();
//builder.Services.AddScoped<ICountryRepo, TestRepository>();
///IDbConnection connection = new SqlConnection(builder.Configuration.GetConnectionString("DefaultConnection"));
















builder.Configuration["Jwt:Key"] = "MySuperSecretKey_1234567890_AAA_BBB";
builder.Services.AddSingleton<TokenService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!);
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "srproskillbridge",
            ValidAudience = "srproskillbridge",
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };

        // Token invalidation check
        var tokenService = builder.Services.BuildServiceProvider().GetRequiredService<TokenService>();
        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = context =>
            {
                var token = context.SecurityToken as JwtSecurityToken;
                if (token != null && tokenService.IsTokenInvalidated(token.RawData))
                {
                    context.Fail("Token has been invalidated.");
                }
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();
  /*
   * builder.Services.AddAuthorization(options =>
{
    // Role-based (Admin already handled by [Authorize(Roles = "Admin")])
    // Policy-based
    options.AddPolicy("HRDepartmentOnly", policy =>
        policy.RequireClaim("Department", "HR"));
});*/
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
