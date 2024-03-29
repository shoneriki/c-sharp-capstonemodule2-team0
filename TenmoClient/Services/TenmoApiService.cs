﻿using RestSharp;
using RestSharp.Authenticators;
using System.Collections.Generic;
using TenmoClient.Models;
using System.Net.Http;

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

        //public Account GetAccountById(int userId)
        //{
        //    RestRequest request = new RestRequest($"users/{userId}/accounts");
        //    IRestResponse<Account> response = client.Get<Account>(request);

        //    CheckForError(response);
        //    return response.Data;
        //}
        public Account GetAccountByUserId(int userId)
        {
            RestRequest request = new RestRequest($"users/{userId}/accounts");
            IRestResponse<Account> response = client.Get<Account>(request);

            CheckForError(response);
            return response.Data;
        }

        public decimal GetUserBalance()
        {
            Account account = GetAccountByUserId(UserId);
            return account.Balance;
        }
        
        public ApiUser GetUserByAccountId(int accountId)
        {
            RestRequest request = new RestRequest($"users/account/{accountId}/user");
            IRestResponse<ApiUser> response = client.Get<ApiUser>(request);

            CheckForError(response);
            return response.Data;
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

        public Transfer GetAccountById(Account userId)
        {
            RestRequest request = new RestRequest($"users/{userId}/receive_money");
            IRestResponse<Transfer> response = client.Get<Transfer>(request);

            CheckForError(response);
            return response.Data;
        }

        /* 

*/
        /* */
        public TransferStatus GetTransferStatusById(int transferStatusId)
        {
            RestRequest request = new RestRequest("transfer_status");
            IRestResponse<TransferStatus> response = client.Get<TransferStatus>(request);
            CheckForError(response);
            return response.Data;
        }

        public TransferType GetTransferTypeById(int transferTypeId)
        {
            RestRequest request = new RestRequest("transfer_type");
            IRestResponse<TransferType> response = client.Get<TransferType>(request);
            CheckForError(response);
            return response.Data;
        }
        /* */

        public Transfer CreateTransfer(Transfer transfer)
        {
            RestRequest request = new RestRequest("transfer");
            request.AddJsonBody(transfer);
            IRestResponse<Transfer> response = client.Post<Transfer>(request);
            CheckForError(response);
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

        public Account UpdateAccount(Account account)
        {
            RestRequest request = new RestRequest($"accounts/update");
            request.AddJsonBody(account);
            IRestResponse<Account> response = client.Put<Account>(request);

            CheckForError(response);
            return response.Data;
        }

        public List<Transfer> GetTransfersByUserId(int user_id)
        {
            RestRequest request = new RestRequest($"transfer/users/{user_id}/transfers");
            IRestResponse<List<Transfer>> response = client.Get<List<Transfer>>(request);

            CheckForError(response);
            return response.Data;
        }

        public List<Transfer> GetPendingTransfersByuserId(int user_id)
        {
			RestRequest request = new RestRequest($"transfer/users/{user_id}/pending_transfers");
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


    }
}
