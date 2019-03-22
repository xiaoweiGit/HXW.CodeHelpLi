using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HXW.CodeHelpLi.DB
{
    public class SqlItemComparer : IComparer<SqlItem>
    {
        public int Compare(SqlItem x, SqlItem y)
        {
            int xSeq = 0;
            int ySeq = 0;

            if (x == null) xSeq = 0; else xSeq = x.Sequence;
            if (y == null) ySeq = 0; else ySeq = y.Sequence;

            return xSeq - ySeq;
        }
    }
}
