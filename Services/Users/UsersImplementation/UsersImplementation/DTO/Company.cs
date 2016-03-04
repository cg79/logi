using MongoRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsersImplementation.DTO
{
    public class Company : BaseMongoDTO
    {
        public Guid UserGuid { get; set; }
        public Guid Guid { get; set; }
        public string NumeFirma { get; set; }
        public string SiglaUrl { get; set; }
        public string CIF { get; set; }
        public string REG_Comert { get; set; }
        public PuncteLucru PuncteLucru { get; set; }
        public Phones Phones { get; set; }
        public string Email { get; set; }
        public string WebSite { get; set; }
    }
}
