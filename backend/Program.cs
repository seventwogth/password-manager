using PManager.Core.Interfaces;
using PManager.Core.Services;
using PManager.Cryptography.Interfaces;
using PManager.Cryptography.Services;
using PManager.Data;
using PManager.API.Token;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IDatabaseContext>(provider =>
    new DatabaseContext("Data Source=passwords.db"));
builder.Services.AddScoped<IQueryManager, QueryManager>();

builder.Services.Configure<JWTSettings>(builder.Configuration.GetSection("JwtSettings"));

var encryptionSettings = builder.Configuration.GetSection("EncryptionSettings");
builder.Services.AddSingleton<byte[]>(Convert.FromBase64String(encryptionSettings["Key"]));
builder.Services.AddSingleton<byte[]>(Convert.FromBase64String(encryptionSettings["IV"]));
builder.Services.AddSingleton<JWTService>();
builder.Services.AddScoped<IEncryptor, Encryptor>();

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"], // Из appsettings.json
            ValidAudience = builder.Configuration["JwtSettings:Audience"], // Из appsettings.json
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                builder.Configuration["JwtSettings:Key"])) // Из appsettings.json
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
