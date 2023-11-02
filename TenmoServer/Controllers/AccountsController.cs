using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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

        //[HttpGet("{userId}")]

        //public ActionResult<Account> ListAccountsByBalance(int id)
        //{
        //    Account account = accountDao.GetBalanceByUserId(id);
        //    if (account != null)
        //    {
        //        return account;
        //    }
        //    else
        //    {
        //        return NotFound();
        //    }
            
        //}





    }
}
