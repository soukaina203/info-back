namespace DTO
{

	public class FileDownloadDTO
	{
		public string FileName { get; set; }
		public string ContentType { get; set; }
		public byte[] FileContent { get; set; }
	}
}
