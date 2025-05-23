using DTO;
namespace DTO
{

	public class loginResponse
	{
		public int Code { get; set; }
		public string Message { get; set; }
		public int UserId { get; set; }

		public string? Token { get; set; }
		public string? RefreshToken { get; set; }

		public UserDto UserData { get; set; }

	}

}