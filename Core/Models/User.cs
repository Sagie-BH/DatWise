using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class AppUser
    {
        public int ID { get; set; }
        public required string Name { get; set; }
        public required string Company { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public bool IsActive { get; set; }
    }
    public class UserCredentials
    {
        public string Email { get; set; }
        public string Password { get; set; }  
    }

}
