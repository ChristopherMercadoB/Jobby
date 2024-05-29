using Jobby.Core.Application.DTOS.Account;
using Jobby.Core.Application.Wrappers;


namespace Jobby.Core.Application.Interfaces.Services
{
    public interface IAccountService
    {
        Task<Response<AuthenticationResponse>> AuthenticateApiAsync(AuthenticationRequest request);
        Task<Response<string>> RegisterAsyncApi(RegisterRequest request);
    }
}
