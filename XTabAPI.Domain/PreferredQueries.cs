using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XTabAPI.Domain
{
    public class PreferredQueries
    {
        public int PreferredQueriesId { get; set; }
        public string WindowsUserName { get; set; }
        public string QueryName { get; set; }
        public int DBDetailsId { get; set; }
        public string Query { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
