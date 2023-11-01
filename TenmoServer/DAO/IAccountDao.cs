using System.Collections.Generic;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public interface IAccountDao
    {
        Account GetBalanceByUserId(int id);
        Account GetUserByUserId(int user_id);
    }
}
