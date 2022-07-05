using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailRegData
{
    public class TagMapper : IMapper<Tag>
    {
        public Tag ReadIteam(SqlDataReader sqlReader)
        {
            return new Tag((Guid)sqlReader["id"], (String)sqlReader["name"]);
        }
    }
}
