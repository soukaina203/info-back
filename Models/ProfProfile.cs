using Models;
namespace Models
{
	
public class ProfProfile
{
	public int Id { get; set; }

	public int UserId { get; set; }  // FK to User
	public string Cv { get; set; } = string.Empty;

	// Navigation to parent User
	public User User { get; set; } = null!;

	// Related collections
	public ICollection<Service> Services { get; set; } = new List<Service>();
	public ICollection<Speciality> Specialities { get; set; } = new List<Speciality>();
	public ICollection<Niveau> Niveaux { get; set; } = new List<Niveau>();
	public ICollection<Method> Methodes { get; set; } = new List<Method>();
}
}
