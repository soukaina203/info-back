using DTO;
using Models;
namespace DTO
{
	public class ResponseDTO
	{
		public int Code { get; set; }
		public string Message { get; set; }
		public string? File { get; set; }
		public object? Data { get; set; }	
		
		
	}
}