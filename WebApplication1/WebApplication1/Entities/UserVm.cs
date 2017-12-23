using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RolesAndClaims.Entities
{
    public class UserVm
    {
        public User User { get; set; }
        public IList<Claim> Claims { get; set; }
    }
}
