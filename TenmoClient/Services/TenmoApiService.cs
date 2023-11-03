using RestSharp;
using RestSharp.Authenticators;
using System.Collections.Generic;
using TenmoClient.Models;

namespace TenmoClient.Services
{
    public class TenmoApiService : AuthenticatedApiService
    {
        public readonly string ApiUrl;

        public TenmoApiService(string apiUrl) : base(apiUrl) { }

        // Add methods to call api here...

        //public static IRestClient client = null;
        /* continue to add api for transfers
         * request transfer detail
         * request transfer view
         * request to see list of users
         */

        public List<Account> GetAccounts(Account userId)
        {
            RestRequest request = new RestRequest($"users/{userId}/accounts");
            IRestResponse<List<Account>> response = client.Get<List<Account>>(request);

            CheckForError(response);
            return response.Data;
        }
        public Account GetUserAccountById(int userId)
        {
            RestRequest request = new RestRequest($"users/{userId}/accounts");
            IRestResponse<Account> response = client.Get<Account>(request);

            CheckForError(response);
            return response.Data;
        }

        public decimal GetUserBalance()
        {
            Account account = GetUserAccountById(UserId);
            return account.Balance;
        }
        
        public int GetUserId()
        {
            
            ApiUser apiUser = new ApiUser();
            apiUser.UserId = UserId;
            return apiUser.UserId;
        }

        public List<ApiUser> GetUsers()
        {
            RestRequest request = new RestRequest("users");
            IRestResponse<List<ApiUser>> response = client.Get<List<ApiUser>>(request);

            CheckForError(response);
            return response.Data;
        }

        //public List<ApiUser> GetUsers()
        //{
        //    List<ApiUser> userList = GetAccountListForTransfer(userList.UserId);
        //    return userList;
        //}

        public List<Transfer> GetAccountsRequest(Account userId)
        {
            RestRequest request = new RestRequest($"users/{userId}/recieve_money");
            IRestResponse<List<Transfer>> response = client.Get<List<Transfer>>(request);

            CheckForError(response);
            return response.Data;
        }

        /* 

*/

        public Transfer CreateTransfer(Transfer transfer)
        {
            RestRequest request = new RestRequest("transfer/create");
            request.AddJsonBody(transfer);
            IRestResponse<Transfer> response = client.Post<Transfer>(request);

            CheckForError(response);

            //if (response.IsSuccessful)
            //{
            //    decimal updatedBalance = GetBalance();
            //}
            return response.Data;
        }

        public Transfer UpdateTransfer(Transfer transfer)
        {
            RestRequest request = new RestRequest($"transfer/update");
            request.AddJsonBody(transfer);
            IRestResponse<Transfer> response = client.Put<Transfer>(request);

            CheckForError(response);
            return response.Data;
        }

        public List<Transfer> GetTransferListOfUser(int user_id)
        {
            RestRequest request = new RestRequest($"transfer/users/{user_id}");
            IRestResponse<List<Transfer>> response = client.Get<List<Transfer>>(request);

            CheckForError(response);
            return response.Data;
        }

        public Transfer GetTransferDetails(int transferId)
        {
            RestRequest request = new RestRequest($"transfer/{transferId}");
            IRestResponse<Transfer> response = client.Get<Transfer>(request);

            CheckForError(response);
            return response.Data;
        }

        //public List<Transfer> GetPendingTransfersForUser(int userId)
        //{
        //    RestRequest request = new RestRequest($"transfer/users/{userId}/pending");
        //    IRestResponse<List<Transfer>> response = client.Get<List<Transfer>>(request);

        //    CheckForError(response);
        //    return response.Data;
        //}


        //public TransferType SendMoney(TransferType transferType)
        //{
        //    RestRequest request = new RestRequest($"transfer/{transferType.TypeDesc}");
        //    IRestResponse<TransferType> response = client.Put<TransferType>(request);
        //    CheckForError(response);
        //    return response.Data;

        //}

        //public TransferType RequestMoney(TransferType transferType)
        //{
        //    RestRequest request = new RestRequest($"transfer/{transferType.TypeDesc}");
        //    IRestResponse<TransferType> response = client.Put<TransferType>(request);
        //    CheckForError(response);
        //    return response.Data;

        //}

        //public List<Transfer> GetPendingTransfers(int transfer_status_id)
        //{
        //    List<Transfer> transfers = new List<Transfer>();
        //    string sql = "SELECT * FROM transfer " +
        //        "WHERE transfer_status_id = @transfer_status_id";

        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection(connectionString))
        //        {
        //            conn.Open();
        //            SqlCommand cmd = new SqlCommand(sql, conn);
        //            cmd.Parameters.AddWithValue("@transfer_status_id", transfer_status_id);
        //            SqlDataReader reader = cmd.ExecuteReader();

        //            while (reader.Read())
        //            {
        //                Transfer transfer = MapRowToTransfer(reader);
        //                transfers.Add(transfer);
        //            }
        //        }
        //    }
        //    catch (SqlException ex)
        //    {
        //        throw new DaoException("SQL exception on GetPendingTransfers", ex); ;
        //    }

        //    return transfers;
        //}


        /* 

         */


    }
}
