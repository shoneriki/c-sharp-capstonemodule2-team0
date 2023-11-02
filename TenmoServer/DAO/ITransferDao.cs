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

        // TODO: TransferMoney (give money aka lose money and balance decreases)
        //public decimal TransferMoneyToUser(decimal money);
        //bool TransferMoney(int accountId, decimal amount);
<<<<<<< HEAD
        //// TODO: ReceiveMoney (get money aka receive money and balance increases)
        ////public decimal GettingMoneyFromUser(decimal money);
=======
        // TODO: ReceiveMoney (get money aka receive money and balance increases)
        //public decimal GettingMoneyFromUser(decimal money);
>>>>>>> 7ba8ad4165d838718fc1191bf3cd2c2c1750a8c1
        //bool ReceiveMoney(int accountId, decimal amount);
    }
}
