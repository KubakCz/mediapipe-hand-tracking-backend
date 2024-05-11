using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using MediaPipeHandTrackingBackend.NatNet;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Allow any origin for CORS
builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAllOrigins",
            builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            });
    });

// Add NatNetService to the container
var natNetService = new NatNetService();
builder.Services.AddSingleton<NatNetService>(natNetService);

// Setup JSON serialization
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Allow any origin for CORS
app.UseCors("AllowAllOrigins");
app.UseRouting();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Try to open browser with frontend
string url = "https://kubakcz.github.io/mediapipe-hand-tracking/";
try
{
    Process.Start(url);
}
catch
{
    // hack because of this: https://github.com/dotnet/corefx/issues/10361
    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
    {
        url = url.Replace("&", "^&");
        Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
    }
    else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
    {
        Process.Start("xdg-open", url);
    }
    else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
    {
        Process.Start("open", url);
    }
    else
    {
        throw;
    }
}

app.Run();
