using Models;

namespace DTO
{
    public class ProfInscriptionDTO
    {
        public User user { get; set; }
        public ProfProfile? profProfile { get; set; }
    }
}