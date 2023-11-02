using System.Collections.Generic;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public interface ITransferDao
    {
        // TODO: create transfer
            // need id, from(name!), to (name!), type("Request", "Send"), status("Pending", "Approved", "Rejected")
        
        Transfer CreateTransfer(Transfer transfer);
        Transfer GetTransferById(int transferId);
        List<Transfer> GetTransfersOfUser(int userId);
    }
}
