using ClassLibraryLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibraryLogic.Users
{
    public interface IUser
    {
        Task<ResponseUsers> GetUsers(string token);
        Task<string[]> Create(RegisterModel model, string jwt);
        Task<string[]> Edit(EditUserModel model, string jwt);
        Task<HttpStatusCode> Delete(string id, string jwt);
    }
}
