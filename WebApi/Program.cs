using FluentValidation;
using WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", @"Credentials/account_service.json");

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllerServices();
builder.Services.AddSwaggerServices();

builder.Services.AddBackgroundServices();
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();
builder.Services.AddConfigureSettings(builder.Configuration);
builder.Services.AddDbContextConfiguration(builder.Configuration);
builder.Services.AddServices();
builder.Services.AddCorsPolicy();
builder.Services.AddConfigureApiBehavior();
builder.Services.AddTechGadgetRateLimiter();

var app = builder.Build();

app.UseTechGadgetsExceptionHandler();
app.UseTechGadgetRateLimiter();
app.UseCors();
app.UseSwaggerServices();
app.UseAuthentication();
app.UseAuthorization();
app.ApplyMigrations();

app.MapControllers().RequireRateLimiting("concurrency");

app.Run();
