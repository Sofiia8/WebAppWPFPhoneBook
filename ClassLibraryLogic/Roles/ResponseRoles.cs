using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibraryLogic.Roles
{
    public class ResponseRoles
    {
        public HttpStatusCode HttpStatus { get; set; }
        public string Message { get; set; }
        public ObservableCollection<IdentityRole> IdentityRoles { get; set; }
        public ResponseRoles(HttpStatusCode httpStatusCode, string message, ObservableCollection<IdentityRole> identityRoles)
        {
            HttpStatus = httpStatusCode;
            Message = message;
            IdentityRoles = identityRoles;
        }
    }
}
