using ClassLibraryLogic.Models;
using ClassLibraryLogic.Users;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace ClassLibraryLogic.Roles
{
    public interface IRole
    {
        Task<ResponseRoles> GetRoles(string token);
        Task<ResponseUsers> UserList(string token);
        Task<ChangeRoleModel> GetRolesByUser(string userId, string userName, string jwt);
        Task<HttpStatusCode> Edit(string userId, List<string> roles, string jwt);
    }
}
