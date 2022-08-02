using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace MailRegData
{
    internal interface IMapper<out TItem>
    {
        TItem ReadIteam(SqlDataReader sqlReader);  
    }
}
