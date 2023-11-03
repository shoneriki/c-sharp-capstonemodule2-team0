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

        public decimal GetBalance()
        {
            Account account = GetUserAccountById(UserId);
            return account.Balance;
        }

        public List<ApiUser> GetUser()
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

        

    }
}
