using Microsoft.EntityFrameworkCore;
using MortalKombatXII.Core.Models.Configs;
using MortalKombatXII.Core.Repositories;
using MortalKombatXII.Core.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(opts =>
{
    var enumConverter = new JsonStringEnumConverter();
    opts.JsonSerializerOptions.Converters.Add(enumConverter);
});

var dbConfig = builder.Configuration.GetSection(nameof(DbConfig)).Get<DbConfig>();
builder.Services.AddDbContext<ApplicationContext>
    (
        options => options.UseMySql
        (
            dbConfig.ConnectionString, 
            new MySqlServerVersion(new Version(10, 1, 40))
        ), ServiceLifetime.Transient);
    

builder.Services.AddTransient<PlayersRepository>();
builder.Services.AddTransient<RoomsRepository>();
builder.Services.AddTransient<WarriorsRepository>();

builder.Services.AddTransient<BattleService>();

builder.Services.AddHostedService<BattleHostedService>();

builder.Services.AddAutoMapper(typeof(Program).Assembly);
var app = builder.Build();

app.UseAuthorization();

app.MapControllers();

app.Run();
