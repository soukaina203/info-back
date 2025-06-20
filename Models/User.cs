using Enums;

namespace Models
{
	public class User
	{
		public int Id { get; set; }
		public required string FirstName { get; set; }
		public required string LastName { get; set; }
		public required string? Telephone { get; set; }
		public required string Email { get; set; }
		public required string? Password { get; set; }
		public string? Photo { get; set; }
		public int RoleId { get; set; }
		
		public virtual Role? Role { get; set; }
		public string? RefreshToken { get; set; }
		public DateTime? RefreshTokenExpiryTime { get; set; }
		public VerificationStatus Status { get; set; }


	}
}