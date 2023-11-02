using System.Collections.Generic;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public interface ITransferStatusDao
    {
        // TODO: display all of the transfer statuses
        List<TransferStatus> GetTransferStatuses();
        // TODO: get transfer status by id
        TransferStatus GetTransferStatusById(int statusId);
    }
}
