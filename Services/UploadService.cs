using System.Security.Permissions;
using Azure;
using context;
using DTO;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Models;
using Microsoft.EntityFrameworkCore;

namespace Services
{
	public class UploadService
	{
		private readonly AppDbContext _context;

		public UploadService(AppDbContext context)
		{
			_context = context;
		}



		public async Task<ResponseDTO> PutFile(string folder, string? oldFileName, IFormFile filename)
		{
			if (string.IsNullOrEmpty(folder) || string.IsNullOrEmpty(oldFileName))
			{
				return new ResponseDTO {Code = 403 , Message="Invalid"};
			}

			if (filename == null)
			{
				return new ResponseDTO {Code = 404 , Message="No file uploaded."};
				
			}

			var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folder, oldFileName);

			if (!System.IO.File.Exists(filePath))
			{
				return new ResponseDTO {Code = 404 , Message="File path not found."+filePath};
				
			}

			try
			{
		

				var result = await UploadFile(filename, folder); 
				var user = await _context.ProfProfiles.Where(e=> e.Cv==oldFileName).FirstOrDefaultAsync();
				if (user!=null)
				{
					user.Cv = result.FileName;

					_context.Entry(user).State = EntityState.Modified;
					await _context.SaveChangesAsync();
				}
				if (result != null)
				{
					System.IO.File.Delete(filePath);

					return new ResponseDTO{Code = 200 ,  Message = result.Msg, File = result.FileName };
				}

					return new ResponseDTO{Code = 500 ,  Message = "File upload failed" };
				
			}
			catch (Exception ex)
			{
					return new ResponseDTO{Code = 500 ,  Message = "Error processing file: " + ex.Message };
				
			}
		}







		public async Task<FileUploadResponseDTO> UploadFile(IFormFile file, string folderName)
		{
			if (file == null || file.Length == 0)
			{ 
				return new FileUploadResponseDTO
				{
					Msg = "No file uploaded or file is empty.",
					FileName = null,
					Code = -1,
				};
			}

			var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folderName);

			if (!Directory.Exists(uploadsFolder))
			{
				_ = Directory.CreateDirectory(uploadsFolder);
			}

			var random = new Random();
			var randomNumber = random.Next(1000000, 9999999);

			var newFileName = $"{randomNumber}_{file.FileName}";

			var filePath = Path.Combine(uploadsFolder, newFileName);

			using (var stream = new FileStream(filePath, FileMode.Create))
			{
				await file.CopyToAsync(stream);
			}
			return new FileUploadResponseDTO
			{
				Msg = "success",
				FileName = newFileName,
				Code = 1,
			};
		}


		public async Task<FileDownloadDTO?> DownloadFileAsync(string folder, string filename)
		{
			var filepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folder, filename);

			if (!System.IO.File.Exists(filepath))
			{
				return null;
			}

			var provider = new FileExtensionContentTypeProvider();
			if (!provider.TryGetContentType(filepath, out var contentType))
			{
				contentType = "application/octet-stream";
			}

			var bytes = await System.IO.File.ReadAllBytesAsync(filepath);

			return new FileDownloadDTO
			{
				FileName = Path.GetFileName(filepath),
				ContentType = contentType,
				FileContent = bytes
			};
		}
	}
}
