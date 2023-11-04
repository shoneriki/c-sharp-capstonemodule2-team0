using System.Collections.Generic;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public interface IAccountDao
    {
        decimal GetBalanceByUserId(int id);
        Account GetAccountById(int account_id);
        Account UpdateBalance(int accountId, decimal balance);
        //bool ReceiveMoney(int accountId, decimal amount);
        //public IList<Account> GetAccounts();

        Account GetAccountByUserId(int user_id);

        Account GetAccountByAccountId(int account_id);
    }
}
