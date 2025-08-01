using OrdersDashboard.Web.Components;
using OrdersDashboard.Web.Models;
using OrdersDashboard.Web.Services;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Configure Azure Search options
builder.Services.Configure<AzureSearchOptions>(
    builder.Configuration.GetSection(AzureSearchOptions.SectionName));

// Register services
builder.Services.AddSingleton<IOrderService, OrderService>();
builder.Services.AddSingleton<IAzureSearchService, AzureSearchService>();

// Configure OpenTelemetry resource
var resourceBuilder = ResourceBuilder.CreateDefault()
    .AddService(
        serviceName: builder.Configuration["OpenTelemetry:ServiceName"] ?? "OrdersDashboard",
        serviceVersion: builder.Configuration["OpenTelemetry:ServiceVersion"] ?? "1.0.0",
        serviceInstanceId: Environment.MachineName);

// Configure OpenTelemetry Logging
builder.Logging.AddOpenTelemetry(options =>
{
    options.SetResourceBuilder(resourceBuilder);
    options.AddConsoleExporter();
});

// Configure OpenTelemetry Tracing and Metrics
builder.Services.AddOpenTelemetry()
    .WithTracing(tracing => tracing
        .SetResourceBuilder(resourceBuilder)
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation()
        .AddConsoleExporter())
    .WithMetrics(metrics => metrics
        .SetResourceBuilder(resourceBuilder)
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation()
        .AddConsoleExporter());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
