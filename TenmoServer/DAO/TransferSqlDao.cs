using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Transactions;
using System.Xml.Schema;
using TenmoServer.Exceptions;
using TenmoServer.Models;
using TenmoServer.Security;
using TenmoServer.Security.Models;

namespace TenmoServer.DAO
{
    public class TransferSqlDao : ITransferDao
    {
        private readonly string connectionString;

        public TransferSqlDao(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        ////// TODO: create transfer
        public Transfer CreateTransfer(Transfer transfer)
        {
            if (transfer.AccountFrom == transfer.AccountTo)
            {
                throw new DaoException("Transfer to the same account is not allowed.");
            }
            Transfer newTransfer = null;
            string insertSql = 
                "INSERT INTO transfer (transfer_type_id, transfer_status_id, account_from, account_to, amount) " +
                "OUTPUT INSERTED.transfer_id " +
                "VALUES (@transfer_type_id, @transfer_status_id, @account_from, @account_to, @amount);";

			int newTransferId = 0;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

					SqlCommand cmd = new SqlCommand(insertSql, conn);
                    cmd.Parameters.AddWithValue("@transfer_type_id", transfer.TransferTypeId);
                    cmd.Parameters.AddWithValue("@transfer_status_id", transfer.TransferStatusId);
                    cmd.Parameters.AddWithValue("@account_from", transfer.AccountFrom);
                    cmd.Parameters.AddWithValue("@account_to", transfer.AccountTo);
                    cmd.Parameters.AddWithValue("@amount", transfer.Amount);

                    newTransferId = Convert.ToInt32(cmd.ExecuteScalar());

					// If the transfer is approved, update the account balances
					if (transfer.TransferStatusId == 2)
					{
						string updateSenderBalanceSql = "UPDATE account SET balance = balance - @amount WHERE account_id = @account_from";
						string updateRecipientBalanceSql = "UPDATE account SET balance = balance + @amount WHERE account_id = @account_to";

						// Deduct amount from sender's account
						SqlCommand updateSenderCmd = new SqlCommand(updateSenderBalanceSql, conn);
						updateSenderCmd.Parameters.AddWithValue("@amount", transfer.Amount);
						updateSenderCmd.Parameters.AddWithValue("@account_from", transfer.AccountFrom);
						updateSenderCmd.ExecuteNonQuery();

						// Add amount to recipient's account
						SqlCommand updateRecipientCmd = new SqlCommand(updateRecipientBalanceSql, conn);
						updateRecipientCmd.Parameters.AddWithValue("@amount", transfer.Amount);
						updateRecipientCmd.Parameters.AddWithValue("@account_to", transfer.AccountTo);
						updateRecipientCmd.ExecuteNonQuery();
					}

				}
                newTransfer = GetTransferById(newTransferId);
            }
            catch (SqlException ex)
            {

                throw new DaoException("error thrown at create transfer", ex);
            }
            return newTransfer;
        }

        public Transfer UpdateTransfer(Transfer transfer)
        {
            Transfer updatedTransfer = null;
            string sql = "UPDATE transfer SET transfer_type_id = @transfer_type_id, transfer_status_id = @transfer_status_id, " +
                "account_from = @account_from, account_to = @account_to, amount = @amount " +
                "WHERE transfer_id = @transfer_id";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@transfer_id", transfer.TransferId);
                    cmd.Parameters.AddWithValue("@transfer_type_id", transfer.TransferTypeId);
                    cmd.Parameters.AddWithValue("@transfer_status_id", transfer.TransferStatusId);
                    cmd.Parameters.AddWithValue("@account_from", transfer.AccountFrom);
                    cmd.Parameters.AddWithValue("@account_to", transfer.AccountTo);
                    cmd.Parameters.AddWithValue("@amount", transfer.Amount);

                    int numberOfRowsAffected = cmd.ExecuteNonQuery();

                    if (numberOfRowsAffected == 0)
                    {
                        throw new DaoException("Zero rows affected, expected at least one");
                    }

					if (transfer.TransferStatusId == 2 && transfer.TransferTypeId == 2)
					{
						string updateSenderBalanceSql = "UPDATE account SET balance = balance - @amount WHERE account_id = @account_from";
						string updateRecipientBalanceSql = "UPDATE account SET balance = balance + @amount WHERE account_id = @account_to";

						// Deduct amount from sender's account
						SqlCommand updateSenderCmd = new SqlCommand(updateSenderBalanceSql, conn);
						updateSenderCmd.Parameters.AddWithValue("@amount", transfer.Amount);
						updateSenderCmd.Parameters.AddWithValue("@account_from", transfer.AccountFrom);
						updateSenderCmd.ExecuteNonQuery();

						// Add amount to recipient's account
						SqlCommand updateRecipientCmd = new SqlCommand(updateRecipientBalanceSql, conn);
						updateRecipientCmd.Parameters.AddWithValue("@amount", transfer.Amount);
						updateRecipientCmd.Parameters.AddWithValue("@account_to", transfer.AccountTo);
						updateRecipientCmd.ExecuteNonQuery();
					}

				}
                updatedTransfer = GetTransferById(transfer.TransferId);
            }
            catch (SqlException ex)
            {
                throw new DaoException("Sql exception occured in UpdateTransfer", ex);
            }
            return updatedTransfer;
        }
        public Transfer GetTransferById(int transferId)
        {
            Transfer transfer = null;

            string sql = "SELECT * FROM transfer WHERE transfer_id = @transfer_id";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@transfer_id", transferId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        transfer = MapRowToTransfer(reader);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new DaoException("SQL exception occurred on GetTransferById", ex);
            }

            return transfer;
        }
        // TODO: step 3 needs transfer id, transfer_from name, transfer_to name...  
        ////// need id, from(name!), to (name!), type("Request", "Send"), status("Pending", "Approved", "Rejected")
        public List<Transfer> GetTransfersOfUser(int userId)
        {
            List<Transfer> transfers = new List<Transfer>();
            

            string sql = @"SELECT transfer.transfer_id, transfer.transfer_type_id, transfer.transfer_status_id, transfer.amount, transfer.account_from, transfer.account_to, transfer_type.transfer_type_desc, transfer_status.transfer_status_desc, tenmo_user.username, account.balance 
                FROM transfer 
                JOIN transfer_type ON transfer.transfer_type_id = transfer_type.transfer_type_id 
                JOIN transfer_status ON transfer.transfer_status_id = transfer_status.transfer_status_id 
                JOIN account ON transfer.account_from = account.account_id 
                JOIN tenmo_user ON account.user_id = tenmo_user.user_id 
                WHERE transfer.account_from IN(Select account_id FROM account WHERE user_id = @user_id) 
                OR 
                transfer.account_to IN(SELECT account_id FROM account WHERE user_id = @user_id);";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@user_id", userId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Transfer transfer = MapRowToTransfer(reader);
                        transfers.Add(transfer);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new DaoException("SQL exception on GetTransfersForUser", ex); ;
            }

            return transfers;
        }

        public List<Transfer> GetPendingTransfers(int user_id)
        {
            List<Transfer> transfers = new List<Transfer>();
            string sql = @"SELECT transfer.transfer_id, tenmo_user.username, transfer.amount
                    FROM transfer 
                    JOIN account ON transfer.account_to = account.account_id 
                    JOIN tenmo_user ON tenmo_user.user_id = account.user_id 
                    WHERE transfer.transfer_status_id = 1 AND 
                    OR
                    transfer.account_from IN(SELECT account_id FROM account WHERE user_id = @user_id))
                    ";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@user_id", user_id);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Transfer transfer = MapRowToTransfer(reader);
                        transfers.Add(transfer);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new DaoException("SQL exception on GetPendingTransfers", ex); ;
            }

            return transfers;
        }

        private Transfer MapRowToTransfer(SqlDataReader reader)
        {
            Transfer transfer = new Transfer();
            transfer.TransferId = Convert.ToInt32(reader["transfer_id"]);
            transfer.TransferTypeId = Convert.ToInt32(reader["transfer_type_id"]);
            transfer.TransferStatusId = Convert.ToInt32(reader["transfer_status_id"]);
            transfer.AccountFrom = Convert.ToInt32(reader["account_from"]);
            transfer.AccountTo = Convert.ToInt32(reader["account_to"]);
            transfer.Amount = Convert.ToDecimal(reader["amount"]);
            return transfer;
        }
    }
}
