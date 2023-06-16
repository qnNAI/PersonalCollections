using Domain.Entities.Items;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Identity
{
	public class ApplicationUser : IdentityUser
	{

		public ICollection<Collection>? Collections { get; set; }
	}
}
