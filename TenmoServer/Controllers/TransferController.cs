using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
<<<<<<< HEAD
using System.Collections.Generic;
using TenmoServer.DAO;
using TenmoServer.Models;
=======
>>>>>>> ea6eba8d77445755e06739b926057b6a4501780e

namespace TenmoServer.Controllers
{
    [Authorize]
    [Route("transfer")]
    [ApiController]
<<<<<<< HEAD
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

        [HttpGet("/user/{user_id}/transfers")]
        public ActionResult<List<Transfer>> GettrasnferByUser(int userId)
        {
            User user = userDao.GetUserById(userId);
            if(user == null)
            {
                return NotFound();
            }
            return TransferDao.GetTransfersOfUser(userId);
        }
=======
    public class TransferController : ControllerBase
    {
        //private ITransferDao transferDao;
>>>>>>> ea6eba8d77445755e06739b926057b6a4501780e
    }
}
