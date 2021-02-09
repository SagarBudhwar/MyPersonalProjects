using Dapper;
using DatingApp.DbConnection;
using DatingApp.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DatingApp.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly SymmetricSecurityKey _key;
        IOptions<DbConnect> _appSettings;
        public AuthRepository(IConfiguration configuration, IOptions<DbConnect> appSettings)
        {
            _appSettings = appSettings;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["TokenKey"]));
        }
        public async Task<CommonResponse> RegisterUser(User user)
        {
            using (var con = new SqlConnection(_appSettings.Value.DefaultConnection))
            {
                con.Open();
                var result = await con.QueryAsync<CommonResponse>("SaveUser",
               param: new { UserName = user.UserName, PasswordHash = user.PasswordHash, PasswordSalt = user.PasswordSalt },
               commandType: CommandType.StoredProcedure
               );

                con.Close();
                return result.FirstOrDefault();

            }
        }
        public async Task<List<User>> GetUserList()
        {
            using (var db = new SqlConnection(_appSettings.Value.DefaultConnection))
            {
                db.Open();
                string readSp = "GetAllUsers";

                var res = await db.QueryAsync<User>(readSp, new { }, commandType: CommandType.StoredProcedure);
                db.Close();
                return res.ToList();
            }
        }
        public string CreateToken(User user)
        {
            //var Claims = new List<Claim>();
            //Claim claim = new Claim(JwtRegisteredClaimNames.NameId,user.UserName);
            //Claims.Add(claim);

            var Claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId,user.UserName)
            };

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(Claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds

            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(securityTokenDescriptor);
            return tokenHandler.WriteToken(token);

        }
    }
}
