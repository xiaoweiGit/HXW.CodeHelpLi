using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HXW.CodeHelpLi.DB
{
    public class SqlItem
    {
        public SqlItem()
        {
            this.Parameters = new List<DbParameter>();
        }

        public string Sql { get; set; }

        public int Sequence { get; set; }

        public List<DbParameter> Parameters { get; set; }

        public void InitCommand(DbCommand command)
        {
            command.CommandText = this.Sql;
            if (this.Parameters != null) command.Parameters.AddRange(this.Parameters.ToArray());
        }

    }
}
