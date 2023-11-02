﻿using RestSharp;
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
        private static Account user = new Account();

        public int Id
        {
            get
            {
                return (user == null) ? 0 : user.Id;
            }
        }

        public int AccountId
        {
            get
            {
                return (user == null) ? 0 : user.AccountId;
            }
        }

        public decimal Balance
        {
            get
            {
                return (user == null) ? 0 : user.Balance;
            }
        }

        // tenmoApiService

        public Account GetBalance(Account userId)
        {
            RestRequest request = new RestRequest($"users/{userId}/balance");
            IRestResponse<Account> response = client.Get<Account>(request);

            CheckForError(response);
            return response.Data;
        }

        /* continue to add api for transfers
         * request transfer detail
         * request transfer view
         * request to see list of users
         */

        //public List<Account> GetAccounts(Account userId)
        //{
        //    RestRequest request = new RestRequest($"users/{userId}/");
        //    IRestResponse<List<Account>> response = client.Get<List<Account>>(request);

        //    CheckForError(response);
        //    return response.Data;
        //}

        public List<Transfer> GetTranferSend(Account userId)
        {
            RestRequest request = new RestRequest($"users/{userId}/send_money");
            IRestResponse<List<Transfer>> response = client.Get<List<Transfer>>(request);

            CheckForError(response);
            return response.Data;
        }

        public List<Transfer> GetTranferRequest(Account userId)
        {
            RestRequest request = new RestRequest($"users/{userId}/recieve_money");
            IRestResponse<List<Transfer>> response = client.Get<List<Transfer>>(request);

            CheckForError(response);
            return response.Data;
        }


    }
}
