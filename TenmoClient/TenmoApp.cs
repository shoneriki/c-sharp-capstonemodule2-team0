using System;
using System.Collections.Generic;
using TenmoClient.Models;
using TenmoClient.Services;

namespace TenmoClient
{
    public class TenmoApp
    {
        private readonly TenmoConsoleService console = new TenmoConsoleService();
        private readonly TenmoApiService tenmoApiService;

        public TenmoApp(string apiUrl)
        {
            tenmoApiService = new TenmoApiService(apiUrl);
        }

        public void Run()
        {
            bool keepGoing = true;
            while (keepGoing)
            {
                // The menu changes depending on whether the user is logged in or not
                if (tenmoApiService.IsLoggedIn)
                {
                    keepGoing = RunAuthenticated();
                }
                else // User is not yet logged in
                {
                    keepGoing = RunUnauthenticated();
                }
            }
        }

        private bool RunUnauthenticated()
        {
            console.PrintLoginMenu();
            int menuSelection = console.PromptForInteger("Please choose an option", 0, 2, 1);
            while (true)
            {
                if (menuSelection == 0)
                {
                    return false;   // Exit the main menu loop
                }

                if (menuSelection == 1)
                {
                    // Log in
                    Login();
                    
                    return true;    // Keep the main menu loop going
                }

                if (menuSelection == 2)
                {
                    // Register a new user
                    Register();
                    return true;    // Keep the main menu loop going
                }
                console.PrintError("Invalid selection. Please choose an option.");
                console.Pause();
            }
        }

        private bool RunAuthenticated()
        {
            console.PrintMainMenu(tenmoApiService.Username);
            int menuSelection = console.PromptForInteger("Please choose an option", 0, 6);
            if (menuSelection == 0)
            {
                // Exit the loop
                return false;
            }

            if (menuSelection == 1)
            {
                // View your current balance
                ShowBalance();
            }

            if (menuSelection == 2)
            {
                // View your past transfers
            }

            if (menuSelection == 3)
            {
                // View your pending requests
            }

            if (menuSelection == 4)
            {
                // Send TE bucks
                PromptforTransfer(menuSelection, tenmoApiService.GetUserId(), tenmoApiService.GetUserBalance(), tenmoApiService.GetUsers());
            }

            if (menuSelection == 5)
            {
                // Request TE bucks
                //console.ListOfUsers(tenmoApiService.GetUser());
            }

            if (menuSelection == 6)
            {
                // Log out
                tenmoApiService.Logout();
                console.PrintSuccess("You are now logged out");
            }

            return true;    // Keep the main menu loop going
        }

        private void Login()
        {
            LoginUser loginUser = console.PromptForLogin();
            if (loginUser == null)
            {
                return;
            }

            try
            {
                ApiUser user = tenmoApiService.Login(loginUser);
                if (user == null)
                {
                    console.PrintError("Login failed.");
                }
                else
                {
                    console.PrintSuccess("You are now logged in");
                    
                }
            }
            catch (Exception)
            {
                console.PrintError("Login failed.");
            }
            console.Pause();
        }

        private void Register()
        {
            LoginUser registerUser = console.PromptForLogin();
            if (registerUser == null)
            {
                return;
            }
            try
            {
                bool isRegistered = tenmoApiService.Register(registerUser);
                if (isRegistered)
                {
                    console.PrintSuccess("Registration was successful. Please log in.");
                }
                else
                {
                    console.PrintError("Registration was unsuccessful.");
                }
            }
            catch (Exception)
            {
                console.PrintError("Registration was unsuccessful.");
            }
            console.Pause();
        }

        private void ShowBalance()
        {
            try
            { 
                Console.WriteLine($"Your current account balance is: {tenmoApiService.GetUserBalance()}");
            }
            catch (Exception)
            {
                console.PrintError("Can not display balance at this time. ");
            }
            console.Pause();
        }

        public Transfer PromptforTransfer(int menuSelection, int loginUserId, decimal loginUserBalance, List<ApiUser> apiUsers)
        {
            loginUserId = tenmoApiService.GetUserId();
            ApiUser appUser = new ApiUser();
            Transfer transfer = null;
            console.ListOfUsers(apiUsers);
            try
            {
                if (menuSelection == 4)
                {
                    int userId = console.PromptForInteger("Id of the user you are sending to[0]: ");
                    appUser.UserId = userId;
                    while (loginUserId == appUser.UserId)
                    {
                        Console.WriteLine("You can not send money to yourself\n");
                        userId = console.PromptForInteger("Id of the user you are sending to[0]: ");
                    }
                    decimal amountToSend = console.PromptForDecimal("Enter amount to send: ");
                    while (loginUserBalance < amountToSend)
                    {
                        Console.WriteLine("Insufficient funds.\n");
                        amountToSend = console.PromptForDecimal("Enter amount to send: ");
                    }
                    transfer.Amount = amountToSend;
                    transfer.TypeId = 2;
                    transfer.StatusId = 1;
                    transfer.AccountFrom = loginUserId;
                    transfer.AccountTo = userId;
                    return transfer;
                }


                if (menuSelection == 5)
                {
                    int userId = console.PromptForInteger("Id of the user you are requesting from[0]: ");
                    while (loginUserId == appUser.UserId)
                    {
                        Console.WriteLine("You can not requesting money from yourself\n");
                        userId = console.PromptForInteger("Id of the user you are requesting from[0]: ");
                        appUser.UserId = userId;
                    }
                    decimal amountToRequest = console.PromptForDecimal("Enter amount to request: ");
                    while (loginUserBalance < amountToRequest)
                    {
                        Console.WriteLine("Insufficient funds.\n");
                        amountToRequest = console.PromptForDecimal("Enter amount to send: ");
                    }
                    transfer.Amount = amountToRequest;
                    transfer.TypeId = 2;
                    transfer.StatusId = 1;
                    transfer.AccountFrom = loginUserId;
                    transfer.AccountTo = userId;
                    //return transfer; void this method, add api to end and add transfer object to api sho has created
                }

            }
            catch (Exception)
            {
                Console.WriteLine("error has occured");
            }
            return transfer;

        }
    }
}
