using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RolesAndClaims.Entities
{
    public class StaticUser
    {
        public string FirstName { get; set; }
        public string Email { get; set; }
        public int? Age { get; set; }
        public string Role { get; set; }
    }
}
