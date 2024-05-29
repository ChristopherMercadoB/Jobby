using Jobby.Core.Application.DTOS.Account;
using Jobby.Core.Application.Features.User.Command.AuthenticateUser;
using Jobby.Core.Application.Features.User.Command.RegisterUser;
using Microsoft.AspNetCore.Mvc;

namespace Jobby.Presentation.WebApi.Controllers
{
    [ApiController]
    public class AccountController : BaseApiController
    {
        [HttpPost("Authenticate")]
        public async Task<IActionResult> AuthenticateAsync(AuthenticationRequest request)
        {
            return Ok(await Mediator.Send(new AuthenticateUserCommand()
            {
                Email = request.Email,
                Password = request.Password
            }));
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync(RegisterRequest request)
        {
            return Ok(await Mediator.Send(new RegisterUserCommand()
            {
                Email = request.Email,
                Password = request.Password,
                Name = request.Name,
                LastName = request.LastName,
                ConfirmPassword = request.ConfirmPassword,
                UserName = request.UserName
            }));
        }

    }
}
