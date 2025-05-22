using Models;
using DTO;

namespace DTO
{
	// this is for teacher registration for the user [ contans all the attributes ]
	public class ProfInscriptionDTO
	{
		public User user { get; set; }
		public ProfProfile? profProfile { get; set; }
	}
}