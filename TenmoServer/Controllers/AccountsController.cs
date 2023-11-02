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
    public class AccountController : ControllerBase
    {
        private readonly IUserDao userDao;
        private readonly IAccountDao accountDao;

        public AccountController(IUserDao userDao, IAccountDao accountDao)
        {
            this.userDao = userDao;
            this.accountDao = accountDao;
        }

<<<<<<< HEAD
        //[HttpGet("/user/{userId}/account")]

        //public ActionResult<List<Account>> ListAccountsByUserId(int id)
        //{
        //    User user = userDao.GetUserById(id);
        //    if(user == null)
        //    {
        //        return NotFound();
        //    }
        //    return accountDao.GetAccountById(id);
        //}

        [HttpGet("{account_id}")]
        public ActionResult<Account> GetAccount(int account_id)
        {
            Account account = accountDao.GetAccountById(account_id);
            if(account != null)
            {
                return account;
            }
            else
            {
                return NotFound();
            }
        }
=======
        //[HttpGet("/users/{userId}/accounts")]

        //public ActionResult<List<Account>> ListAccountsByUser(int userId)
        //{
        //    User user = userDao.GetUserById(userId);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }
        //    accountDao.GetBalanceByUserId(userId);
        //}
>>>>>>> 7ba8ad4165d838718fc1191bf3cd2c2c1750a8c1

        [HttpGet("/balance")]
        public decimal GetBalanceById(int id)
        {
            decimal balance = accountDao.GetBalanceByUserId(id);
            if(balance != 0)
            {
                return balance;
            }
            else
            {
                return 0.0M;
            }
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
