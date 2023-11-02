using System.Collections.Generic;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public interface IAccountDao
    {
        decimal GetBalanceByUserId(int id);
        Account GetAccountById(int account_id);
        bool TransferMoney(int accountId, decimal amount);
        bool ReceiveMoney(int accountId, decimal amount);
        //public IList<Account> GetAccounts();

        Account GetAccountByUserId(int user_id);

        Account GetAccountByAccountId(int account_id);
    }
}
