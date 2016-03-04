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
    public class CompanyRepository : MRepository<Company>
    {
        public object GetCompanyByUserGuid(string request)
        {
            Dictionary<string, object> dictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(request);
            Guid userGuid = Guid.Parse(dictionary["userGuid"].ToString());

            UsersRepository usersRepository = new UsersRepository();
            User user = usersRepository.GetByGuid(userGuid);
            if(user == null)
                return null;

            Company rez = this.GetByFilter(it => it.Guid == user.CompanyGuid).FirstOrDefault();

            return rez;
        }

        public object SaveCompany(string request)
        {
            Company company = request.JsonDeserialize<Company>();
            return SaveComp(company);
        }
        private object SaveComp(Company company)
        {
            if (company.Guid == Guid.Empty)
            {
                company.Guid = Guid.NewGuid();
                this.Save(company);

                UsersRepository userRepository = new UsersRepository();
                userRepository.SetCompanyForUser(company.UserGuid,company.Guid);

                return new
                {
                    code=0,
                    Guid = company.Guid
                };
            }

            Company dbCompany = this.GetByFilter(it => it.Guid == company.Guid).FirstOrDefault();
            if (dbCompany == null)
            {
                return new
                {
                    code = 1,
                    Guid = company.Guid
                };
            }

            dbCompany.CIF = company.CIF;
            dbCompany.Email = company.Email;
            dbCompany.NumeFirma = company.NumeFirma;
            dbCompany.Phones = company.Phones;
            dbCompany.PuncteLucru = company.PuncteLucru;
            dbCompany.REG_Comert = company.REG_Comert;
            dbCompany.WebSite = company.WebSite;

            this.Save(dbCompany);

            return new
            {
                code = 0,
            };
        }
    }
}
