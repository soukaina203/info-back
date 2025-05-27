using DTO;
using Models;
namespace DTO
{
	public class PutUserResponseDTO
	{
		public int Code { get; set; }
		public string Message { get; set; }
		public User? UserData { get; set; }
		public ProfProfile? ProfData { get; set; }
	}
}