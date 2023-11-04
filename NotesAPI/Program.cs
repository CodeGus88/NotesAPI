using NotesAPI.Repositories;
using NotesAPI.Repositories.Interfaces;
using NotesAPI.Services;
using NotesAPI.Services.Interfaces;
using NotesAPI.Utils;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("RedisConnection");
});

// My Services
builder.Services.AddScoped<DbContext>();
builder.Services.AddTransient<ICacheAdmin, CacheAdmin>();

builder.Services.AddTransient<IRedisRepository, RedisRepository>();
builder.Services.AddTransient<INoteRepository, NoteRepository>();
builder.Services.AddTransient<INoteService, NoteService>();

builder.Services.AddAutoMapper(typeof(Program));

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

app.Run();
