using PManager.Core.Interfaces;
using PManager.Core.Services;
using PManager.Cryptography.Interfaces;
using PManager.Cryptography.Services;
using PManager.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IDatabaseContext>(provider =>
    new DatabaseContext("YourConnectionStringHere"));
builder.Services.AddScoped<IQueryManager, QueryManager>();

var encryptionSettings = builder.Configuration.GetSection("EncryptionSettings");
builder.Services.AddSingleton<byte[]>(Convert.FromBase64String(encryptionSettings["Key"]));
builder.Services.AddSingleton<byte[]>(Convert.FromBase64String(encryptionSettings["IV"]));
builder.Services.AddScoped<IEncryptor, Encryptor>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseEndpoints(endpoints => endpoints.MapControllers());

app.Run();
