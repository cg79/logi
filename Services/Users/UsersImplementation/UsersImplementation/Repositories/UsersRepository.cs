using MongoDB.Driver;
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
    public class UsersRepository : MRepository<User>
    {
        public User UILogin(string request)
        {
            LoginObj login = request.JsonDeserialize<LoginObj>();
            return Login(login);
        }
        public User Login(LoginObj request)
        {
            User rez = new User();
            if (request == null)
            {
                rez.R = "EMPTY_REQUEST";
                return rez;
            }
          
            if (!string.IsNullOrEmpty(request.Password))
            {
                MD5 md = new MD5();
                md.Value = request.Password;
                request.Password = md.FingerPrint;
            }

            User tmp = this.GetByFilter(
                  it => (it.Login == request.Login || it.Email == request.Login) &&
                  it.Password == request.Password).FirstOrDefault();
          

            if (tmp != null)
            {
                //tmp.SessionId = Guid.NewGuid().ToString().ToUpper().Substring(0, 4);
                //this.Save(tmp);
                tmp.Password = string.Empty;
                return tmp;
            }

            rez.R = "INVALID_NAME_OR_PASSWORD";
            return rez;
        }

        public User NewUser(string request)
        {
            User req = request.JsonDeserialize<User>();
            return CreateUser(req);
        }
        public User CreateUser(User request)
        {
            User rez = new User();

            if (request == null)
            {
                rez.R = "EMPTY_REQUEST";
                return rez;
            }


            

            if (string.IsNullOrEmpty(request.Login) || string.IsNullOrEmpty(request.Password))
            {
                rez.R = "EMPTY_USER_OR_PASSWORD";
                return rez;
            }
            if (string.IsNullOrEmpty(request.FirstName) || string.IsNullOrEmpty(request.FirstName))
            {
                rez.R = "EMPTY_NAME";
                return rez;
            }

            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Email))
            {
                rez.R = "ID_EMAIL_INVALID";
                return rez;
            }
            if (string.IsNullOrEmpty(request.Phone) || string.IsNullOrEmpty(request.Phone))
            {
                rez.R = "ID_PHONE_INV";
                return rez;
            }

            if (request.BirthDay == null)
            {
                rez.R = "ID_BIRTH";
                return rez;
            }
            if (request.BirthDay > DateTime.Now)
            {
                rez.R = "BIRTH_INV";
                return rez;
            }
            int years = DateTime.Now.Year - request.BirthDay.Year;
            if (years > 90 || years < 16)
            {
                rez.R = "BIRTH_INV";
                return rez;
            }

            request.Guid = Guid.NewGuid();

            MD5 md = new MD5();
            md.Value = request.Password;
            request.Password = md.FingerPrint;

            User tmp = this.GetByFilter(it => it.Login == request.Login).FirstOrDefault();

            if (tmp != null)
            {
                rez.R = "USER_ALREADY_EXISTS";
                return rez;
            }

            if (!string.IsNullOrEmpty(request.Email))
            {
                tmp = this.GetByFilter(it => it.Email == request.Email).FirstOrDefault();

                if (tmp != null)
                {
                    rez.R = "EMAIL_ALREADY_REGISTERED";
                    return rez;
                }
            }
            request.Guid = Guid.NewGuid();
                 
            this.Save(request);
            return request;

        }

        public User UISendResetPassEmail(string request)
        {
            User req = request.JsonDeserialize<User>();
            return TryResetPassword(req);
        }
        public User TryResetPassword(User request)
        {
            User rez = new User();

            if (request == null)
            {
                rez.R = "EMPTY_REQUEST";
                return rez;
            }

            if (string.IsNullOrEmpty(request.Email))
            {
                rez.R = "EMPTY_EMAIL";
                return rez;
            }

            User tmp = this.GetByFilter(it => it.Email == request.Email).FirstOrDefault();
            if (tmp == null)
            {
                rez.R = "USER_NOT_FOUND_BY_EMAIL";
                return rez;
            }

            TempPassword tmpPassword = new TempPassword()
            {
                Id = tmp.Id,
                Password = Guid.NewGuid().ToString().Substring(0, 4).ToUpper(),
                Email = request.Email
            };

            TempPasswordRepository tempPasswordRepository = new TempPasswordRepository();
            tempPasswordRepository.Save(tmpPassword);
            //SendResetPasswordEmail();

            string emailBody = "Resetati parola folosind codul " + tmpPassword.Password;

            EmailHelper.SendEmail(request.Email, "Resetare Parola", emailBody);
            //rez.R = "RESET_EMAIL_SENT";
            return rez;

        }

        public User UIResetPassword(string request)
        {
            User req = request.JsonDeserialize<User>();
            return ResetPassword(req);
        }

        public object UIChangePassword(string jsonObject)
        {
            Dictionary<string, object> dictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonObject);
            Guid userGuid = Guid.Parse(dictionary["g"].ToString());



            User tmp = this.GetByFilter(it => it.Guid == userGuid).FirstOrDefault();

            if(tmp == null)
                return new { R = "USER_NOT_FOUND" };

            string oldP = dictionary["OldP"].ToString();
            MD5 md = new MD5();
            md.Value = oldP;
            oldP = md.FingerPrint;


            if (tmp.Password != oldP)
                return new { R="OLD_PASS_WRONG"};

            string newP = dictionary["Password"].ToString();
            md = new MD5();
            md.Value = newP;
            newP = md.FingerPrint;

            tmp.Password = newP;

            this.Save(tmp);

            return null;
        }

      
        
        /// <summary>
        /// login = code
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        internal User ResetPassword(User request)
        {
            User rez = new User();
            if (request == null)
            {
                rez.R = "EMPTY_REQUEST";
                return rez;
            }



            TempPasswordRepository tempPasswordRepository = new TempPasswordRepository();
            TempPassword tempPassword = tempPasswordRepository.GetByFilter(it => it.Password == request.Email).FirstOrDefault();
            if (tempPassword == null)
            {
                rez.R = "KEY_NOT_FOUND";
                return rez;
            }
            User tmp = this.GetByFilter(it => it.Email == tempPassword.Email).FirstOrDefault();
            if (tmp == null)
            {
                rez.R = "DEVICE_NOT_FOUND";
                return rez;
            }


            tempPasswordRepository.Remove(tempPassword.Id);

            MD5 md = new MD5();
            md.Value = request.Password;
            tmp.Password = md.FingerPrint;

            this.Save(tmp);


            return rez;
        }

        public object UISetAvatar(string jsonObject)
        {
            Dictionary<string, object> dictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonObject);
            Guid userGuid = Guid.Parse(dictionary["g"].ToString());
            User tmp = this.GetByFilter(it => it.Guid == userGuid).FirstOrDefault();
            if (tmp == null)
                return false;

            tmp.ImgUrl = dictionary["url"].ToString();

            this.Save(tmp);



            return new { url=tmp.ImgUrl};
        }

        public object UpdateUser(string jsonObject)
        {
            Dictionary<string, object> dictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonObject);
            Guid userGuid = Guid.Parse(dictionary["Guid"].ToString());
            User tmp = this.GetByFilter(it => it.Guid == userGuid).FirstOrDefault();
            if (tmp == null)
                return false;

            tmp.FirstName = dictionary["FirstName"].ToString();
            tmp.LastName = dictionary["LastName"].ToString();
            //tmp.Email = dictionary["Email"].ToString();
            tmp.Phone = dictionary["Phone"].ToString();
            tmp.Login = dictionary["Login"].ToString();
            this.Save(tmp);



            return null;
        }

        public List<User> GetThisUsers(List<Guid> userGuids)
        {
            List<IMongoQuery> queries = new List<IMongoQuery>();
            foreach(Guid userGuid in userGuids)
            {
                var uQuery = Query.EQ("Guid", userGuid);
                queries.Add(uQuery);
            }

            var queryString = Query.Or(queries);

            List<User> resultBsons = this.Find(queryString);
            return resultBsons;
        }

        public User GetByGuid(Guid userGuid)
        {
            User usr = this.GetByFilter(it => it.Guid == userGuid).FirstOrDefault();
            return usr;
        }
        internal void SetCompanyForUser(Guid userGuid, Guid companyGuid)
        {
            User usr = this.GetByFilter(it => it.Guid == userGuid).FirstOrDefault();
            if (usr == null)
            {
                return;
            }
            usr.CompanyGuid = companyGuid;

            this.Save(usr);
        }
    }
}
