namespace DTO
{
	
public class RegistrationResponse<T>
{
	public int Code { get; set; }
	public string Message { get; set; }
	public T? Data { get; set; }

		public int UserId { get; set; }
	
}

}