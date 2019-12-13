using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XTabAPI.Domain
{
    public class SQLParameter
    {
        private string paramName;
        public String ParamName
        {
            get { return "@" + paramName; }
            set { paramName = value; }
        }
        public SqlDbType SqlDBType { get; set; }
        public int? size { get; set; }
        public ParameterDirection? ParamDirection { get; set; }

        public object ParamValue;

        public string typeName;
    }
}
