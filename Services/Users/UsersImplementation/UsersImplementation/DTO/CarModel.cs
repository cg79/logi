using MongoRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsersImplementation.DTO
{
    public class CarModel : BaseMongoDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public int CarID { get; set; }
    }
}
