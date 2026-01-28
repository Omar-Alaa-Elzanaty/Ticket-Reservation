using Core.Dtos;
using Core.Models;
using FluentValidation;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Core.Features.Auth.Register
{
    public class Registercommand : IRequest<BaseResponse<bool>>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    internal class RegistercommandHandler : IRequestHandler<Registercommand, BaseResponse<bool>>
    {
        private readonly UserManager<User> _userManager;
        private readonly IValidator<Registercommand> _validator;

        public RegistercommandHandler(
            UserManager<User> userManager,
            IValidator<Registercommand> validator)
        {
            _userManager = userManager;
            _validator = validator;
        }

        public async Task<BaseResponse<bool>> Handle(Registercommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                return BaseResponse<bool>.Failure(string.Join(",", validationResult.Errors), System.Net.HttpStatusCode.UnprocessableEntity);
            }

            if (await _userManager.Users.AnyAsync(u => u.UserName == request.UserName, cancellationToken))
            {
                return BaseResponse<bool>.Failure("Username is already taken", System.Net.HttpStatusCode.Conflict);
            }

            var user = request.Adapt<User>();

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                return BaseResponse<bool>.Failure(string.Join(",", result.Errors), System.Net.HttpStatusCode.UnprocessableEntity);
            }

            return BaseResponse<bool>.Success(true, "User registered successfully", System.Net.HttpStatusCode.Created);
        }
    }
}
