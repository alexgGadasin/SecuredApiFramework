using Domain.Api;
using Domain.Database;
using Domain.Identity.Data;
using System.Threading.Tasks;

namespace Domain.Identity.Business
{
    public interface IUser : IBusinessLogic<User>
    {
        Task RegisterUser(User data);
        Task<User> GetById(string UserID);
    }
}
