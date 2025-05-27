using context;
using Microsoft.EntityFrameworkCore;
using Services;
using DotNetEnv;
using Utilities;

Env.Load(); 
var builder = WebApplication.CreateBuilder(args);

// Read from environment (already loaded by DotNetEnv)
var allowedOrigins = Environment.GetEnvironmentVariable("AllowedOrigins");

builder.Services.AddCors(options =>
{
	options.AddPolicy("CorsPolicy", policy =>
		{
			policy.WithOrigins(allowedOrigins).AllowAnyHeader().AllowAnyMethod().AllowCredentials();
	});
});



// Add .env variables to configuration
builder.Configuration.AddEnvironmentVariables();
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication();

builder.Services.AddScoped<PasswordHashing>();
builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<AccountService>();
builder.Services.AddScoped<UploadService>();
builder.Services.AddScoped<EmailService>();
builder.Services.AddScoped(typeof(SuperService<>));

builder.Services.AddScoped<UserService>();

var connectionString = Environment.GetEnvironmentVariable("DefaultConnection");



builder.Services.AddDbContext<AppDbContext>(options =>
options.UseNpgsql(connectionString));

builder.Services.AddControllers();

var app = builder.Build();
app.UseCors("CorsPolicy");


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	_ = app.UseSwagger();
	_ = app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthorization();
app.UseAuthentication();
app.MapControllers();


app.Run();
