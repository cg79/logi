using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoRepository
{
    public class BaseMongoDTO
    {
        public string R { get; set; }
        private string id;
        public string Id
        {
            get
            {
                if (!string.IsNullOrEmpty(id))
                {
                    return id;
                }
                id = Guid.NewGuid().ToString("N");
                return id;
            }
            set { id = value; }
        }
    }
}
