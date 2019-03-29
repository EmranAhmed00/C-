using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;


namespace Bank_Test
{
    class Program
    {

        static List<Customer> customers = new List<Customer>();
        static List<Account> accounts = new List<Account>();
        static void Main(string[] args)

        {
            
                //File path for streamreader
                String filePath = "bankdata.txt";
            ReadFromFile(filePath);

            Console.WriteLine("***********************\nWelcome to Bank App 1.0\n*****************************");

            Console.WriteLine("Number of Customers:" + customers.Count);

            Console.WriteLine("Number of Accounts:" + accounts.Count);
            decimal balance = 0;
            foreach (var singleaccount in accounts)
            {
                balance += singleaccount.Balance;
            }

            Console.WriteLine("Total Balance:" + balance);

            bool done = false;
            while (done == false)
            {
                // print menu
                Console.WriteLine("Main menu");
                Console.WriteLine("0): Exit and save");
                Console.WriteLine("1): Search customer");
                Console.WriteLine("2): View customer image");
                Console.WriteLine("3): Create customer");
                Console.WriteLine("4): Remove customer");
                Console.WriteLine("5): Create account");
                Console.WriteLine("6): Delete account");
                Console.WriteLine("7): Deposit");
                Console.WriteLine("8): Withdrawal");
                Console.WriteLine("9): Transfer");

                //ask user
                string input = Console.ReadLine();

                //run subroutine
                if (input == "0")
                {
                    done = true;
                }
                else if (input == "1")
                {
                    SearchCustomer();
                }
                else if (input == "2")
                {
                    ViewCustomer();

                }
                else if (input == "3")
                {
                    CreateCustomer();
                }
                else if (input == "4")
                {
                    RemoveCustomer();
                }
                else if (input == "5")
                {
                    CreateAccount();
                }
                else if (input == "6")
                {
                    DeleteAccount();
                }
                else if (input == "7")
                {
                    Deposit();
                }
                else if (input == "8")
                {
                    WithDrawal();
                }
                else if (input == "9")
                {
                    Transfer();
                }
            }
            Console.WriteLine("*Exit and save*");

            Console.WriteLine("Number of Customers:" + customers.Count);
          
            Console.WriteLine("Number of Accounts:" + accounts.Count);
             balance = 0;
            foreach (var singleaccount in accounts)
            {
                balance += singleaccount.Balance;
            }

            Console.WriteLine("Total Balance:" + balance);
            string file = DateTime.Now.ToString("yyyyMMdd-HHmm") + ".txt";
            using (StreamWriter writer = new StreamWriter(file))
            {
             writer.WriteLine(customers.Count);
             foreach (Customer customer in customers)
             {
                 writer.WriteLine($"{customer.CustomerId};{customer.Organization};{customer.Name};" +
                        $"{customer.Address};{customer.City};;{customer.ZipCode};" +
                        $"{customer.Country};");
              }
                  writer.WriteLine(accounts.Count);
                  foreach (Account account in accounts)
                  {
                   writer.WriteLine($"{account.AccountNumber};{account.CustomerId};" +
                        $"{(account.Balance).ToString(CultureInfo.InvariantCulture)}");
                  }
               
            }
        }       


        private static void Transfer()

        {
            Console.WriteLine("Transfer");
            Console.WriteLine("From Account?");
            int accountnumber = int.Parse(Console.ReadLine());
            Console.WriteLine("To Account?");
            int toaccountnumber = int.Parse(Console.ReadLine());
            Console.WriteLine("Amount ?");
            decimal amount = decimal.Parse(Console.ReadLine());

            //Check for sufficient amount on from account 
            //Withdraw amount from , from account .deposit amount to , to account.
            Account account1 = accounts.Where(a => a.AccountNumber == accountnumber).FirstOrDefault();
            Account account2 = accounts.Where(a => a.AccountNumber == toaccountnumber).FirstOrDefault();


            if (account1.Balance < amount)
            {
                Console.WriteLine("Insufficient Funds");
            }
            else
            {
                account1.Withdrawal(amount);
                account2.Deposit(amount);
            }
        }

        private static void WithDrawal()
        {
            Console.WriteLine("Withdrawal");
            Console.WriteLine("Which account number?");
            int toaccountnumber = int.Parse(Console.ReadLine());
            Console.WriteLine("Amount ?");
            decimal amount = decimal.Parse(Console.ReadLine());


            Account account = accounts.Where(a => a.AccountNumber == toaccountnumber).FirstOrDefault();
            if(account.Balance<amount)
            {
                Console.WriteLine("Insufficient funds");
            }
            else
            {
                account.Withdrawal(amount);
            }
           
        }

        private static void Deposit()
        {
            Console.WriteLine("Deposit");
            Console.WriteLine("Account number?");
            int toaccountnumber = int.Parse(Console.ReadLine());
            Console.WriteLine("Amount ?");
            decimal amount = decimal.Parse(Console.ReadLine());

            Account account = accounts.Where(a => a.AccountNumber == toaccountnumber).FirstOrDefault();
            account.Deposit(amount);

        }

        private static void DeleteAccount()
        {
            Console.WriteLine("*Delete Account*");
            Console.WriteLine("Account Number?");
            int id = int.Parse(Console.ReadLine());
        
            Account account = accounts.FirstOrDefault(a => a.AccountNumber == id);

            if (account.Balance == 0)
            {
                accounts.Remove(account);
            }
            else
            {
                Console.WriteLine("Can not Delete, Your account have still money");
            }

        }

        private static void CreateAccount()
        {
            Console.WriteLine(" *Create Account* ");
            // Ask user customer number.
            Console.WriteLine("Ask User Customer Id:");
            int id = int.Parse(Console.ReadLine());
           

            //Find new unique account number
            var accountNumber = (from c in accounts
                                 orderby c.AccountNumber
                                 select c.AccountNumber).Max() + 1;

            //Create new instance of account
            Account account = new Account();

            //Add this account to list of all accounts


            account.Balance = 00;
            account.AccountNumber = accountNumber;
            account.CustomerId = id;
            accounts.Add(account);

        }

        private static void RemoveCustomer()
        {
            Console.WriteLine("Remove Customer");
           
            Console.WriteLine("Customer Number?");
            int id = int.Parse(Console.ReadLine());

            Customer customer = customers.FirstOrDefault(c => c.CustomerId == id);

            var accs = accounts.Where(a => a.CustomerId == id).ToList();
            for (int i = accs.Count - 1; i >= 0; i--)

            {
                if (accs[i].Balance != 0)
                {
                    Console.WriteLine("Can not Remove ,your account have still money.");
                    return;
                }
                  accounts.Remove(accs[i]);
            }
            
            customers.Remove(customer);
        }

        private static void ViewCustomer()
        {
            Console.WriteLine("customer id");
            string input = Console.ReadLine();
          
            var query = (from cus in customers
                        where cus.CustomerId == int.Parse(input)
                        select cus).Single();
          
            Console.WriteLine("ID={0}, Organization={1}, Name={2},Address={3},", query.CustomerId,
                    query.Organization, query.Name, query.Address);

            var accs = accounts.Where(a => a.CustomerId == query.CustomerId);
            decimal sum = 0;
            foreach (Account account in accs)
            {
                Console.WriteLine(account.AccountNumber + ":" + account.Balance);
                sum += account.Balance;
            }
            Console.WriteLine("Total balance" + sum);
        }

        private static void SearchCustomer()
        {
            Console.WriteLine("*Search Customer*");
            Console.WriteLine("Name: or City:");
            string text = Console.ReadLine().ToUpper();
            Console.WriteLine("List with Customer Id and name:");
           

           var customerList = customers.Where(c => c.Name.ToUpper().StartsWith(text)||c.City.ToUpper().StartsWith(text)).ToList();
           if (customerList.Count <= 0)
            {
               customerList = customers.Where(c => c.City.ToUpper(). StartsWith(text)).ToList();
                if (customerList.Count <= 0)
                {
                    Console.WriteLine("Invalid customer:");
                }
                else
                {
                    foreach (Customer customer in customerList)
                    {
                        Console.WriteLine("Id" + customer.CustomerId + "Name" + customer.Name);
                    }

                }
            }

            else
            {
                foreach (Customer customer in customerList)
                {
                    Console.WriteLine("Id" + customer.CustomerId + "Name" + customer.Name);
                }
            }
          

        }

        private static void CreateCustomer()
        {
            Console.WriteLine("* Create Customer *");

            // Ask customer name,organization...  1
            Console.Write("Ask customer name?");
            string name = Console.ReadLine();
            Console.Write("Customer Address");
            string address = Console.ReadLine();
            Console.WriteLine("Organization");
            string organization = Console.ReadLine();
            Console.WriteLine("City");
            string city = Console.ReadLine();
            Console.WriteLine("Zipcode");
            string zipcode = Console.ReadLine();
            Console.WriteLine("Country:");
            string country = Console.ReadLine();

            // Get new unique customer number X

            var customerId = (from c in customers
                              orderby c.CustomerId
                              select c.CustomerId).Max()+1;

            // Create a new instance for customer 2
            Customer customer = new Customer();

            // Set new customer properties 3
            customer.Name = name;
            customer.CustomerId = customerId;
            customer.Address = address;
            customer.Organization = organization;
            customer.City = city;
            customer.ZipCode = zipcode;
            customer.Country = country;

            // Add new customer to List . 4
            customers.Add(customer);

            // Create new Account for customer X
            Account account = new Account();

            //Add this account to list of all accounts


            var accountNumber = (from c in accounts
                              orderby c.AccountNumber
                              select c.AccountNumber).Max() + 1;
           
            account.AccountNumber = accountNumber;
            account.CustomerId = customerId;
            account.Balance = 0;
            accounts.Add(account);

            Console.WriteLine("New Account has been made successfully!");

        }

        //Code to read text file using StreamReader

        public static void ReadFromFile(string filePath)
            {
            StreamReader reader = new StreamReader(filePath);
           
            int count = int.Parse(reader.ReadLine());

            for (int i = 0; i < count; i++)
            {

                string[] colum = reader.ReadLine().Split(';');
                Customer customer = new Customer();
                customer.CustomerId = int.Parse(colum[0]);
                customer.Organization = colum[1];
                customer.Name = colum[2];
                customer.Address = colum[3];
                customer.City = colum[4];
                customer.ZipCode = colum[6];
                customer.Country = colum[7];

                customers.Add(customer);
            }

            int acc = int.Parse(reader.ReadLine());

            for (int i = 0; i < acc; i++)
            {
                string[] row = reader.ReadLine().Split(';');
                Account account = new Account();
                account.AccountNumber = int.Parse(row[0]);
                account.CustomerId = int.Parse(row[1]);
                account.Balance = decimal.Parse(row[2],CultureInfo.InvariantCulture);
               
                accounts.Add(account);
            }
        
            reader.Close();
            }
    }
}



   
