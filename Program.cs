using System;
using System.Diagnostics;
using MySql.Data.MySqlClient;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(c =>
    {
        c.AddPolicy("AllowOrigin", options =>options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
    }
);

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", 
        builder => builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});


// Registrieren Sie die MySqlConnection als Scoped Service
builder.Services.AddScoped<MySqlConnection>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    string connectionString = configuration.GetConnectionString("EmployeeAppCon");
    return new MySqlConnection(connectionString);
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors("CorsPolicy");

app.Run();

