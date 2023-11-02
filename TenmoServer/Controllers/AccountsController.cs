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

        [HttpGet("/users/{userId}/accounts")]

        public ActionResult<List<Account>> ListAccountsByUser(int userId)
        {
            User user = userDao.GetUserById(userId);
            if (user == null)
            {
                return NotFound();
            }
            accountDao.GetBalanceByUserId(userId);
        }





    }
}
