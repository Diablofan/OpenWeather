using OpenWeather.Models;
using OpenWeather.Pages;
using OpenWeather.Services;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
var owSettings = builder.Configuration.GetSection("OpenWeather");
builder.Services.Configure<OpenWeatherSettings>(owSettings);

builder.Services.AddHttpClient<WeatherService>(client =>
{
    client.BaseAddress = new Uri(owSettings.GetValue<string>("Endpoint"));
});

builder.Services.AddSingleton<IWeatherService, WeatherService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
