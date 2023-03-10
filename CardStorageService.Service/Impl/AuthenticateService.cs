using CardStorageService.Data;
using CardStorageService.Data.Entity;
using CardStorageService.Models;
using CardStorageService.Models.Requests;
using CardStorageServiceAll.Models.DTO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace CardStorageService.Services.Impl
{
    public class AuthenticateService : IAuthenticateService<AuthenticationResponse, AuthenticationRequest,SessionInfo>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;


        private readonly Dictionary<string, SessionInfo> _sessions =
            new Dictionary<string, SessionInfo>();

        public const string SecretKey = "kYp3s6v9y/B?E(H+";

        public AuthenticateService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public SessionInfo GetSessionInfo(string sessionToken)
        {
            throw new System.NotImplementedException();
        }

        public AuthenticationResponse Login(AuthenticationRequest authenticationRequest)
        {
            using IServiceScope scope = _serviceScopeFactory.CreateScope();
            CardStorageServiceDbContext context = scope.ServiceProvider.GetRequiredService<CardStorageServiceDbContext>();

            Account account =
             !string.IsNullOrWhiteSpace(authenticationRequest.Login) ? 
             FindAccountByLogin(context, authenticationRequest.Login) : null;

            if (account == null)
            {
                return new AuthenticationResponse
                {
                    Status = AuthenticationStatus.UserNotFound
                };
            }

            if (!PasswordUtils.VerifyPassword(authenticationRequest.Password, account.PasswordSalt, account.PasswordHash))
            {
                return new AuthenticationResponse
                {
                    Status = AuthenticationStatus.InvalidPassword
                };
            }


            AccountSession session = new AccountSession
            {
                AccountId = account.AccountId,
                SessionToken = CreateSessionToken(account),
                TimeCreated = DateTime.Now,
                TimeLastRequest = DateTime.Now,
                IsClosed = false,
            };

            context.AccountSessions.Add(session);

            context.SaveChanges();


            SessionInfo sessionInfo = GetSessionInfo(account, session);

            lock (_sessions)
            {
                _sessions[sessionInfo.SessionToken] = sessionInfo;
            }


            return new AuthenticationResponse
            {
                Status = AuthenticationStatus.Success,
                SessionInfo = sessionInfo
            };

        }

        private SessionInfo GetSessionInfo(Account account, AccountSession accountSession)
        {
            return new SessionInfo
            {
                SessionId = accountSession.SessionId,
                SessionToken = accountSession.SessionToken,
                Account = new AccountDto
                {
                    AccountId = account.AccountId,
                    EMail = account.EMail,
                    FirstName = account.FirstName,
                    LastName = account.LastName,
                    SecondName = account.SecondName,
                    Locked = account.Locked
                }
            };
        }


        private string CreateSessionToken(Account account)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.ASCII.GetBytes(SecretKey);
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    new Claim[]{
                        new Claim(ClaimTypes.NameIdentifier, account.AccountId.ToString()),
                        new Claim(ClaimTypes.Name, account.EMail),
                    }),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private Account FindAccountByLogin(CardStorageServiceDbContext context, string login)
        {
            return context.Accounts.FirstOrDefault(account => account.EMail == login);
        }
    }
}
