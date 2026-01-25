namespace ReservationSystem.Infrastructure.Services;

public static class EnvironmentService
{
	public static string SqlConnectionString => GetEnvironmentVariable("ConnectionStrings__DefaultConnection");
	public static string EnvName => GetEnvironmentVariable("OtherEnvVariable");
	public static string PasswordPepper => GetEnvironmentVariable("PasswordPepper");
	public static string JwtIssuer => GetEnvironmentVariable("Jwt__Issuer");
	public static string JwtAudience => GetEnvironmentVariable("Jwt__Audience");
	public static string JwtExpiresInMinutes => GetEnvironmentVariable("Jwt__ExpiresInMinutes");

	public static string GetEnvironmentVariable(string variable)
	{
		string? value = Environment.GetEnvironmentVariable(variable);

		if (string.IsNullOrWhiteSpace(value))
		{
			string errorMessage = $"Environment variable '{variable}' not found or is empty.";
			throw new InvalidOperationException(errorMessage);
		}

		return value;
	}
}