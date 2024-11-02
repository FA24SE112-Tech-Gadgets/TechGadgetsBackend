using FluentValidation;
using WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", @"Credentials/account_service.json");

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllerServices();
builder.Services.AddSwaggerServices();

builder.Services.AddBackgroundServices();
//builder.Services.AddSignalRService();
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();
builder.Services.AddConfigureSettings(builder.Configuration);
builder.Services.AddDbContextConfiguration(builder.Configuration);
builder.Services.AddServices();
builder.Services.AddCorsPolicy();
builder.Services.AddConfigureApiBehavior();
//builder.Services.AddSingletonForSignalR();
//builder.Services.AddAuthenticationForSignalR(builder.Configuration);
//builder.Services.AddAuthorizationForSignalR();

var app = builder.Build();

app.UseCors();
app.UseSwaggerServices();
app.UseTechGadgetsExceptionHandler();
app.UseAuthentication();
app.UseAuthorization();
app.ApplyMigrations();

app.MapControllers();
//app.UseNotificationHubHandler();

app.Run();
