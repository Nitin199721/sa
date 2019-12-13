using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XTabAPI.Domain
{
    public class DBSchema
    {
        public string TableName { get; set; }
        public string ColumnName { get; set; }
        public int ColumnId { get; set; }
        public string DataType { get; set; }
        public bool IsPrimaryKey { get; set; }
    }
}
