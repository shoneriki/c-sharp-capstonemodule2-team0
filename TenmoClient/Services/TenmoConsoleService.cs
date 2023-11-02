﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using TenmoClient.Models;

namespace TenmoClient.Services
{
    public class TenmoConsoleService : ConsoleService
    {

        /************************************************************
            Print methods
        ************************************************************/
        public void PrintLoginMenu()
        {
            Console.Clear();
            Console.WriteLine("");
            Console.WriteLine("Welcome to Tenmo!");
            Console.WriteLine("1: Login");
            Console.WriteLine("2: Register");
            Console.WriteLine("0: Exit");
            Console.WriteLine("---------");
        }

        public void PrintMainMenu(string username)
        {
            Console.Clear();
            Console.WriteLine("");
            Console.WriteLine($"Hello, {username}!");
            Console.WriteLine("1: View your current balance");
            Console.WriteLine("2: View your past transfers");
            Console.WriteLine("3: View your pending requests");
            Console.WriteLine("4: Send TE bucks");
            Console.WriteLine("5: Request TE bucks");
            Console.WriteLine("6: Log out");
            Console.WriteLine("0: Exit");
            Console.WriteLine("---------");
        }
        public LoginUser PromptForLogin()
        {
            string username = PromptForString("User name");
            if (String.IsNullOrWhiteSpace(username))
            {
                return null;
            }
            string password = PromptForHiddenString("Password");

            LoginUser loginUser = new LoginUser
            {
                Username = username,
                Password = password
            };
            return loginUser;
        }

        //create promptfortransfer including logic for send and request include listOfUser

        // Add application-specific UI methods here...
        public void ListOfUsers(List<ApiUser> apiUsers)
        {
            
            foreach (ApiUser element in apiUsers)
            {

                Console.WriteLine($"{element.UserId} / {element.Username}");

            }
            Pause();
        }

        //publi

        //TODO: add logic for transfer. remember to add Api request for both. when finished add into app.
        public Transfer PromptForSendTransfer()
        {
            Transfer holder = new Transfer();
            return holder;
        }

        public Transfer PromptForRequestTransfer()
        {
            Transfer holder = new Transfer();
            return holder;
        }

        // TODO: create prompts for viewing past transfers and pending transfer
        // TODO: Prompt to accept, deny, or wait on transfer request
    }
}
