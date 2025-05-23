namespace DTO
{
	public class UserDto
	{

		public int Id { get; set; }
		public required string FirstName { get; set; }
		public required string LastName { get; set; }
		public required string? Telephone { get; set; }
		public required string Email { get; set; }
		public string? Photo { get; set; }
		public int? RoleId { get; set; }
	
	

	}
}