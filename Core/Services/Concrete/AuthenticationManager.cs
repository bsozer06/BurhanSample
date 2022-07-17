using BurhanSample.Core.Services.Abstract;
using BurhanSample.Core.Utilities.Models.Users;
using BurhanSample.Entities.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BurhanSample.Core.Services.Concrete
{
    public class AuthenticationManager : IAuthenticationManager
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private User _user;

        /// AppSettings 'de bu standartta olmalı
        private const string jwtSettings = "JwtSettings";
        private const string secretKey = "secretKey";
        private const string validIssuer = "validIssuer";
        private const string validAudience = "validAudience";
        private const string expires = "expires";

        public AuthenticationManager(UserManager<User> userManager, IConfiguration configuration)
        { 
            _userManager = userManager; 
            _configuration = configuration;
        }


        public async Task<bool> ValidateUser(UserForAuthenticationDto userForAuth)
        {
            _user = await _userManager.FindByNameAsync(userForAuth.UserName);
            var result = _user != null && await _userManager.CheckPasswordAsync(_user, userForAuth.Password);
            return result;
        }

        public async Task<string> CreateToken()
        {
            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaims();
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);

            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }

        private SigningCredentials GetSigningCredentials()
        {
            /// bu standartta olmalı ve tanımlanmalı !!!
            var key = Encoding.UTF8.GetBytes(_configuration[jwtSettings + ":" + secretKey]);
            var secret = new SymmetricSecurityKey(key);

            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        private async Task<List<Claim>> GetClaims()
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, _user.UserName)
            };

            var roles = await _userManager.GetRolesAsync(_user);

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims;
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var jwtSettingsConfigs = _configuration.GetSection(jwtSettings);
            var tokenOptions = new JwtSecurityToken
                (
                    issuer: jwtSettingsConfigs.GetSection(validIssuer).Value,
                    audience: jwtSettingsConfigs.GetSection(validAudience).Value,
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettingsConfigs.GetSection(expires).Value)),
                    signingCredentials: signingCredentials
                );

            return tokenOptions;
        }


    }
}
