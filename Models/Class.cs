using Models;

namespace Models
{
	public class Class
	{
		public int Id { get; set; }
		public string Title { get; set; }

		public DateTime Date { get; set; }

		public string Link { get; set; }
		public string Description { get; set; }
		public string Duree { get; set; }
		public string Picture { get; set; }

		public int UserId { get; set; }
		
		public virtual User? User { get; set; }


	}
}