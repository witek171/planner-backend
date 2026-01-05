using System.ComponentModel.DataAnnotations;

namespace Schedule.Contracts.Dtos.Responses;

public class StaffMemberCompaniesResponse
{
	[Required] public StaffMemberResponse StaffMember { get; }
	[Required] public IReadOnlyList<CompanyResponse> Companies { get; }

	public StaffMemberCompaniesResponse(
		StaffMemberResponse staffMember,
		IReadOnlyList<CompanyResponse> companies)
	{
		StaffMember = staffMember;
		Companies = companies;
	}
}