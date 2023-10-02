using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Cache.CacheManager;

var builder = WebApplication.CreateBuilder(args);

//Configure Ocelot Json files here
builder.Configuration
    .AddJsonFile(
        $"ocelot.{builder.Environment.EnvironmentName}.json", 
        optional: true, 
        reloadOnChange: true);
builder.Services
    .AddOcelot(builder.Configuration)
    .AddCacheManager(x => x.WithDictionaryHandle());

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

await app.UseOcelot();

app.Run();
