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
		public bool IsActive { get; set; } = true;
		public DateTime RegistrationDate { get; set; }

		public ICollection<Collection> Collections { get; set; } = new List<Collection>();
        public ICollection<Like> Likes { get; set; } = new List<Like>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
