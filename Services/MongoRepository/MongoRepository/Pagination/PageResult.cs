using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoRepository.Pagination
{
    public class PageResult<T>
    {
        public PageRequest PageRequest { get; set; }
        public List<T> Items { get; set; }
        public int TotalRecords { get; set; }

    }
}
