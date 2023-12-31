﻿using System.Collections.Generic;
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

        Transfer UpdateTransfer(Transfer transfer);

        List<Transfer> GetPendingTransfers(int user_id);

        //Account UpdateBalance(int accountId, decimal balance);

        // TODO: TransferMoney (give money aka lose money and balance decreases)
        //public decimal TransferMoneyToUser(decimal money);
        //bool TransferMoney(int accountId, decimal amount);


    }
}
