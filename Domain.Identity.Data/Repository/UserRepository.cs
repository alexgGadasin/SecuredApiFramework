using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Database;

namespace Domain.Identity.Data
{
    public class UserRepository : RepoSqlSrvDbRepository<User>
    {
        public UserRepository(IUnitOfWork uow) : base(uow) { }
        public async Task<User> ReadSingle(User user)
        {
            string query = base.QuerySelect;
            IDictionary<string, object> parameters = new Dictionary<string, object>();

            if (user.UserId.Trim() != string.Empty)
            {
                query += " AND [UserID] = @UserID";
                parameters.Add("UserID", user.UserId);
            }
            if (user.UserName.Trim() != string.Empty)
            {
                query += " AND [UserName] = @UserName";
                parameters.Add("UserName", user.UserName);
            }
            if (user.Email.Trim() != string.Empty)
            {
                query += " AND [Email] = @Email";
                parameters.Add("Email", user.Email);
            }

            var response = await base.ReadByQuery(query: query, parameters: parameters);
            return response.FirstOrDefault();
        }
    }
}