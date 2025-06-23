using Models;
namespace Models
{

    public class StudentProfile
    {
        public int Id { get; set; }

        public int UserId { get; set; } 

        public User User { get; set; } = null!;
    }
}