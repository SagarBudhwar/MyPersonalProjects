using DatingApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DatingApp.Data
{
    public interface IAuthRepository
    {
        Task<CommonResponse> RegisterUser(User user);
        string CreateToken(User user);
        Task<List<User>> GetUserList();
    }
}
