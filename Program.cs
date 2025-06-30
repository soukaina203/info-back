using context;
using Microsoft.EntityFrameworkCore;
using Services;
using DotNetEnv;
using Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

Env.Load(); 
var builder = WebApplication.CreateBuilder(args);

var allowedOrigins = Environment.GetEnvironmentVariable("AllowedOrigins")?? "http://localhost:4200";

builder.Services.AddCors(options =>
{
	options.AddPolicy("CorsPolicy", policy =>
		{
			policy.WithOrigins(allowedOrigins).AllowAnyHeader().AllowAnyMethod().AllowCredentials();
	});
});



builder.Configuration.AddEnvironmentVariables();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(options =>
	{
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuer = true,
			ValidateAudience = true,
			ValidateLifetime = true,
			ValidateIssuerSigningKey = true,
			ValidIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER"),
			ValidAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE"),
			IssuerSigningKey = new SymmetricSecurityKey(
				Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_SECRET"))
			)
		};
	});

builder.Services.AddScoped(typeof(SuperService<>));
builder.Services.AddScoped<PasswordHashing>();
builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<AccountService>();
builder.Services.AddScoped<UploadService>();
builder.Services.AddScoped<EmailService>();
builder.Services.AddScoped<ClassService>();
builder.Services.AddScoped<RoleService>();

builder.Services.AddScoped<UserService>();

var connectionString = Environment.GetEnvironmentVariable("DefaultConnection");



builder.Services.AddDbContext<AppDbContext>(options =>
options.UseNpgsql(connectionString));

builder.Services.AddControllers();

var app = builder.Build();
app.UseCors("CorsPolicy");


// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
	_ = app.UseSwagger();
	_ = app.UseSwaggerUI();
// }
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();


app.Run();
