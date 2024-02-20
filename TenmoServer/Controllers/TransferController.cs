using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TenmoServer.DAO;
using TenmoServer.Exceptions;
using TenmoServer.Models;

namespace TenmoServer.Controllers
{
    [Authorize]
    [Route("transfer")]
    [ApiController]
    public class TransfersController : ControllerBase
    {
        private ITransferDao transferDao;
        private IUserDao userDao;
        //private IAccountDao accountDao;

        public TransfersController(ITransferDao TransferDao1, IUserDao userDao1)
        {
            transferDao = TransferDao1;
            userDao = userDao1;
        }

        [HttpPost()]
        public ActionResult<Transfer> CreatingTransfer(Transfer transfer)
        {
            Transfer added = transferDao.CreateTransfer(transfer);
            Transfer newlyAddedTransfer = transferDao.GetTransferById(added.TransferId);
            return Created($"/transfer/{newlyAddedTransfer.TransferId}", newlyAddedTransfer);
        }

        [HttpGet("{transferid}")]
        public ActionResult<Transfer> Gettransfer(int transferId)
        {
            Transfer transfer = transferDao.GetTransferById(transferId);
            if (transfer != null)
            {
                return transfer;
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("users/{user_id}/transfers")]
        public ActionResult<List<Transfer>> GetTransfersByUserId(int user_id)
        {
            User user = userDao.GetUserById(user_id);
            if (user == null)
            {
                return NotFound();
            }
            return transferDao.GetTransfersOfUser(user_id);
        }


        [HttpPut("{transferId}")]
        public ActionResult<Transfer> UpdatesTransfer(Transfer transfer)
        {
            try
            {
                Transfer result = transferDao.UpdateTransfer(transfer);
                return result;
            }
            catch (DaoException)
            {
                return NotFound();
            }
        }

        //[HttpGet("/transfer/user/{userId}/pending")]
        //public ActionResult<List<Transfer>> PendingTransfers(int user_id)
        //{
        //    return transferDao.GetPendingTransfers(user_id);
        //}
    }
}
