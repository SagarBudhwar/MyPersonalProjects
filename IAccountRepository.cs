using DatingApp.Models;
using System;
using System.Threading.Tasks;

namespace DatingApp.Data
{
    public interface IAccountRepository
    {

        Task<CommonResponse> RegisterUser(User user);
    }
}
