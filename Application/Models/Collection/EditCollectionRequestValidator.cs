using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.Models.Collection
{
    public class EditCollectionRequestValidator : AbstractValidator<EditCollectionRequest>
    {
        public EditCollectionRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.Fields).Custom((fields, context) => {
                var set = new HashSet<string>();
                foreach(var field in fields)
                {
                    if(!set.Add(field.Name))
                    {
                        context.AddFailure(new FluentValidation.Results.ValidationFailure
                        {
                            ErrorMessage = "Field name must be unique!",
                            PropertyName = "Fields"
                        });
                    }
                }
            });
        }
    }
}
