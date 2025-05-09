using context;
using DTO;
using Models;

namespace Services
{
    public class UploadService
    {
        private readonly AppDbContext _context;

        public UploadService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<FileUploadResponseDTO> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            { // code -1 failure
                return new FileUploadResponseDTO
                {
                    Msg = "No file uploaded or file is empty.",
                    FileName = null,
                    Code = -1,
                };
            }

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "cvs");

            if (!Directory.Exists(uploadsFolder))
            {
                _ = Directory.CreateDirectory(uploadsFolder);
            }

            // Generate a random number of length 7
            var random = new Random();
            var randomNumber = random.Next(1000000, 9999999);

            // Create a new filename with the random number
            var newFileName = $"{randomNumber}_{file.FileName}";

            var filePath = Path.Combine(uploadsFolder, newFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            // code 1 = success
            return new FileUploadResponseDTO
            {
                Msg = "success",
                FileName = newFileName,
                Code = 1,
            };
        }
    }
}
