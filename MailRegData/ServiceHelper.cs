using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace MailRegData
{
    internal static class ServiceHelper
    {
        public static String ConnectionString { get; }

        static ServiceHelper()
        {
            ConnectionString= "data source=localhost; initial catalog=email_saver_db; " + "persist security info=True; Integrated Security=SSPI; " +"MultipleActiveResultSets=True;";

            if (!String.IsNullOrEmpty(ConnectionString)) 
				return;

            throw new Exception("////not found connection////");
        }

		public static async Task<TItem> GetItemAsync<TItem>(SqlCommand command, IMapper<TItem> mapper)
			where TItem : class
		{
			try
			{
				if (command.Connection.State != ConnectionState.Open) 
					await command.Connection.OpenAsync();

				using SqlDataReader reader = await command.ExecuteReaderAsync();

				TItem result = null;
				if (await reader.ReadAsync()) 
					result = mapper.ReadIteam(reader);

				return result;
			}
			catch
			{
				throw new Exception("///GetItem////");
			}
		}
		public static async Task<TItem> FindItemAsync<TItem>(SqlCommand command, IMapper<TItem> mapper) where TItem : class
        {
            try
            {
                if (command.Connection.State != ConnectionState.Open) 
					await command.Connection.OpenAsync();
               
                using SqlDataReader sqlReader = await command.ExecuteReaderAsync();

                TItem result = null;
                if (await sqlReader.ReadAsync()) 
					result = mapper.ReadIteam(sqlReader);

				throw new Exception("Item not found.");
			}
			catch 
			{
				throw new Exception("//ex FindItem");
			}
		}

		public static async Task<List<TItem>> GetItemsAsync<TItem>(SqlCommand command, IMapper<TItem> mapper)
			where TItem : class
		{
			try
			{
				if (command.Connection.State != ConnectionState.Open) 
					await command.Connection.OpenAsync();

				using SqlDataReader sqlReader =await command.ExecuteReaderAsync();

				var result = new List<TItem>();

				while (await sqlReader.ReadAsync()) 
					result.Add(mapper.ReadIteam(sqlReader));

				return result;
			}
			catch 
			{
				throw new Exception("///ex GetItems///");
			}
		}

		public static async Task<Guid> AddItemAsync(SqlCommand command)
        {
            try
            {
                if (command.Connection.State!=ConnectionState.Open) 
					await command.Connection.OpenAsync();

                if (await command.ExecuteScalarAsync() is Guid id) 
					return id;

                throw new Exception("Not added");
            }
            catch
            {
                throw new Exception("///ex AddItem///");
            }
        }

		public static async Task ExecuteAsync(SqlCommand command)
		{
			try
			{
				if (command.Connection.State != ConnectionState.Open) 
					await command.Connection.OpenAsync();

				await command.ExecuteNonQueryAsync();
			}
			catch 
			{
				throw new Exception("///ex Execute///");
			}
		}
		/// <summary>
		/// Creates a command without attaching to any transaction.
		/// </summary>
		/// <param name="procedure">Name of procedure to be called.</param>
		/// <param name="connection">Current connection to database.</param>
		/// <param name="params">Set of parameters to the procedure.</param>
		public static SqlCommand CreateProcedureCommand(String procedure, SqlConnection connection,
			params SqlParameter[] @params)
		{
			var command = new SqlCommand(procedure, connection);

			command.Parameters.AddRange(@params);
			command.CommandType = CommandType.StoredProcedure;

			return command;
		}

		/// <summary>
		/// Creates a command as a part of transaction.
		/// </summary>
		/// <param name="procedure">Name of procedure to be called.</param>
		/// <param name="transaction">Current transaction to attach the procedure.</param>
		/// <param name="params">Set of parameters to the procedure.</param>
		public static SqlCommand CreateProcedureCommand(String procedure, SqlTransaction transaction,
			params SqlParameter[] @params)
		{
			var command = new SqlCommand(procedure, transaction.Connection, transaction);

			command.Parameters.AddRange(@params);
			command.CommandType = CommandType.StoredProcedure;

			return command;
		}
	}	
}