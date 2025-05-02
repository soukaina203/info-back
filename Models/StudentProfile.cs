using Models;
namespace Models
{

public class StudentProfile
{
	public int Id { get; set; }

	public int UserId { get; set; }  // FK to User



	// Navigation to parent User
	public User User { get; set; } = null!;
}
}