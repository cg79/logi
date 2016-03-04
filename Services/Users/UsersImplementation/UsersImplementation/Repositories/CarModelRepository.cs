using ESB.Utils;
using MongoRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersImplementation.DTO;
using ESB.Utils.Serializers;

namespace UsersImplementation.Repositories
{
    public class CarModelRepository : MRepository<CarModel>
    {
        public List<CarModel> GetModelsForCar(string req)
        {
            IDReq idReq = req.JsonDeserialize<IDReq>();
            List < CarModel > rez = this.GetByFilter(it=>it.CarID == idReq.ID).ToList();
            return rez;
        }
    }
}
