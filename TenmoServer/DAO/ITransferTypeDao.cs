using System.Collections.Generic;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public interface ITransferTypeDao
    {
        // TODO: display all of the transfer types
        List<TransferType> GetTransferTypes();
        // TODO: get transfer type by id
        TransferType GetTransferTypeById(int statusId);

    }
}
