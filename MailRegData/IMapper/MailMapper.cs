using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailRegData
{
    internal class MailMapper : IMapper<Mail>
    {
        public Mail ReadIteam(SqlDataReader sqlReader)
        {
            return new Mail((Guid) sqlReader["id"],
                            (String)sqlReader["name"],
                            (DateTime) sqlReader["reg_date"],
                            (String) sqlReader["recipient"],
                            (String) sqlReader["sender"],
                            (String) sqlReader["context"],
                            (String) sqlReader["tags"]
                            );
        }
    }
}
