using DatWise;
using Infrastructure.Loggers;
using Application.Config;

var builder = WebApplication.CreateBuilder(args);

var appSettingsConfiguration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true)
    .Build();

try
{
    // Adding AppSetting from json
    builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("Settings"));

    builder.Services.AddControllers();

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    // Register chain of responsibility loggers
    builder.Services.RegisterAppLoggers();

    // Register App services
    builder.Services.AddAppServices();

    builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(
            builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            });
    });


    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseRouting();

    //app.UseHttpsRedirection();

    app.UseCors();
    app.UseAuthorization();

    //app.UseResponseCaching();
    app.MapControllers();

    app.Run();

}
catch (Exception ex)
{
    return 1;
}
finally
{
    //Log.CloseAndFlush();
}

return 0;