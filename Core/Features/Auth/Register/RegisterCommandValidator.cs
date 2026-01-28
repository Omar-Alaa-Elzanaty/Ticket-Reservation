using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Features.Auth.Register
{
    public class RegisterCommandValidator:AbstractValidator<Registercommand>
    {
        public RegisterCommandValidator()
        {

        }
    }
}
