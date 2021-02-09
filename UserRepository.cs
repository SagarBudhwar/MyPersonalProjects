using Dapper;
using DatingApp.DbConnection;
using DatingApp.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.Data
{
    public class UsersRepository:IUsersRepository
    {

        private readonly IOptions<DbConnect> _appSettings;
        public UsersRepository(IOptions<DbConnect> appSettings)
        {
            _appSettings = appSettings;
                
        }

        public async Task<List<User>> GetUserList()
        {
            using (var db = new SqlConnection(_appSettings.Value.DefaultConnection))
            {
                db.Open();
                string readSp = "GetAllUsers";

                var res = await  db.QueryAsync<User>(readSp,new { }, commandType: CommandType.StoredProcedure);
                db.Close();
                return res.ToList();
            }
        }
        public async Task<List<VehicleList>> GetVehicleList()
        {
            using (var db = new SqlConnection(_appSettings.Value.DefaultConnection))
            {
                db.Open();
                string readSp = "GetVehicleListForDdl";

                var res = await db.QueryAsync<VehicleList>(readSp, new { }, commandType: CommandType.StoredProcedure);
                db.Close();
                return res.ToList();
            }
        }

        
    }
}
