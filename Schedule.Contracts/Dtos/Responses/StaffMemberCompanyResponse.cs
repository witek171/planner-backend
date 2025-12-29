namespace Schedule.Contracts.Dtos.Responses;

public class StaffMemberCompanyResponse
{
	public Guid Id { get; set; }
	public Guid StaffMemberId { get; set; }
	public Guid CompanyId { get; set; }
	public DateTime CreatedAt { get; set; }
}