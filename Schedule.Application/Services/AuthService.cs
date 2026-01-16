using Schedule.Application.Interfaces.Services;
using Schedule.Application.Interfaces.Utils;
using Schedule.Domain.Exceptions;
using Schedule.Domain.Models;

namespace Schedule.Application.Services;

public class AuthService : IAuthService
{
	protected IJwtTokenService _jwtTokenService;
	protected IStaffMemberService _staffMemberService;
	protected IPasswordHasher _passwordHasher;

	public AuthService(
		IJwtTokenService jwtTokenService,
		IStaffMemberService staffMemberService,
		IPasswordHasher passwordHasher)
	{
		_jwtTokenService = jwtTokenService;
		_staffMemberService = staffMemberService;
		_passwordHasher = passwordHasher;
	}

	// obecnie obsuluje tylko logowanie staff members bez participants
	public async Task<string> LoginAsync(string email, string password)
	{
		StaffMember? staffMember = await _staffMemberService.GetByEmailAsync(email);
		if (staffMember == null)
			throw new InvalidCredentialsException();

		Boolean isPasswordValid = _passwordHasher.Verify(password, staffMember.Password);
		if (!isPasswordValid)
			throw new InvalidCredentialsException();

		String token = _jwtTokenService.GenerateToken(staffMember.Id, staffMember.Role);
		return token;
	}

	// to bedzie rejestracja klientow (participants) a nie staffMembers
	public async Task<Guid> RegisterAsync(StaffMember staffMember)
	{
		throw new NotImplementedException();
		// 	staffMember.Normalize();
		// 	string hashed = _passwordHasher.Hash(staffMember.Password);
		// 	staffMember.SetPassword(hashed);
		// 	return await _staffMemberService.CreateAsync(staffMember);
	}
}