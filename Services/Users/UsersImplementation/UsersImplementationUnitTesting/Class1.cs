using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersImplementation.Repositories;

namespace UsersImplementationUnitTesting
{
    public class Class1
    {
        [Test]
        public void AddUser()
        {
            UsersRepository ur = new UsersRepository();
            ur.CreateUser(new UsersImplementation.DTO.User() 
            {
                Email = "claudiu9379@yahoo.com",
                Login = "claudiu",
                Password = "aaa"
            });
        }

        [Test]
        public void TryResetPassword()
        {
            UsersRepository ur = new UsersRepository();
            ur.TryResetPassword(new UsersImplementation.DTO.User()
            {
                Email = "claudiu9379@yahoo.com",
                Login = "claudiu",
                Password = "aaa"
            });
        }
    }
}
