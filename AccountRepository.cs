using Dapper;
using DatingApp.DbConnection;
using DatingApp.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatingApp.Data
{
    public class AccountRepository: IAccountRepository
    {
        private readonly IOptions<DbConnect> _appSettings;
        public AccountRepository(IOptions<DbConnect> appSettings)
        {
            _appSettings = appSettings;   
        }

        public async Task<CommonResponse> RegisterUser(User user)
        {
            using (var con = new SqlConnection(_appSettings.Value.DefaultConnection))
            {
                con.Open();
                var result = await con.QueryAsync<CommonResponse>("SaveUser",
               param: new { UserName= user.UserName, PasswordHash=user.PasswordHash, PasswordSalt= user.PasswordSalt },
               commandType:CommandType.StoredProcedure
               );

                con.Close();
                return result.FirstOrDefault();

            }
        }

    }
}
