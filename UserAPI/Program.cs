using System.Collections.Generic;
using UserAPI.Interfaces;
using UserAPI.ViewModels.Common;
using UserAPI;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UserAPI.Helper;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using StackExchange.Redis;
using GraphQL;
using GraphQL.Types;
using GraphQL.Server;
using UserAPI.Middlewere;
using GraphQL.Server.Ui.Playground;
using Microsoft.Extensions.Options;


var builder = WebApplication.CreateBuilder(args);
builder.Services.RegisterServices(Assembly.GetExecutingAssembly());
// Add services to the container.
builder.Services.Configure<JWTSettings>(builder.Configuration.GetSection("JWT"));
var jwtSetting = builder.Configuration.GetSection("JWT").Get<JWTSettings>();
// Add services to the container.
builder.Services.AddSingleton<DapperContext>();
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
builder.Services.AddGraphQL(builder =>
{

    builder.AddSystemTextJson(); 
    builder.AddGraphTypes(Assembly.GetExecutingAssembly()); 
});
builder.Services.AddAntiforgery(options => options.HeaderName = "X-CSRF-TOKEN");
builder.Services.AddSingleton<UserType>();
builder.Services.AddSingleton<DynamicGraphQLQuery>();
builder.Services.AddSingleton<ISchema, DynamicGraphQLSchema>();
builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(option =>
{
    option.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateLifetime = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSetting?.Issuer,
        ValidAudience = jwtSetting?.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSetting?.Key))
    };
});
builder.Services.AddStackExchangeRedisCache(option =>
{
    option.Configuration = builder.Configuration.GetConnectionString("Redis");
    option.InstanceName = "SampleInstance";
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(option =>
{
    option.AddPolicy("allowedOrigin", policy =>
    {
        policy.WithOrigins("https://localhost:7052").AllowAnyHeader().AllowAnyMethod();
    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("allowedOrigin");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
//app.UseMiddleware<GraphQLMetricsMiddleware>();
app.MapGraphQL("/graphql");

// Enable GraphQL Playground for UI testing (optional)
app.UseGraphQLPlayground();
app.Run();
