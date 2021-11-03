using FluentValidation;
using Manager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager.Domain.Validators
{
    class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(x => x)
                .NotEmpty()
                .WithMessage("Entity should not be empty.")

                .NotNull()
                .WithMessage("Entity should not be null.");

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name should not be empty.")

                .NotNull()
                .WithMessage("Name should not be null.")

                .MinimumLength(3)
                .WithMessage("Name should have at least 3 characters.")

                .MaximumLength(80)
                .WithMessage("Name must have a maximum of 80 characters.");

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email should not be empty.")

                .NotNull()
                .WithMessage("Email should not be null.")

                .MinimumLength(8)
                .WithMessage("Email should have at least 8 characters.")

                .MaximumLength(180)
                .WithMessage("Email must have a maximum of 180 characters.")

                .Matches(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$")
                .WithMessage("Email is not valid.");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Password should not be empty.")

                .NotNull()
                .WithMessage("Password should not be null.")

                .MinimumLength(6)
                .WithMessage("Password should have at least 6 characters.")

                .MaximumLength(30)
                .WithMessage("Password must have a maximum of 30 characters.");
        }
    }
}
