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

        //public ActionResult<List<Account>> ListAccountsByUser(int userId)
        //{
        //    User user = userDao.GetUserById(userId);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }
        //    accountDao.GetBalanceByUserId(userId);
        //}


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
