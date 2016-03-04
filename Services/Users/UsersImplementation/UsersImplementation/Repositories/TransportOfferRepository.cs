using MongoDB.Driver.Builders;
using MongoRepository;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersImplementation.DTO;
using Utils;


namespace UsersImplementation.Repositories
{
    public class TransportOfferRepository : MRepository<TransportOffer>
    {
        public object SearchOfertaTransport(string req)
        {
            //http://stackoverflow.com/questions/27425691/query-nested-documents-with-c-sharp-mongodb
            try
            {
                TransportOffer request = req.JsonDeserialize<TransportOffer>();
                request.UseRec = (request.Rec != null);
                request.StartDate.isoDate = request.StartDate.localDate.AddMinutes(request.StartDate.offset);
                TransportOfferRepository transportOfferRepository = new TransportOfferRepository();

                Way startPoint = request.Route.way[0];
                Way endPoint = request.Route.way[request.Route.way.Count - 1];

                //var queryString = Query.EQ("Route.way.address.z", endPoint.address.z);
                var queryString = Query.And(
                    Query.EQ("Route.way.address.z", startPoint.address.z),
                    Query.EQ("Route.way.address.z", endPoint.address.z)
                );

                List<TransportOffer> resultBsons = this.Find(queryString);

                if (resultBsons.Count == 0)
                {
                    return null;
                }
                UsersRepository usersRepository = new UsersRepository();
                List<User> users = usersRepository.GetThisUsers(resultBsons.Select(it => it.UserGuid).ToList());

                    foreach(var u in users)
                    {
                        u.Password = "";
                    }

                    UserCarRepository carRepository = new UserCarRepository();
                var cars = carRepository.GetThisCars(resultBsons.Select(it => it.CarGuid).ToList());

                return new { routes = resultBsons ,cars = cars,users=users};
            }
            catch (Exception ex)
            {
                return new { ex = ex.Message };
            }
        }

        public object aaa()
        {
            var queryString = Query.EQ("Route.way.address.c", "BN");
            //queryString = Query<TransportOffer>.GT(i => i.Route.dist, 6464);

            var resultBsons = this.Find(queryString);
            return null;
        }
    }
}
