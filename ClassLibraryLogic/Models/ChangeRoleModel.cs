using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Net;

namespace ClassLibraryLogic.Models
{
    public class ChangeRoleModel
    {
        public string UserId { get; set; }
        public string UserLogin { get; set; }
        public List<IdentityRole> AllRoles { get; set; }
        public IList<string> UserRoles { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public ChangeRoleModel()
        {
            AllRoles = new List<IdentityRole>();
            UserRoles = new List<string>();
        }
    }
}
