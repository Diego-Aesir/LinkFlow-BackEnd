using UserAPI.Models;
using UserAPI.Interfaces;
using UserAPI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<LinkFlowUsersDatabaseSettings>(builder.Configuration.GetSection("MongoDB"));

builder.Services.AddSingleton<IUserService, UserService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("SecurityPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:6060", "http://localhost:6061")
        .WithMethods("GET", "POST", "PUT", "DELETE")
        .AllowAnyHeader();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
} else
{
    app.UseHttpsRedirection();
}

app.UseCors("SecurityPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();
