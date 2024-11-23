using Confluent.Kafka;
using Gvz.Laboratory.ResearchService;
using Gvz.Laboratory.ResearchService.Abstractions;
using Gvz.Laboratory.ResearchService.Helpers;
using Gvz.Laboratory.ResearchService.Kafka;
using Gvz.Laboratory.ResearchService.Middleware;
using Gvz.Laboratory.ResearchService.Repositories;
using Gvz.Laboratory.ResearchService.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add JWT bearer authentication
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(nameof(JwtOptions)));
var jwtOptions = builder.Configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions?.SecretKey ?? "secretkeysecretkeysecretkeysecretkeysecretkeysecretkeysecretkey"))
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                context.Token = context.Request.Cookies["test-cookies"];
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddDbContext<GvzLaboratoryResearchServiceDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

builder.Services.AddScoped<IResearchService, ResearchService>();
builder.Services.AddScoped<IResearchRepository, ResearchRepository>();

builder.Services.AddScoped<IPartyService, PartyService>();
builder.Services.AddScoped<IPartyRepository, PartyRepository>();

builder.Services.AddScoped<IResearchResultsService, ResearchResultsService>();
builder.Services.AddScoped<IResearchResultsRepository, ResearchResultsRepository>();

var producerConfig = new ProducerConfig
{
    BootstrapServers = "kafka:29092"
};
builder.Services.AddSingleton<IProducer<Null, string>>(new ProducerBuilder<Null, string>(producerConfig).Build());
builder.Services.AddScoped<IKafkaProducer, KafkaProducer>();

var consumerConfig = new ConsumerConfig
{
    BootstrapServers = "kafka:29092",
    GroupId = "research-group-id",
    AutoOffsetReset = AutoOffsetReset.Earliest
};
builder.Services.AddSingleton(consumerConfig);

builder.Services.AddSingleton<AddProductKafkaConsumer>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<AddProductKafkaConsumer>());

builder.Services.AddSingleton<UpdateProductKafkaConsumer>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<UpdateProductKafkaConsumer>());

builder.Services.AddSingleton<DeleteProductKafkaConsumer>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<DeleteProductKafkaConsumer>());

builder.Services.AddSingleton<AddPartyKafkaConsumer>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<AddPartyKafkaConsumer>());

builder.Services.AddSingleton<UpdatePartyKafkaConsumer>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<UpdatePartyKafkaConsumer>());

builder.Services.AddSingleton<DeletePartyKafkaConsumer>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<DeletePartyKafkaConsumer>());

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseSwagger();
app.UseSwaggerUI();

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors(x =>
{
    x.WithHeaders().AllowAnyHeader();
    x.WithOrigins("http://localhost:3000");
    x.WithMethods().AllowAnyMethod();
});

app.Run();
