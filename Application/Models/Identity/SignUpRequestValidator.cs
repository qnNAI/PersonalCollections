﻿using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Identity
{
	public class SignUpRequestValidator : AbstractValidator<SignUpRequest>
	{
		public SignUpRequestValidator()
		{
			RuleFor(x => x.Username).NotEmpty();
			RuleFor(x => x.Email).NotEmpty().EmailAddress();
			RuleFor(x => x.Password).NotEmpty();
		}
	}
}
