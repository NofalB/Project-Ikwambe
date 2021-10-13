using Domain;
using Domain.DTO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class TokenService : ITokenService
    {
        private readonly IUserService _userService;

        private ILogger Logger { get; }

        private string Issuer { get; }
        private string Audience { get; }
        private TimeSpan ValidityDuration { get; }
        private SigningCredentials Credentials { get; }
        private TokenIdentityValidationParameters ValidationParameters { get; }

        public TokenService(IConfiguration Configuration, ILogger<TokenService> Logger, IUserService userService)
        {
            this.Logger = Logger;
            _userService = userService;

            Issuer = "DebugIssuer";// Configuration.GetClassValueChecked("JWT:Issuer", "DebugIssuer", Logger);
            Audience = "DebugAudience";// Configuration.GetClassValueChecked("JWT:Audience", "DebugAudience", Logger);
            ValidityDuration = TimeSpan.FromDays(1);// Todo: configure
            string Key = "DebugKey DebugKey";//Configuration.GetClassValueChecked("JWT:Key", "DebugKey DebugKey", Logger);

            SymmetricSecurityKey SecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));

            Credentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256Signature);

            ValidationParameters = new TokenIdentityValidationParameters(Issuer, Audience, SecurityKey);
        }

        public class TokenIdentityValidationParameters : TokenValidationParameters
        {
            public TokenIdentityValidationParameters(string Issuer, string Audience, SymmetricSecurityKey SecurityKey)
            {
                RequireSignedTokens = true;
                ValidAudience = Audience;
                ValidateAudience = true;
                ValidIssuer = Issuer;
                ValidateIssuer = true;
                ValidateIssuerSigningKey = true;
                ValidateLifetime = true;
                IssuerSigningKey = SecurityKey;
                AuthenticationType = JwtBearerDefaults.AuthenticationScheme;
            }
        }

        public async Task<LoginResult> CreateToken(LoginRequest Login)
        {
            // Todo: Check if username and password match with some database...
            // from the database
            User userExist = _userService.UserCheck(Login.Email, Login.Password);

            if(userExist != null)
            {
               // if(userExist.Role == Role.Admin)
               // {
                    JwtSecurityToken AdminToken = await CreateToken(new Claim[] {
                    new Claim(ClaimTypes.Role, userExist.Role.ToString()),
                    new Claim(ClaimTypes.Role, userExist.Role.ToString()),
                    new Claim(ClaimTypes.NameIdentifier, userExist.UserId.ToString()),
                    new Claim(ClaimTypes.Email, userExist.Email),
                    new Claim(ClaimTypes.Name, userExist.FirstName),
                    new Claim(ClaimTypes.Surname, userExist.LastName),
                    });

                    return new LoginResult(AdminToken);
                /*}
                else if (userExist.Role == Role.User)
                {
                    JwtSecurityToken userToken = await CreateToken(new Claim[] {
                    new Claim(ClaimTypes.Role, "User"),
                    //new Claim(ClaimTypes.Role, userExist.Role.ToString()),
                    new Claim(ClaimTypes.NameIdentifier, userExist.UserId.ToString()),
                    new Claim(ClaimTypes.Email, userExist.Email),
                    new Claim(ClaimTypes.Name, userExist.FirstName),
                    new Claim(ClaimTypes.Surname, userExist.LastName),
                    });

                    return new LoginResult(userToken);
                }*/
            }

            throw new Exception("user does not exist");
        }

        public async Task<ClaimsPrincipal> GetByValue(string Value)
        {
            if (Value == null)
            {
                throw new Exception("No Token supplied");
            }

            JwtSecurityTokenHandler Handler = new JwtSecurityTokenHandler();

            try
            {
                SecurityToken ValidatedToken;
                ClaimsPrincipal Principal = Handler.ValidateToken(Value, ValidationParameters, out ValidatedToken);

                return await Task.FromResult(Principal);
            }
            catch (Exception e)
            {
                throw;
            }
        }
        private async Task<JwtSecurityToken> CreateToken(Claim[] Claims)
        {
            JwtHeader Header = new JwtHeader(Credentials);

            JwtPayload Payload = new JwtPayload(Issuer, Audience, Claims, DateTime.UtcNow, DateTime.UtcNow.Add(ValidityDuration), DateTime.UtcNow);

            JwtSecurityToken SecurityToken = new JwtSecurityToken(Header, Payload);

            return await Task.FromResult(SecurityToken);
        }
    }
}
