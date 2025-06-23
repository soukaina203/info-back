using System.Text.Json.Serialization;
using Models;
namespace Models
{

	public class ProfProfile
	{
		public int Id { get; set; }

		public int UserId { get; set; }  
		public string City { get; set; }  
		public string Cv { get; set; } = string.Empty;
		public User? User { get; set; }

		public int[] Services { get; set; }
		public int[] Specialities { get; set; }
		public int[] Niveaux { get; set; }
		public int[] Methodes { get; set; }
	}
}
