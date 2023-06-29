using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.Models.Identity {

    public class ResetPasswordRequestValidator : AbstractValidator<ResetPasswordRequest> {

        public ResetPasswordRequestValidator()
        {
            RuleFor(x => x.Password).NotEmpty();
            RuleFor(x => x.PasswordConfirm).NotEmpty().Equal(x => x.Password).WithMessage("Passwords do not match");

            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Token).NotEmpty();
        }
    }
}
