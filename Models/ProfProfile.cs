using Models;
namespace Models
{

	public class ProfProfile
	{
		public int Id { get; set; }

		public int UserId { get; set; }  // FK to User
		public string City { get; set; }  // FK to User
		public string Cv { get; set; } = string.Empty;
		

		public User? User { get; set; }

		// Related collections
		public int[] Services { get; set; }
		public int[] Specialities { get; set; }
		public int[] Niveaux { get; set; }
		public int[] Methodes { get; set; }
	}
}
