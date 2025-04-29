using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
	public class User
	{
public int Id { get; set; }
public string FirstName { get; set; }
public string LastName { get; set; }
public string Telephone { get; set; }
public string Email { get; set; }
public string Password { get; set; }
public bool IsAdmin { get; set; }


public int RoleId { get; set; }
public virtual Role? Role { get; set; }

public string? Photo { get; set; }
public string? City { get; set; }
public string Cv { get; set; }


public ICollection<Service> Services { get; set; }
public ICollection<Speciality> Specialities { get; set; }
public ICollection<Niveau> niveaux { get; set; }
public ICollection<Method> methodes { get; set; }

    }
}