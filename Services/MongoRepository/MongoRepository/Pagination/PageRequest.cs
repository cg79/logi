
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoRepository.Pagination
{
    public class PageRequest
    {
        public int PageIndex { get; set; }
        public int RowsPerPage { get; set; }
        public List<SortCriteria> SortCriteria { get; set; }
        public List<FilterCriteria> FilterCriteria { get; set; }
    }
}
