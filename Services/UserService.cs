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
		public async Task<ResponseDTO> Put(int id, ProfInscriptionDTO user)
		{
			// Récupérer depuis la base les champs sensibles qu'on ne reçoit jamais du frontend
			var dbUser = await _context.Users
				.AsNoTracking()
				.Where(u => u.Id == id)
				.Select(u => new { u.Password, u.IsAdmin })
				.FirstOrDefaultAsync();

			if (dbUser == null)
			{
				return new ResponseDTO { Code = 404, Message = "Utilisateur introuvable." };
			}

			// Si mot de passe non vide => on le hash, sinon on garde l'ancien
			if (!string.IsNullOrWhiteSpace(user.user.Password))
			{
				user.user.Password = _passwordHasher.HashPassword(user.user.Password);
			}
			else
			{
				user.user.Password = dbUser.Password;
			}

			// Toujours restaurer isAdmin depuis la base (jamais depuis le frontend)
			user.user.IsAdmin = dbUser.IsAdmin;

			// Marquer l'utilisateur comme modifié
			_context.Entry(user.user).State = EntityState.Modified;

			// Si c'est un prof, marquer aussi le profil comme modifié
			if (user.user.RoleId == 1 && user.profProfile != null)
			{
				_context.Entry(user.profProfile).State = EntityState.Modified;
			}

			try
			{
				await _context.SaveChangesAsync();
				return new ResponseDTO { Code = 200, Message = "Success" };
			}
			catch (DbUpdateConcurrencyException ex)
			{
				return new ResponseDTO { Code = 500, Message = ex.Message };
			}
		}




	}


}
