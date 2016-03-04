using MongoRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsersImplementation.DTO
{
    public class User : BaseMongoDTO
    {
        public Guid Guid { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string FBId { get; set; }
        public Guid  CompanyGuid { get; set; }
        public string ImgUrl { get; set; }
        public int Sex { get; set; }
        public DateTime BirthDay { get; set; }
    }
}
