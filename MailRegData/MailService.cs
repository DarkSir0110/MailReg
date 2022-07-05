using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace MailRegData
{
    internal static class MailDataService
    {
        public static String ConnectionString { get; }

        static MailDataService()
        {
            ConnectionString =
                "data source=localhost; initial catalog=email_server_db; " +
                "persist security info=True; Integrated Security=SSPI; " +
                "MultipleActiveResultSets=True;";

            if (!String.IsNullOrEmpty(ConnectionString)) return;

            throw new Exception("Connection string not found in cfg");

        }

        public static async Task<TItem> FindItemAsync<TItem>(SqlCommand command, IMapper<TItem> mapper)
			where TItem : class
        { 
				try
				{
					if (command.Connection.State != ConnectionState.Open) await command.Connection.OpenAsync();
					await 
                    
             using SqlDataReader reader = await command.ExecuteReaderAsync();

					TItem result = null;
					if (await reader.ReadAsync()) result = mapper.ReadIteam(reader);

					return result;
				}
				catch (Exception ex)
				{
					String message = ExceptionManager.BuildMessage(ex, command);
					throw new Exception(message, ex);
				}
			
        }
    }
}
