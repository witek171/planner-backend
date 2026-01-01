using Schedule.Infrastructure.Services;
using Schedule.Application.Interfaces.Utils;

namespace Schedule.Infrastructure.Utils;

public class PasswordHasher : IPasswordHasher
{
	private readonly string _pepper;

	public PasswordHasher()
	{
		_pepper = EnvironmentService.PasswordPepper;
	}

	public string Hash(string password)
	{
		string passwordWithPepper = password + _pepper;
		return BCrypt.Net.BCrypt.HashPassword(passwordWithPepper);
	}

	public bool Verify(string password, string hash)
	{
		string passwordWithPepper = password + _pepper;
		return BCrypt.Net.BCrypt.Verify(passwordWithPepper, hash);
	}
}