using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;
using RMAN_test.Server.Data;
using RMAN_test.Server.Models;
using RMAN_test.Server.Repositories;
using RMAN_test.Server.Settings;

var builder = WebApplication.CreateBuilder(args);

// Load MongoDB settings
var mongoDbSettings = builder.Configuration.GetSection(nameof(MongoDbConfig)).Get<MongoDbConfig>();

// Configure Identity with MongoDB stores
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
       .AddMongoDbStores<ApplicationUser, ApplicationRole, Guid>
       (
           mongoDbSettings.ConnectionString, mongoDbSettings.Name
       )
       .AddDefaultTokenProviders();


// Register MongoDbContext
builder.Services.AddSingleton<IMongoClient>(serviceProvider =>
{
    return new MongoClient(mongoDbSettings.ConnectionString);
});

builder.Services.AddScoped(serviceProvider =>
{
    var mongoClient = serviceProvider.GetRequiredService<IMongoClient>();
    var database = mongoClient.GetDatabase(mongoDbSettings.Name);
    return new MongoDbContext(mongoDbSettings.ConnectionString, mongoDbSettings.Name);
});



builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Ensure HTTPS is being used
    options.Cookie.SameSite = SameSiteMode.None; // Required for cross-site cookies
    options.Cookie.Name = ".AspNetCore.Identity.Application";
    // Remove LoginPath, LogoutPath, and AccessDeniedPath since you're handling this in React
});

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {
            builder.WithOrigins("https://localhost:5173")
                   .AllowAnyHeader()
                   .AllowAnyMethod()
                   .AllowCredentials();
        });
});

builder.Services.AddScoped<IUserRepository, UserRepository>();

// Add controllers and Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors("AllowSpecificOrigin");

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapFallbackToFile("/index.html");
});

app.Run();
