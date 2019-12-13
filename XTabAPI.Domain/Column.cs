using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XTabAPI.Domain
{
    public class Column
    {
        public int Id { get; set; }
        public string columnName { get; set; }
        public string dataType { get; set; }
        public bool isPrimaryKey { get; set; }
        public bool selectField { get; set; }
        public bool grpcolumnselected { get; set; }
    }
}
