using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace MailRegData
{
    internal static class MailDataServiceHelpers
    {
        public static String ConnectionString { get; }

        public static async Task<TItem> FindItemAsync<TItem>(SqlCommand command, IMapper<TItem> mapper)
            where TItem : class
        {
            try
            {
                if (command.Connection.State == ConnectionState.Open) await command.Connection.OpenAsync();

                


            }

            catch
            {

            }

        }
    }
}