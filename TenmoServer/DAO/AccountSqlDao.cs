﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Dynamic;
using TenmoServer.Exceptions;
using TenmoServer.Models;
using TenmoServer.Security;
using TenmoServer.Security.Models;

namespace TenmoServer.DAO
{

    public class AccountSqlDao : IAccountDao
    {
        private readonly string connectionString;

        public AccountSqlDao(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        //get account by userId

        public Account GetAccountByUserId(int user_id)
        {
            Account account = null;
            string sql = "SELECT * FROM account " +
                "JOIN tenmo_user ON account.user_id = tenmo_user.user_id " +
                "WHERE tenmo_user.user_id = @user_id";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@user_id", user_id);
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        account = MapRowToAccount(reader);
                    }

                }
            }
            catch (SqlException ex)
            {
                throw new DaoException("SqlException on GetAccountByUserId",ex);
            }
            return account;
        }
        public Account GetAccountByAccountId(int account_id)
        {
            Account account = null;
            string sql = "SELECT * FROM account " +
                "WHERE account.account_id = @account_id";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@account_id", account_id);
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        account = MapRowToAccount(reader);
                    }

                }
            }
            catch (SqlException ex)
            {
                throw new DaoException("SqlException on GetAccountByUserId", ex);
            }
            return account;
        }


        //

        public decimal GetBalanceByUserId(int user_id)
        {
            decimal balance = 0;
            string sql = "SELECT balance FROM account " +
                "JOIN tenmo_user ON account.user_id = tenmo_user.user_id " +
                "WHERE tenmo_user.user_id = @user_id";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@user_id", user_id);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        balance = Convert.ToDecimal(reader["balance"]);
                    }

                }
            }
            catch (SqlException ex)
            {
                throw new DaoException("Error in fetching account balance", ex);
            };
            return balance;
        }

        public Account GetAccountById(int account_id)
        {
            Account account = null;
            string sql = "SELECT * FROM account " +
                "WHERE account_id = @account_id";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@account_id", account_id);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        account = MapRowToAccount(reader);
                    }

                }
            }
            catch (SqlException ex)
            {
                throw new DaoException("Error in fetching account balance", ex);
            };
            return account;
        }

        //public List<Account> GetAccounts()
        //{
        //    List<Account> accounts = new List<Account>();

        //    string sql = "SELECT * FROM account";

        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection(connectionString))
        //        {
        //            conn.Open();

        //            SqlCommand cmd = new SqlCommand(sql, conn);
        //            SqlDataReader reader = cmd.ExecuteReader();

        //            while (reader.Read())
        //            {
        //                Account account = MapRowToAccount(reader);
        //                accounts.Add(account);
        //            }
        //        }
        //    }
        //    catch (SqlException ex)
        //    {
        //        throw new DaoException("SQL exception occurred", ex);
        //    }

        //    return accounts;
        //}


        // account stuff?
        public Account UpdateBalance(int accountId, decimal balance)
        {
            Account account = null;
            string sql = "UPDATE account SET balance = @balance " +
                "WHERE account_id = @accountId";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@account_id", accountId);
                    cmd.Parameters.AddWithValue("@balance", balance);

                    int numberOfRowsAffected = cmd.ExecuteNonQuery();

                    if (numberOfRowsAffected == 0)
                    {
                        throw new DaoException("Zero rows affected, expected at least one");
                    }
                }
                account = GetAccountByAccountId(accountId);
                
            }
            catch (SqlException ex)
            {
                throw new DaoException("SQL exception occured on TransferMoney", ex);
            }
            return account;
        }

        // account stuff?
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

        private Account MapRowToAccount(SqlDataReader reader)
        {
            Account account = new Account();
            account.UserId = Convert.ToInt32(reader["user_id"]);
            account.AccountId = Convert.ToInt32(reader["account_id"]);
            account.Balance = Convert.ToDecimal(reader["balance"]);
            return account;
        }

    }
}
