using Models;
using DTO;

namespace DTO
{
	// this is for teacher and students profile data , for the userDto [ contains only non sensitive data]
	
	public class ProfProfilDTO
	{
		public UserDto user { get; set; }
		public ProfProfile? profProfile { get; set; }
	}
}