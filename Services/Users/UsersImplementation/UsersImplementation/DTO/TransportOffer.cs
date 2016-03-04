using MongoRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsersImplementation.DTO
{
    public class TransportOffer : BaseMongoDTO
    {
        public Guid UserGuid { get; set; }
        public Guid CarGuid { get; set; }
        public StartDate StartDate { get; set; }
        public bool UseRec { get; set; }
        public Rec Rec { get; set; }
        public string Desc { get; set; }
        public Route Route { get; set; }
    }
}
