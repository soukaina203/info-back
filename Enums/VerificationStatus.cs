namespace Enums
{
	public enum VerificationStatus
	{
		Pending = 0,
		EmailSended = 1,
		NotVerified=2,
		Verified = 4,
		Failed = 5 // failed to send the mail also for exceptions
	}
}
