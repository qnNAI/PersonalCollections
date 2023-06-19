using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.Models.Identity {

    public class ExternalLoginModelValidator : AbstractValidator<ExternalLoginModel> {

        public ExternalLoginModelValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
        }
    }
}
