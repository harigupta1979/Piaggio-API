using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;

namespace Piaggio_API.JWT
{
    public class Auth : IJwtAuth
    {
        private readonly string key;
        public Auth(string key)
        {
            this.key = key;
        }
        public string AuthenticationToken(string username,string role, Core.Module.CommonList commonList)
        {
            // 1. Create Security Token Handler
            var tokenHandler = new JwtSecurityTokenHandler();

            // 2. Create Private Key to Encrypted
            var tokenKey = Encoding.ASCII.GetBytes(key);

            //3. Create JETdescriptor
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(
                    new Claim[]
                    {
                        new Claim("USER_NAME", commonList.Data.Rows[0]["USER_NAME"].ToString()),
                        new Claim("USER_ID", commonList.Data.Rows[0]["USER_ID"].ToString()),
                        new Claim("NAME", commonList.Data.Rows[0]["USER_FIRST_NAME"].ToString()+" "+commonList.Data.Rows[0]["USER_LAST_NAME"].ToString()),
                        new Claim("USER_TYPE", commonList.Data.Rows[0]["USER_TYPE"].ToString()),
                        new Claim("IS_ADMIN", commonList.Data.Rows[0]["IS_ADMIN"].ToString()),
                        new Claim("DEALER_ID", commonList.Data.Rows[0]["DEALER_ID"].ToString()),
                    }),
                    Expires = DateTime.UtcNow.AddMinutes(10),
                    SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };
            //4. Create Token
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // 5. Return Token from method
            return tokenHandler.WriteToken(token);
        }
    }
}
