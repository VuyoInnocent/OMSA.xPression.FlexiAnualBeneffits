using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace SaGE.Common.ADO
{
	public class DataAccess : IDisposable
	{
		private SqlConnection connection = null;

		public DataAccess()
		{
		}

		public DataAccess(string connectionString)
		{
			connection = new SqlConnection(connectionString);
		}

		public DataTable ExecuteSelectCommand(string commandName, CommandType cmdType)
		{
			SqlCommand cmd = null;
			DataTable table = new DataTable();

			cmd = connection.CreateCommand();

			cmd.CommandType = cmdType;
			cmd.CommandText = commandName;

			try
			{
				connection.Open();

				SqlDataAdapter da = null;
				using (da = new SqlDataAdapter(cmd))
				{
					da.Fill(table);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				cmd.Dispose();
				cmd = null;
				connection.Close();
			}

			return table;
		}

		public DataTable ExecuteParamerizedSelectCommand(string commandName, CommandType cmdType, SqlParameter[] param)
		{
			SqlCommand cmd = null;
			DataTable table = new DataTable();

			cmd = connection.CreateCommand();

			cmd.CommandType = cmdType;
			cmd.CommandText = commandName;
			cmd.Parameters.AddRange(param);

			try
			{
				connection.Open();

				SqlDataAdapter da = null;
				using (da = new SqlDataAdapter(cmd))
				{
					da.Fill(table);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				cmd.Dispose();
				cmd = null;
				connection.Close();
			}

			return table;
		}

		public bool ExecuteNonQuery(string commandName, CommandType cmdType, SqlParameter[] pars)
		{
			SqlCommand cmd = null;
			int res;

			cmd = connection.CreateCommand();

			cmd.CommandType = cmdType;
			cmd.CommandText = commandName;
			cmd.Parameters.AddRange(pars);

			try
			{
				connection.Open();
				res = cmd.ExecuteNonQuery();
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				cmd.Dispose();
				cmd = null;
				connection.Close();
			}

			return res >= 1;
		}

		public void Dispose()
		{
			throw new NotImplementedException();
		}
	}
}