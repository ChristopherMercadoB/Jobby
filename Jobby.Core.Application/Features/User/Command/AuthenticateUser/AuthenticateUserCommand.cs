using Jobby.Core.Application.DTOS.Account;
using Jobby.Core.Application.Interfaces.Services;
using Jobby.Core.Application.Wrappers;
using MediatR;


namespace Jobby.Core.Application.Features.User.Command.AuthenticateUser
{
    public class AuthenticateUserCommand:IRequest<Response<AuthenticationResponse>>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class AuthenticateUserCommandHandler : IRequestHandler<AuthenticateUserCommand, Response<AuthenticationResponse>>
    {
        private readonly IAccountService _accountService;
        

        public AuthenticateUserCommandHandler(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task<Response<AuthenticationResponse>> Handle(AuthenticateUserCommand request, CancellationToken cancellationToken)
        {
            return await _accountService.AuthenticateApiAsync(new AuthenticationRequest()
            {
                Email = request.Email,
                Password = request.Password,
            });
        }
    }

}
