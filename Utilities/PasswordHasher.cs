using Microsoft.AspNetCore.Identity;
namespace Utilities
{

	public class PasswordHashing
	{
		private readonly PasswordHasher<object> _hasher = new();

		public string HashPassword(string password)
		{
			return _hasher.HashPassword(null, password);
		}

		public bool VerifyPassword(string password, string hashedPassword)
		{
			var result = _hasher.VerifyHashedPassword(null, hashedPassword, password);
			return result == PasswordVerificationResult.Success;
		}
				public bool CheckPassword(string enteredPassword, string storedHashedPassword)
		{
			return VerifyPassword(enteredPassword, storedHashedPassword);
		}
	}
}

