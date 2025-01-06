using PostsAPI.Interface;
using PostsAPI.Models;
using PostsAPI.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<LinkFlowPostsDatabaseSettings>(builder.Configuration.GetSection("MongoDB"));

builder.Services.AddScoped<IPostCommands, PostsService>();

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
}

app.UseCors("SecurityPolicy");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
