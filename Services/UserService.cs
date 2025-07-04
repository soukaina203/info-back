using DTO;
using Models;
using Microsoft.EntityFrameworkCore;
using Utilities;
using context;
using DTO;
namespace Services
{
	public class UserService : SuperService<User>
	{
		private readonly AppDbContext _context;
		private readonly PasswordHashing _passwordHasher;

		public UserService(AppDbContext context, PasswordHashing passwordHasher) : base(context)
		{
			_context = context;
			_passwordHasher = passwordHasher;
		}
		
		
		
		public override   async Task<object>  GetAll()
		{
			var data = await _context.Users.Include(u=>u.Role).ToListAsync();
			
			return new {Data = data};
		}

		public  async Task<ProfProfilDTO> GetUserById(int id)
		{
			var user = await _context.Users
				.Include(u => u.Role)
				.FirstOrDefaultAsync(u => u.Id == id);
				
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
				RoleId = user.RoleId,
				Role = user.Role
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


		public  async Task<PutUserResponseDTO> PutUser(int id, ProfInscriptionDTO dto)
		{
			var user = await _context.Users
			.Where(u => u.Id == id)
			.FirstOrDefaultAsync();
			if (user == null)
				return new PutUserResponseDTO { Code = 404, Message = "Utilisateur introuvable." };
				
				
			if (user.Email != dto.user.Email)
			{
			var emailExists = await _context.Users
			.Where(u => u.Email == dto.user.Email)
			.FirstOrDefaultAsync();
		
				if (emailExists != null)
				{
					return new PutUserResponseDTO { Code = -1, Message = "Email already exists" };
				}
			}
			
			if ( dto.user.Password!=null  &&!_passwordHasher.CheckPassword(dto.user.Password, user.Password))
			{
				user.Password=_passwordHasher.HashPassword(dto.user.Password);
			}



			user.Email = dto.user.Email;
			user.FirstName = dto.user.FirstName;
			user.LastName = dto.user.LastName;
			user.Telephone = dto.user.Telephone;
			user.Photo = dto.user.Photo;

			if (dto.profProfile != null && dto.user.RoleId==1)
			{
				var prof = await _context.ProfProfiles
				.Where(p => p.UserId == id)
				.FirstOrDefaultAsync();
				if (prof != null)
				{
					prof.City = dto.profProfile.City;
					prof.Services = dto.profProfile.Services;
					prof.Specialities = dto.profProfile.Specialities;
					prof.Niveaux = dto.profProfile.Niveaux;
					prof.Methodes = dto.profProfile.Methodes;

				}

			}

			var result = await _context.SaveChangesAsync();
			return new PutUserResponseDTO

			{

				Code = 200,

				Message = "Success",

				UserData = dto.user,

				ProfData = dto.profProfile

			};
		}


		public async Task<object> SearchUsers(string nom, string prenom, string email, int roleId)
		{
			var query = _context.Users
				.Include(u => u.Role)
				.AsQueryable();

			if (nom != "null" && !string.IsNullOrEmpty(nom))
			{
				query = query.Where(u => u.LastName.ToLower().Contains(nom.Trim().ToLower()));
			}

			if (prenom != "null" && !string.IsNullOrEmpty(prenom))
			{
				query = query.Where(u => u.FirstName.ToLower().Contains(prenom.Trim().ToLower()));
			}

			if (email != "null" && !string.IsNullOrEmpty(email))
			{
				query = query.Where(u => u.Email.ToLower().Contains(email.Trim().ToLower()));
			}

			if (roleId > 0)
			{
				query = query.Where(u => u.RoleId == roleId);
			}

			var result = await query
				.Select(u => new UserSearchDTO
				{
					Id = u.Id,
					LastName = u.LastName,
					FirstName = u.FirstName,
					Email = u.Email,
					RoleId = u.RoleId,
					RoleName = u.Role != null ? u.Role.Name : string.Empty
				})
				.ToListAsync();

			return new
			{
				query = new
				{
					result
				},
				filters = new
				{
					nom,
					prenom,
					email,
					roleId
				}
			};
		}

	}


}
