namespace ReservationSystem.Domain.Models;

public class ReservationParticipant
{
	public Guid Id { get; }
	public Guid CompanyId { get; }
	public Guid ReservationId { get; }
	public Guid ParticipantId { get; }

	public ReservationParticipant(
		Guid id,
		Guid companyId,
		Guid reservationId,
		Guid participantId)
	{
		Id = id;
		CompanyId = companyId;
		ReservationId = reservationId;
		ParticipantId = participantId;
	}
}