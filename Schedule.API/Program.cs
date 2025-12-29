using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Schedule.API.Mappings;
using Schedule.Application.Interfaces.Repositories;
using Schedule.Application.Interfaces.Services;
using Schedule.Application.Interfaces.Utils;
using Schedule.Application.Interfaces.Validators;
using Schedule.Application.Services;
using Schedule.Infrastructure.Extensions;
using Schedule.Infrastructure.Repositories;
using Schedule.Infrastructure.Services;
using Schedule.Infrastructure.Utils;
using Schedule.Infrastructure.Validators;
using System.Security.Cryptography;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// ============ SERVICES ============

builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddEndpointsApiExplorer();

// Swagger
builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new OpenApiInfo { Title = "Planner API", Version = "v1" });
	c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
	{
		Description = "Enter the token in the format: {token}",
		Name = "Authorization",
		In = ParameterLocation.Header,
		Type = SecuritySchemeType.Http,
		Scheme = "bearer",
		BearerFormat = "JWT"
	});

	c.AddSecurityRequirement(new OpenApiSecurityRequirement
	{
		{
			new OpenApiSecurityScheme
			{
				Reference = new OpenApiReference
				{
					Type = ReferenceType.SecurityScheme,
					Id = "Bearer"
				}
			},
			Array.Empty<string>()
		}
	});
});

// Authentication
RSA rsa = RSA.Create();
string publicKeyContent = LoadPublicKey();
rsa.ImportFromPem(publicKeyContent);

builder.Services.AddAuthentication("Bearer")
	.AddJwtBearer(options =>
	{
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuer = true,
			ValidIssuer = EnvironmentService.JwtIssuer,
			ValidateAudience = true,
			ValidAudience = EnvironmentService.JwtAudience,
			ValidateLifetime = true,
			ValidateIssuerSigningKey = true,
			IssuerSigningKey = new RsaSecurityKey(rsa)
		};
	});

builder.Services.AddAuthorization();

// Repositories
string connectionString = EnvironmentService.SqlConnectionString;
builder.Services.AddScoped<IParticipantRepository>(_ =>
	new ParticipantRepository(connectionString));
builder.Services.AddScoped<IStaffMemberRepository>(_ =>
	new StaffMemberRepository(connectionString));
builder.Services.AddScoped<IStaffMemberSpecializationRepository>(_ =>
	new StaffMemberSpecializationRepository(connectionString));
builder.Services.AddScoped<IStaffMemberAvailabilityRepository>(_ =>
	new StaffMemberAvailabilityRepository(connectionString));
builder.Services.AddScoped<IEventScheduleRepository>(_ =>
	new EventScheduleRepository(connectionString));
builder.Services.AddScoped<IEventScheduleStaffMemberRepository>(_ =>
	new EventScheduleStaffMemberRepository(connectionString));
builder.Services.AddScoped<ISpecializationRepository>(_ =>
	new SpecializationRepository(connectionString));
builder.Services.AddScoped<ICompanyRepository>(_ =>
	new CompanyRepository(connectionString));
builder.Services.AddScoped<IEventTypeRepository>(_ =>
	new EventTypeRepository(connectionString));
builder.Services.AddScoped<IReservationRepository>(_ =>
	new ReservationRepository(connectionString));
builder.Services.AddScoped<IReservationParticipantRepository>(_ =>
	new ReservationParticipantRepository(connectionString));
builder.Services.AddScoped<ICompanyConfigRepository>(_ =>
	new CompanyConfigRepository(connectionString));

// Services
builder.Services.AddScoped<IHealthCheckService>(provider =>
{
	IHealthCheckUtils healthCheckUtils = provider.GetRequiredService<IHealthCheckUtils>();
	ILogger<HealthCheckService> logger = provider.GetRequiredService<ILogger<HealthCheckService>>();
	return new HealthCheckService(healthCheckUtils, logger, connectionString);
});
builder.Services.AddScoped<IParticipantService, ParticipantService>();
builder.Services.AddScoped<IStaffMemberService, StaffMemberService>();
builder.Services.AddScoped<IStaffMemberSpecializationService, StaffMemberSpecializationService>();
builder.Services.AddScoped<IStaffMemberAvailabilityService, StaffMemberAvailabilityService>();
builder.Services.AddScoped<IEventScheduleStaffMemberService, EventScheduleStaffMemberService>();
builder.Services.AddScoped<IEventScheduleService, EventScheduleService>();
builder.Services.AddScoped<ISpecializationService, SpecializationService>();
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<IEventTypeService, EventTypeService>();
builder.Services.AddScoped<IReservationService, ReservationService>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<ICompanyConfigService, CompanyConfigService>();

// Utils & Validators
builder.Services.AddScoped<IHealthCheckUtils, HealthCheckUtils>();
builder.Services.AddScoped<IScheduleConflictValidator, ScheduleConflictValidator>();
builder.Services.AddScoped<IAvailabilityCalculator, AvailabilityCalculator>();

// CORS
builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowFrontend", policy =>
	{
		policy.WithOrigins("http://localhost:5173", "https://localhost:5174")
			.AllowAnyMethod()
			.AllowAnyHeader()
			.AllowCredentials();
	});
});

// ============ APP ============

WebApplication app = builder.Build();

// ============ MIDDLEWARE PIPELINE ============

app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI(c =>
	{
		c.SwaggerEndpoint("/swagger/v1/swagger.json", "Planner API V1");
		c.RoutePrefix = "swagger";
	});
}

app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", () => Results.Redirect("/swagger"))
	.ExcludeFromDescription();

app.MapControllers();

app.Run();

// ============ LOCAL FUNCTIONS ============

static string LoadPublicKey()
{
	string[] possiblePaths =
	{
		"/app/data/public.key",
		"./Data/public.key",
		"./data/public.key"
	};

	foreach (string path in possiblePaths)
	{
		if (File.Exists(path))
		{
			Console.WriteLine($"Key used from: {path}");
			return File.ReadAllText(path);
		}
	}

	throw new FileNotFoundException(
		"Public key not found! Please check whether the keys have been generated.");
}