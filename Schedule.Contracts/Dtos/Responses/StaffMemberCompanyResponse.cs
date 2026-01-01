namespace Schedule.Contracts.Dtos.Responses;

public class StaffMemberCompanyResponse
{
	public Guid Id { get; init; }
	public Guid StaffMemberId { get; init; }
	public Guid CompanyId { get; init; }
	public DateTime CreatedAt { get; init; }
}