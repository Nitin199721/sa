using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XTabAPI.Domain
{
    public class Table
    {
        public int Id { get; set; }
        public string tableName { get; set; }
        public bool selectAllFields { get; set; }
        public string tableAlias { get; set; }
        public List<Column> columns { get; set; }
    }
}
