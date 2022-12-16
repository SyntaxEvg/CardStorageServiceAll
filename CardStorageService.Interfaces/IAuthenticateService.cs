
namespace CardStorageService.Services
{
    public interface IAuthenticateService<AuthenticationResponse, AuthenticationRequest, SessionInfo>
    {
        AuthenticationResponse Login(AuthenticationRequest authenticationRequest);

        public SessionInfo GetSessionInfo(string sessionToken);

    }
}
