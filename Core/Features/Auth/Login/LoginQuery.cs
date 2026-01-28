using Core.Dtos;
using Core.Models;
using Core.Ports;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Core.Features.Auth.Login
{
    public class LoginQuery : IRequest<BaseResponse<LoginQueryDto>>
    {
        public string UserName { get; init; }
        public string Password { get; init; }
    }

    internal class LoginQueryHandler : IRequestHandler<LoginQuery, BaseResponse<LoginQueryDto>>
    {
        private readonly IAuthServices _authServices;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public LoginQueryHandler(
            IAuthServices authServices,
            UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _authServices = authServices;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<BaseResponse<LoginQueryDto>> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            var signIn = await _signInManager.PasswordSignInAsync(request.UserName, request.Password, true, false);

            if (!signIn.Succeeded)
            {
                return BaseResponse<LoginQueryDto>.Failure("Invalid username or password", System.Net.HttpStatusCode.Unauthorized);
            }
            
            var user = await _userManager.FindByNameAsync(request.UserName);

            var token = await _authServices.GenerateTokenAsync(user);

            return BaseResponse<LoginQueryDto>.Success(new LoginQueryDto()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Token = token
            });
        }
    }
}
