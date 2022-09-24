using System.Text;
using CompaniesRatingWebApi.Models.DatabaseSettingsForStores;
using CompaniesRatingWebApi.Services.AuthService;
using CompaniesRatingWebApi.Services.CompanyServices;
using CompaniesRatingWebApi.Services.LocationServices;
using CompaniesRatingWebApi.Services.UserServices;
using CompaniesRatingWebApi.Services.ReviewServices;
using CompaniesRatingWebApi.Services.TokenService;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.Configure<StoresDatabaseSettings>(
    builder.Configuration.GetSection(nameof(StoresDatabaseSettings)));

builder.Services.AddSingleton<IStoresDatabaseSettings>(sp =>
    sp.GetRequiredService<IOptions<StoresDatabaseSettings>>().Value);

builder.Services.AddSingleton<IMongoClient>(s =>
    new MongoClient(builder.Configuration.GetValue<string>("StoresDatabaseSettings:ConnectionString")));

builder.Services.AddAuthentication(opt =>     //
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        
        ValidIssuer = "https://localhost:7234",
        ValidAudience = "https://localhost:7234",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@100"))
    };
});

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>(); //

builder.Services.AddScoped<ICompanyService, CompanyService>();

builder.Services.AddScoped<ILocationService, LocationService>();

builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IReviewService, ReviewService>();

builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors((setup) =>
{
    setup.AddPolicy("default", (options) =>
    {
        options.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin();
    });
});

var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("default");

app.UseHttpsRedirection();

app.UseAuthentication(); //

app.UseAuthorization();

app.MapControllers();

app.Run();