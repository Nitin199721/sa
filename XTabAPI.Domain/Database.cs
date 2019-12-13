using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XTabAPI.Domain
{
    public class Database
    {
        public int Id { get; set; }
        public string DBName { get; set; }
        public List<Table> Tables { get; set; }
    }
}
