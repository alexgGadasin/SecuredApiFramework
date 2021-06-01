using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Linq;
using System.Security.Cryptography;

namespace Domain.Identity.Oauth
{
    public class OauthTokenManager
    {
        //
        // Summary:
        //     Generate access token
        //
        // Parameters:
        //   issuer: token issuer url
        //   audience: audience issuer url
        //   claims: list of claims which will be stored in the token
        //   secret: secret key for encryption
        //   expire_interval: expire interval (in seconds)
        public static string GenerateAccessToken(string issuer, string audience, string secret, List<Claim> claims, double expire_interval)
        {
            return GenerateJwtToken(issuer, audience, secret, claims, expire_interval);
        }
        public static string GenerateRefreshToken()
        {
            return GenerateRandomNumber();
        }
        public static string GenerateCode(string issuer, string audience, string secret)
        {
            return GenerateJwtToken(issuer, audience, secret, expire_interval: 10);
        }
        private static string GenerateJwtToken(string issuer, string audience, string secret, List<Claim> claims = null, double expire_interval = 10)
        {
            // Key credentials
            var secretBytes = Encoding.ASCII.GetBytes(secret);
            var key = new SymmetricSecurityKey(secretBytes);
            var algorithm = SecurityAlgorithms.HmacSha256;
            var signingCredentials = new SigningCredentials(key, algorithm);
            var issued_at = DateTime.Now;
            var utc0 = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

            // Create Token
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = new JwtSecurityToken(
                issuer,
                audience,
                claims,
                notBefore: issued_at,
                //expires: issued_at.AddSeconds(expire_interval),
                expires: issued_at.AddYears(1),
                signingCredentials: signingCredentials);

            token.Payload[JwtRegisteredClaimNames.Iat] = (int)issued_at.Subtract(utc0).TotalSeconds;            

            return tokenHandler.WriteToken(token);
        }
        private static string GenerateRandomNumber()
        {
            var randomNumber = new byte[32];
            using (var randomNumberGenerator = RandomNumberGenerator.Create())
                randomNumberGenerator.GetBytes(randomNumber);

            return Convert.ToBase64String(randomNumber);
        }
        /*
        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token, string secret)
        {
            var key = Encoding.ASCII.GetBytes(secret);
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid Token");
            return principal;
        }
        */
    }
}
