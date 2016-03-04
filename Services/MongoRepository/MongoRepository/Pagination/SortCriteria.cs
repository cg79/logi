
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoRepository.Pagination
{
    public class SortCriteria 
    {
        public bool Ascending { get; set; }
        public string PropertyName { get; set; }
    }
}
