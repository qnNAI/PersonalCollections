using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Identity
{
	public class SignInRequestValidator : AbstractValidator<SignInRequest>
	{
		public SignInRequestValidator()
		{
			RuleFor(x => x.Username).NotEmpty();
			RuleFor(x => x.Password).NotEmpty();
		}
	}
}
