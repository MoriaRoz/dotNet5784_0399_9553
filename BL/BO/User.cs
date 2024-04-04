using BlImplementation;
using DalApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System;


namespace BO
{
    public class User
    {
        public int Id { get; init; }
        public String? Password { get; set; }
        public UserRole Role { get; set; }
    }
    
}
