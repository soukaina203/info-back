using DTO;
using Models;
using Microsoft.EntityFrameworkCore;
using Utilities;
using context;
using DTO;
namespace Services
{
	public class UserService : SuperService<ProfProfilDTO>
	{
		private readonly AppDbContext _context;
		private readonly PasswordHashing _passwordHasher;

		public UserService(AppDbContext context, PasswordHashing passwordHasher) : base(context)
		{
			_context = context;
			_passwordHasher = passwordHasher;
		}

		// For teachers and students
		public override async Task<ProfProfilDTO> GetById(int id)
		{
			var user = await _context.Users.FindAsync(id);
			if (user == null)
			{
				throw new Exception($"User with ID {id} not found.");
			}
			var userDto = new UserDto
			{
				Id = user.Id,
				FirstName = user.FirstName,
				LastName = user.LastName,
				Telephone = user.Telephone,
				Email = user.Email,
				Photo = user.Photo,
				RoleId = user.RoleId
			};
			if (user.RoleId == 1)
			{
				var prof = await _context.ProfProfiles
					.FirstOrDefaultAsync(p => p.UserId == id);

				if (prof == null)
				{
					throw new Exception($"Prof profile for user ID {id} not found.");
				}

				return new ProfProfilDTO { user = userDto, profProfile = prof };
			}
			else
			{
				return new ProfProfilDTO { user = userDto, profProfile = null };
			}
		}

		// reason behind using ProfInscriptionDTO is that the user here contains the pwd too
		// while in the ProfProfilDTO the user doesn't contain the password because we should not send back the password even if its hashed to the frontend



		public async Task<PutUserResponseDTO> Put(int id, ProfInscriptionDTO user)
		{
			var dbUser = await _context.Users
				.AsNoTracking()
				.Where(u => u.Id == id)
				.Select(u => new { u.Password, u.IsAdmin })
				.FirstOrDefaultAsync();

			if (dbUser == null)
			{
				return new PutUserResponseDTO { Code = 404, Message = "Utilisateur introuvable." };
			}

			if (!string.IsNullOrWhiteSpace(user.user.Password))
			{
				user.user.Password = _passwordHasher.HashPassword(user.user.Password);
			}
			else
			{
				user.user.Password = dbUser.Password;
			}

			user.user.IsAdmin = dbUser.IsAdmin;

			_context.Entry(user.user).State = EntityState.Modified;

			if (user.user.RoleId == 1 && user.profProfile != null)
			{
				_context.Entry(user.profProfile).State = EntityState.Modified;
			}

			try
			{
				await _context.SaveChangesAsync();

	
				return new PutUserResponseDTO
				{
					Code = 200,
					Message = "Success",
					UserData = user.user ,
					ProfData = user.profProfile != null ? user.profProfile:null
					
				};
			}
			catch (DbUpdateConcurrencyException ex)
			{
				return new PutUserResponseDTO { Code = 500, Message = ex.Message };
			}
		}



	}


}
