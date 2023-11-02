using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TenmoServer.DAO;
using TenmoServer.Models;

namespace TenmoServer.Controllers
{
    [Authorize]
    [Route("transfer")]
    [ApiController]
    public class TransfersController : ControllerBase
    {
        private ITransferDao TransferDao;
        private IUserDao userDao;

        public TransfersController(ITransferDao TransferDao1, IUserDao userDao1)
        {
            TransferDao = TransferDao1;
            userDao = userDao1;
        }

        [HttpPost()]
        public ActionResult<Transfer> CreatingTransfer(Transfer transfer)
        {
            Transfer added = TransferDao.CreateTransfer(transfer);
            return Created($"/transfer/{added.TransferId}", added);
        }

        [HttpGet("{transferid}")]
        public ActionResult<Transfer> Gettransfer(int transferId)
        {
            Transfer transfer = TransferDao.GetTransferById(transferId);
            if(transfer != null)
            {
                return transfer;
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("users")]
        public IList<User> GetUsersForTransfer()
        {      
            return userDao.GetUsers();
        }

        [HttpGet("users/{user_id}")]
        public ActionResult<List<Transfer>> ListTransfersOfUser(int user_id)
        {
            User user = userDao.GetUserById(user_id);
            if (user == null)
            {
                return NotFound();
            }
            return TransferDao.GetTransfersOfUser(user_id);
        }

    }
}
