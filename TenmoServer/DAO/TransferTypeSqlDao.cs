﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using TenmoServer.Exceptions;
using TenmoServer.Models;
using TenmoServer.Security;
using TenmoServer.Security.Models;

namespace TenmoServer.DAO
{
    public class TransferTypeSqlDao : ITransferTypeDao
    {
        private readonly string connectionString;

        public TransferTypeSqlDao(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public List<TransferType> GetTransferTypes()
        {
            List<TransferType> transferTypes = new List<TransferType>();

            string sql = "SELECT transfer_type_id, transfer_type_desc " +
                "FROM transfer_type;";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        TransferType transferType = MapRowToTransferType(reader);
                        transferTypes.Add(transferType);
                    }

                }
            }
            catch (SqlException ex)
            {
                throw new DaoException("Sql exception thrown on GetTransferStatuses", ex);
            }
            return transferTypes;
        }

        public TransferType GetTransferTypeById(int transferTypeId)
        {
            TransferType transferType = null;
            string sql = "SELECT * " +
                "FROM transfer_type " +
                "WHERE transfer_type_id = @transfer_type_id";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@transfer_type_id", transferTypeId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        transferType = MapRowToTransferType(reader);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new DaoException("SQL error thrown in GetTransferStatusById", ex);
            }
            return transferType;
        }

        private TransferType MapRowToTransferType(SqlDataReader reader)
        {
            TransferType transferType = new TransferType();
            transferType.TransferTypeId = Convert.ToInt32(reader["transfer_type_id"]);
            transferType.TransferTypeDesc = Convert.ToString(reader["transfer_status_desc"]);
            return transferType;
        }
    }
}
