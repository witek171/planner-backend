using Microsoft.IdentityModel.Tokens;
using Schedule.Application.Interfaces.Services;
using Schedule.Domain.Models.Enums;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Schedule.Infrastructure.Services;

public class JwtTokenService : IJwtTokenService
{
	public JwtTokenService()
	{
	}

	public string GenerateToken(Guid userId, StaffRole role)
	{
		List<Claim> claims =
		[
			new(ClaimTypes.NameIdentifier, userId.ToString()),
			new(ClaimTypes.Role, role.ToString())
		];

		RSA rsa = RSA.Create();
		string[] possiblePaths =
		{
			"/app/data/private.key",
			"./Data/private.key",
			"./data/private.key"
		};

		string? privateKeyContent = (
			from path in possiblePaths
			where File.Exists(path)
			select File.ReadAllText(path)).FirstOrDefault();

		if (privateKeyContent == null)
			throw new FileNotFoundException(
				"Private key not found! Check whether the keys have been generated.");

		rsa.ImportFromPem(privateKeyContent);
		SigningCredentials creds = new(new RsaSecurityKey(rsa), SecurityAlgorithms.RsaSha256);

		JwtSecurityToken token = new(
			issuer: EnvironmentService.JwtIssuer,
			audience: EnvironmentService.JwtAudience,
			claims: claims,
			expires: DateTime.UtcNow.AddMinutes(int.Parse(EnvironmentService.JwtExpiresInMinutes)),
			signingCredentials: creds);

		return new JwtSecurityTokenHandler().WriteToken(token);
	}
}