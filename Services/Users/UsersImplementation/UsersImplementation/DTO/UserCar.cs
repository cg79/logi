using MongoRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsersImplementation.DTO
{
    public class UserCar : BaseMongoDTO
    {
        public Guid CarGuid { get; set; }
        public Guid UserGuid { get; set; }



        public string NrInm { get; set; }
        public int MarcaID { get; set; }
        public string Marca { get; set; }
        
        public int ModelID { get; set; }
        public string Model { get; set; }

        public int Carac { get; set; }

        public int ConfortID { get; set; }
        public string C_ID { get; set; }

        public int NrLocuri { get; set; }
        
        public int TipMasinaID { get; set; }
        public string Tip_M_ID { get; set; }

        public List<vdim> vdim { get; set; }
        public vdim volume { get; set; }
        public string ImgUrl { get; set; }
        //IsCalculatedVolume
        public bool ICV { get; set; }

        //transport greutate
        public vdim tg { get; set; }
    }
}
