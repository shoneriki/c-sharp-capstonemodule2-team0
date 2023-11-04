using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Xml.Linq;
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

        //TODO: figure out how to compare the ID's and then when everything is complete add all the information into the transfer object to return and add that object into the  


        // Add application-specific UI methods here...
        public void PromptforTransfer(int menuSelection, TenmoApiService tenmo)
        {

            ListOfUsers(tenmo);
            try
            {
                if (menuSelection == 4)
                {
                    int userId = PromptForInteger("Id of the user you are sending to[0]");
                    while (tenmo.UserId == userId)
                    {
                        Console.WriteLine("You can not send money to yourself\n");
                        userId = PromptForInteger("Id of the user you are sending to[0]");
                    }
                    decimal amountToSend = PromptForDecimal("Enter amount to send");
                    while (amountToSend <= 0)
                    {
                        Console.WriteLine("Can't send zero or negative amount\n");
                        amountToSend = PromptForDecimal("Enter amount to send");
                    }
                    if (amountToSend > tenmo.GetUserBalance())
                    {
                        Console.WriteLine("Insufficient funds\n");
                        Pause();
                    }
                    Transfer transfer = new Transfer
                    {
                        TransferTypeId = 2,
                        TransferStatusId = 2,
                        AccountTo = tenmo.GetAccountByUserId(tenmo.UserId).AccountId,
                        AccountFrom = tenmo.GetAccountByUserId(userId).AccountId,
                        Amount = amountToSend
                    };
                    tenmo.CreateTransfer(transfer);
                    Console.WriteLine("Tranfer request sent!");

                }
                

                if (menuSelection == 5)
                {
                    int userId = PromptForInteger("Id of the user you are requesting from[0]");
                    while (userId == tenmo.UserId)
                    {
                        Console.WriteLine("You can not requesting money from yourself\n");
                        userId = PromptForInteger("Id of the user you are requesting from[0]");
                    }
                    decimal amountToRequest = PromptForDecimal("Enter amount to request");
                    while (amountToRequest <= 0)
                    {
                        Console.WriteLine("Can't request zero or negative amount\n");
                        amountToRequest = PromptForDecimal("Enter amount to send");
                    }
                    Transfer transfer = new Transfer
                    {
                        TransferTypeId = 1,
                        TransferStatusId = 1,
                        AccountFrom = tenmo.GetAccountByUserId(tenmo.UserId).AccountId,
                        AccountTo = tenmo.GetAccountByUserId(userId).AccountId,
                        Amount = amountToRequest
                        
                    };
                    tenmo.CreateTransfer(transfer);
                    Console.WriteLine("Request was sent!");
                }

            }
            catch (Exception)
            {
                Console.WriteLine("error has occured");
            }



        }

        //publi

        //TODO: add logic for transfer. remember to add Api request for both. when finished add into app.
        //public Transfer PromptForSendTransfer(int choice, TenmoApiService tenmo)
        //{
        //    tenmo.GetUserId
        //    Transfer holder = new Transfer();
        //    return holder;
        //}

        public void PromptToViewPendingTranfers(TenmoApiService tenmo)
        {
            List<Transfer> transfers = tenmo.GetTransfersByUserId(tenmo.UserId);
            Account loginUser = tenmo.GetAccountByUserId(tenmo.UserId);

            foreach (Transfer element in transfers)
            {
                //This goes through the list and only pulls pending request and only pulls them when the request is sent to the user.
                if(element.TransferStatusId == 1 && element.AccountTo == loginUser.AccountId)
                {                  
                    //this prints the usernames.
                    string username = tenmo.GetUserByAccountId(element.AccountFrom).Username;
                    
                    Console.WriteLine($"{element.TransferId} / {username} / {element.Amount}");
                }

            }
            foreach (Transfer element in transfers)
            {
                //This goes through the list and only pulls pending request and only pulls them when the request is sent to the user.
                if (element.TransferStatusId == 1 && element.AccountTo == loginUser.AccountId)
                {
                    //this allows user to pick from available requests again and make a decision.
                    //TODO: finish logic on updating franfer

                    //prompting for the user they want to accept/reject/do nothing
                    int userSelection = PromptForInteger("Please enter transfer ID to approve/reject (0 to cancel)");
                    while (userSelection < 0 & userSelection != element.TransferId)
                    {
                        Console.WriteLine("Invalid selection\n");
                        userSelection = PromptForInteger("Please enter transfer ID to approve/reject (0 to cancel)");
                    }
                    if (userSelection == 0)
                    {
                        return;
                    }
                    else if (userSelection == element.TransferId)
                    {
                        int userInput = PromptForInteger("Select (1)Approve / (2)Reject / (0)Cancel");

                    }
                }

            }
            Pause();
        }

        // TODO: create prompts for viewing past transfers and pending transfer
        // TODO: Prompt to accept, deny, or wait on transfer request
        public void ListOfUsers(TenmoApiService tenmo)
        {
            List<ApiUser> users = tenmo.GetUsers();
            foreach (ApiUser element in users)
            {
                if (element.UserId != tenmo.UserId)
                {
                    Console.WriteLine($"{element.UserId} / {element.Username}");
                }

            }
        }
    }
}
