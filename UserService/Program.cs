using System.Collections.Generic;
using UserService.Interfaces;
using UserService.ViewModels.Common;
using UserService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UserService.Helper;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using StackExchange.Redis;
using GraphQL;
using GraphQL.Types;
using GraphQL.Server;
using UserService.Middlewere;
using GraphQL.Server.Ui.Playground;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using Hangfire;
using Hangfire.MemoryStorage;


var builder = WebApplication.CreateBuilder(args);
builder.Services.RegisterServices(Assembly.GetExecutingAssembly());
// Add services to the container.
builder.Services.Configure<JWTSettings>(builder.Configuration.GetSection("JWT"));
var jwtSetting = builder.Configuration.GetSection("JWT").Get<JWTSettings>();
// Add services to the container.
builder.Services.AddSingleton<DapperContext>();
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
builder.Services.AddAutoMapper(typeof(MappingProfile));
//builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining(typeof(MoneyTransferHandler)));
//builder.Services.AddScoped<IRabbitMqBus, RabbitMqBus>();
builder.Services.AddSingleton<IConnection>(sp =>
{
    var factory = new ConnectionFactory()
    {
        Uri = new Uri("amqps://kvpmuftr:ibuumh3S1nsMCBp2UC8oBV1kvpUxNAlf@lionfish.rmq.cloudamqp.com/kvpmuftr"),
        ConsumerDispatchConcurrency = 1
    }; // Update with RabbitMQ settings
    return factory.CreateConnectionAsync().Result;
});
builder.Services.AddSingleton<RabbitMQHelper>();
builder.Services.AddHangfire(config =>
{
    config.UseMemoryStorage(); // Use persistent storage for production
});
builder.Services.AddHangfireServer();
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
var rabbitMqConsumer = app.Services.GetRequiredService<RabbitMQHelper>();
await rabbitMqConsumer.ConsumeMessage("queue1");
await rabbitMqConsumer.ConsumeMessage("queue2");

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
app.UseHangfireDashboard();
app.MapGet("/", () => "Hello, Hangfire!");
app.Run();
