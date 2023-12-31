﻿using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
            //string transferStatusFindSql =
            //    "SELECT transfer_status_id " +
            //    "FROM transfer_status " +
            //    "WHERE transfer_status.transfer_status_id = @transfer_status_id";
            //string transferTypeFindSql =
            //    "SELECT transfer_type_id " +
            //    "FROM transfer_type " +
            //    "WHERE transfer_type.transfer_type_id = @transfer_type_id";
            string insertSql = 
                "INSERT INTO transfer (transfer_type_id, transfer_status_id, account_from, account_to, amount) " +
                "OUTPUT INSERTED.transfer_id " +
                "VALUES (@transfer_type_id, @transfer_status_id, @account_from, @account_to, @amount);";

            int newTransferId = 0;

            try
            {
                //using (SqlConnection conn = new SqlConnection(connectionString))
                //{
                //    conn.Open();
                //    SqlCommand cmd = new SqlCommand(transferStatusFindSql, conn);
                //    cmd.Parameters.AddWithValue("@transfer_status_id", transfer.TransferStatusId);
                //    SqlDataReader reader = cmd.ExecuteReader();
                //    if (reader.Read())
                //    {
                //        transferStatusId = Convert.ToInt32(reader["transfer_status_id"]);
                //    }
                //}
                //using (SqlConnection conn = new SqlConnection(connectionString))
                //{
                //    conn.Open();
                //    SqlCommand cmd = new SqlCommand(transferTypeFindSql, conn);
                //    cmd.Parameters.AddWithValue("@transfer_type_id", transfer.TransferTypeId);
                //    SqlDataReader reader = cmd.ExecuteReader();
                //    if (reader.Read())
                //    {
                //        transferTypeId = Convert.ToInt32(reader["transfer_type_id"]);
                //    }
                //}
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
            //string sql = "SELECT transfer.transfer_id, transfer.transfer_type_id, transfer.transfer_status_id, " +
            //    "transfer.amount, transfer.account_from, transfer.account_to, transfer_type.transfer_type_desc, transfer_status.transfer_status_desc, " +
            //    "tenmo_user.username, account.balance " +
            //    "FROM transfer " +
            //    "JOIN transfer_type ON transfer.transfer_type_id = transfer_type.transfer_type_id " +
            //    "JOIN transfer_status ON transfer.transfer_status_id = transfer_status.transfer_status_id " +
            //    "JOIN account ON transfer.account_from = account.account_id " +
            //    "JOIN tenmo_user ON account.user_id = tenmo_user.user_id " +
            //    "WHERE transfer.account_from IN(Select account_id FROM account WHERE user_id = @user_id) " +
            //    "OR transfer.account_to IN(SELECT account_id FROM account WHERE user_id = @user_id);";

            string sql = "SELECT transfer.transfer_id, transfer.transfer_type_id, transfer.transfer_status_id, " +
    "transfer.amount, transfer.account_from, transfer.account_to, transfer_type.transfer_type_desc, transfer_status.transfer_status_desc, " +
    "tenmo_user.username, account.balance " +
    "FROM transfer " +
    "JOIN transfer_type ON transfer.transfer_type_id = transfer_type.transfer_type_id " +
    "JOIN transfer_status ON transfer.transfer_status_id = transfer_status.transfer_status_id " +
    "JOIN account ON transfer.account_from = account.account_id " +
    "JOIN tenmo_user ON account.user_id = tenmo_user.user_id " +
    "WHERE " +
    "transfer.account_from IN(Select account_id FROM account WHERE user_id = @user_id) " +
    "OR " +
    "transfer.account_to IN(SELECT account_id FROM account WHERE user_id = @user_id);";

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
            //(transfer.account_from IN(Select account_id FROM account WHERE user_id = @user_id)

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

        //// account stuff?
        //public bool TransferMoney(int accountId, decimal amount)
        //{
        //    string sql = "UPDATE account SET balance = balance - @amount " +
        //        "WHERE account_id = @accountId AND balance >= @amount";
        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection(connectionString))
        //        {
        //            conn.Open();

        //            SqlCommand cmd = new SqlCommand(sql, conn);
        //            cmd.Parameters.AddWithValue("@amount", amount);
        //            cmd.Parameters.AddWithValue("@account_id", accountId);

        //            int numberOfRowsAffected = cmd.ExecuteNonQuery();

        //            if (numberOfRowsAffected == 0)
        //            {
        //                throw new DaoException("Zero rows affected, expected at least one");
        //            }
        //        }
        //        return true;
        //    }
        //    catch (SqlException ex)
        //    {
        //        throw new DaoException("SQL exception occured on TransferMoney", ex);
        //    }
        //}

        //// account stuff?
        //public bool ReceiveMoney(int accountId, decimal amount)
        //{
        //    string sql = "UPDATE account SET balance = balance + @amount " +
        //        "WHERE account_id = @accountId";
        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection(connectionString))
        //        {
        //            conn.Open();

        //            SqlCommand cmd = new SqlCommand(sql, conn);
        //            cmd.Parameters.AddWithValue("@amount", amount);
        //            cmd.Parameters.AddWithValue("@account_id", accountId);

        //            int numberOfRowsAffected = cmd.ExecuteNonQuery();

        //            if (numberOfRowsAffected == 0)
        //            {
        //                throw new DaoException("Zero rows affected, expected at least one");
        //            }
        //        }
        //        return true;
        //    }
        //    catch (SqlException ex)
        //    {
        //        throw new DaoException("SQL exception occured on TransferMoney", ex);
        //    }
        //}

        //public Account UpdateBalance(int accountId, decimal balance)
        //{
        //    Account account = null;
        //    string sql = "UPDATE account SET balance = @balance " +
        //        "WHERE account_id = @accountId";
        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection(connectionString))
        //        {
        //            conn.Open();

        //            SqlCommand cmd = new SqlCommand(sql, conn);
        //            cmd.Parameters.AddWithValue("@balance", balance);
        //            cmd.Parameters.AddWithValue("@account_id", accountId);

        //            int numberOfRowsAffected = cmd.ExecuteNonQuery();

        //            if (numberOfRowsAffected == 0)
        //            {
        //                throw new DaoException("Zero rows affected, expected at least one");
        //            }

        //        }
        //        account = GetAccountById(account_id);

        //    }
        //    catch (SqlException ex)
        //    {
        //        throw new DaoException("SQL exception occured on TransferMoney", ex);
        //    }
        //}


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
