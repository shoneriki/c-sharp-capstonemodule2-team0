using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using TenmoServer.DAO;
using TenmoServer.Exceptions;
using TenmoServer.Models;
using TenmoServer.Security;

namespace TenmoServer.Controllers
{
    [Authorize]
    [Route("accounts")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IUserDao userDao;
        private readonly IAccountDao accountDao;

        public AccountsController(IUserDao userDao1, IAccountDao accountDao1)
        {
            userDao = userDao1;
            accountDao = accountDao1;
        }

        //[HttpGet("/accounts")]

        //public List<Account> ListAccounts()
        //{
        //    return accountDao.GetAccounts();
        //}

        [HttpGet("/users/{user_id}/accounts")]
        public ActionResult<Account> GetAccount(int user_id)
        {
            User user = userDao.GetUserById(user_id);
            if (user == null)
            {
                return NotFound();
            }
            return accountDao.GetAccountByUserId(user_id);
        }

        [HttpGet("users/{user_id}/accounts")]
        public decimal GetBalanceById(int user_id)
        {
            decimal balance = accountDao.GetBalanceByUserId(user_id);
            //}
            //else
            //{
            //    return 0.0M;
            //}
            return balance;
        }

        [HttpPut("/balance")]
        public bool UpdateBalanceByGiving(int accountId, decimal amount)
        {
            try
            {
                bool giving = accountDao.TransferMoney(accountId, amount);
                return giving;
            }
            catch (DaoException)
            {
                throw new DaoException("Unacceptable command");
            }
        }

        [HttpPut("/balance")]
        public bool UpdateBalanceByTaking(int accountId, decimal amount)
        {
            try
            {
                bool taking = accountDao.TransferMoney(accountId, amount);
                return taking;
            }
            catch (DaoException)
            {
                throw new DaoException("Unacceptable command");
            }
        }


    }
}
