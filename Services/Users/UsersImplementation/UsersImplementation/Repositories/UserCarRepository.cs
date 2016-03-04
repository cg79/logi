using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoRepository;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using UsersImplementation.DTO;
using Utils;


namespace UsersImplementation.Repositories
{
    public class UserCarRepository : MRepository<UserCar>
    {
        public object AddEditCar(string jsonObject)
        {
            Dictionary<string, object> dictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonObject);
            Guid userGuid = Guid.Parse(dictionary["Guid"].ToString());

            UserCar inputCar = dictionary["car"].ToString().JsonDeserialize<UserCar>();
            Guid carGuid = inputCar.CarGuid;
            inputCar.UserGuid = userGuid;

            if (carGuid == Guid.Empty)
            {
                inputCar.CarGuid = Guid.NewGuid();

                this.Save(inputCar);
            }
            else {
                UserCar dbCar = this.GetByFilter(it => it.CarGuid == carGuid).FirstOrDefault();
                if (dbCar == null)
                {
                    return new  {R="CAR_NOT_FOUND" };
                }

                dbCar.NrLocuri = inputCar.NrLocuri;
                dbCar.MarcaID = inputCar.MarcaID;
                dbCar.Marca = inputCar.Marca;


                dbCar.ModelID = inputCar.ModelID;
                dbCar.Model = inputCar.Model;

                dbCar.Carac = inputCar.Carac;

                dbCar.ConfortID = inputCar.ConfortID;
                dbCar.C_ID = inputCar.C_ID;

                dbCar.NrInm = inputCar.NrInm;
                dbCar.Tip_M_ID = inputCar.Tip_M_ID;
                dbCar.TipMasinaID = inputCar.TipMasinaID;

                dbCar.vdim = inputCar.vdim;
                dbCar.volume = inputCar.volume;
                dbCar.tg = inputCar.tg;
                dbCar.ICV = inputCar.ICV;
                


                this.Save(dbCar);
            }

            return new  { CarGuid=inputCar.CarGuid};
        }

        public object CarPage(string request)
        {
           // this.GetByFilter
            return null;
        }

        public object GetMyCars(string jsonObject)
        {
            object response = null;
            Dictionary<string, object> dictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonObject);
            Guid userGuid = Guid.Parse(dictionary["Guid"].ToString());

            response = this.GetByFilter(it=>it.UserGuid == userGuid);
            return response;
        }
        protected override Expression<Func<UserCar, bool>> GetFuncExpr(MongoRepository.Pagination.FilterCriteria ft)
        {
            return x => x.UserGuid == Guid.NewGuid();
        }

        public object SaveOfertaTransport(string request)
        {
            try
            {
                TransportOffer transportOffer = request.JsonDeserialize<TransportOffer>();
                transportOffer.StartDate.isoDate = transportOffer.StartDate.localDate.AddMinutes(transportOffer.StartDate.offset);
                TransportOfferRepository transportOfferRepository = new TransportOfferRepository();

                transportOfferRepository.Save(transportOffer);


                return null;//new object { };
            }
            catch (Exception ex)
            {
                return new { ex=ex.Message};
            }
        }

        public List<UserCar> GetThisCars(List<Guid> userGuids)
        {
            List<IMongoQuery> queries = new List<IMongoQuery>();
            foreach (Guid userGuid in userGuids)
            {
                var uQuery = Query.EQ("CarGuid", userGuid);
                queries.Add(uQuery);
            }

            var queryString = Query.Or(queries);

            List<UserCar> resultBsons = this.Find(queryString);
            return resultBsons;
        }
       
       
    }
}
