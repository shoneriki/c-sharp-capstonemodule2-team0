using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TenmoServer.DAO;
using TenmoServer.Exceptions;
using TenmoServer.Models;
using TenmoServer.Security;

namespace TenmoServer.Controllers 
{
    [Route("users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUserDao userDao;

        public UsersController(IUserDao userDao)
        {
            this.userDao = userDao;
        }

        [HttpGet("{id}")]

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

        
    }
}
