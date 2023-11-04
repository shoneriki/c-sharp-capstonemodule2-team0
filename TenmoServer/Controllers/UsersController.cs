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
    [Route("users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUserDao userDao;

        public UsersController(IUserDao userDao)
        {
            this.userDao = userDao;
        }

        [HttpGet()]

        public IList<User> ListUsers()
        {
            return userDao.GetUsers();
        }

        [HttpGet("{userId}")]

        public ActionResult<User> GetUser(int userId)
        {
            User user = userDao.GetUserById(userId);

            if (user != null)
            { 
                return user;
            }
            else
            {
                return NotFound();
            }

        }

        [HttpGet("{accountid}")]
        public ActionResult<User> GetUserByAccountID(int accountId)
        {
            User user = userDao.GetUserByAccountId(accountId);

            if (user != null)
            {
                return user;
            }
            else
            {
                return NotFound();
            }

        }

    }
}
