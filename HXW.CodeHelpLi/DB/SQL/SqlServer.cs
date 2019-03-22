using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;

namespace HXW.CodeHelpLi.DB
{
    public class SqlServer : DataBase
    {
        public SqlServer()
            : base(SqlClientFactory.Instance)
        {
        }

        public SqlServer(string connectionString)
            : base(connectionString, SqlClientFactory.Instance)
        {
        }

    }
}
