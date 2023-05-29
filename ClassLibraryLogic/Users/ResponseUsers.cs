using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;

namespace ClassLibraryLogic.Users
{
    public class ResponseUsers
    {
        public HttpStatusCode HttpStatus { get; set; }
        public string Message { get; set; }
        public ObservableCollection<IdentityUser> IdentityUsers { get; set; }
        public ResponseUsers(HttpStatusCode httpStatusCode, string message, ObservableCollection<IdentityUser> identityUsers)
        {
            HttpStatus = httpStatusCode;
            Message = message;
            IdentityUsers = identityUsers;
        }
    }
}
