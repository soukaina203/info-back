using Models;
namespace DTO
{

	public class RegistrationResponse<T>
	{
		public int Code { get; set; }
		public string Message { get; set; }
		public string? Token { get; set; }

		public int UserId { get; set; }
		public bool? IsEmailSended { get; set; }
		public User? User { get; set; }
		public string? errors { get; set; }


	}

}