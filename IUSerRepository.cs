using DatingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.Data
{
    public interface IUsersRepository
    {
       Task<List<User>> GetUserList();
        Task<List<VehicleList>> GetVehicleList();

    }
}
