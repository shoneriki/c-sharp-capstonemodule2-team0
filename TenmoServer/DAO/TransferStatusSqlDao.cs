using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using TenmoServer.Exceptions;
using TenmoServer.Models;
using TenmoServer.Security;
using TenmoServer.Security.Models;

namespace TenmoServer.DAO
{
    public class TransferStatusSqlDao : ITransferStatusDao
    {
        private readonly string connectionString;

        public TransferStatusSqlDao(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public List<TransferStatus> GetTransferStatuses()
        {
            List<TransferStatus> transferStatuses = new List<TransferStatus>();

            string sql = "SELECT transfer_status_id, transfer_status_desc " +
                "FROM transfer_status;";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        TransferStatus status = MapRowToTransferStatus(reader);
                        transferStatuses.Add(status);                        
                    }

                }
            }
            catch (SqlException ex)
            {
                throw new DaoException("Sql exception thrown on GetTransferStatuses", ex);
            }
            return transferStatuses;
        }

        public TransferStatus GetTransferStatusById(int transferStatusId)
        {
            TransferStatus transferStatus = null;
            string sql = "SELECT * " +
                "FROM transfer_status " +
                "WHERE transfer_status_id = @transfer_status_id";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@transfer_status_id", transferStatusId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        transferStatus = MapRowToTransferStatus(reader);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new DaoException("SQL error thrown in GetTransferStatusById", ex);
            }
            return transferStatus;
        }

        private TransferStatus MapRowToTransferStatus(SqlDataReader reader)
        {
            TransferStatus transferStatus = new TransferStatus();
            transferStatus.TransferStatusId = Convert.ToInt32(reader["transfer_status_id"]);
            transferStatus.TransferStatusDesc = Convert.ToString(reader["transfer_status_desc"]);
            return transferStatus;
        }
    }
}
