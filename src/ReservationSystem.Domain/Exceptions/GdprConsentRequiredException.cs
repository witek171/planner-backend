namespace ReservationSystem.Domain.Exceptions;

public class GdprConsentRequiredException : Exception
{
	public GdprConsentRequiredException()
		: base("GDPR consent is required to process participant data")
	{
	}
}