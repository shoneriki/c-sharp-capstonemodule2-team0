using System.Collections.Generic;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public interface IAccountDao
    {
        decimal GetBalanceByUserId(int id);
<<<<<<< HEAD
        
=======
        bool TransferMoney(int accountId, decimal amount);
        bool ReceiveMoney(int accountId, decimal amount);
>>>>>>> ef1bab75a3a8621a4cb71eba9df3f32fc72026c0
    }
}
