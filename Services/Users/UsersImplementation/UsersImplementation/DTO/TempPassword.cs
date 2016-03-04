using MongoRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsersImplementation.DTO
{
    public class TempPassword : BaseMongoDTO
    {
        public string Password { get; set; }
        public string Email { get; set; }
    }
}
