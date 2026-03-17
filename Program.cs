using System;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Runtime.Intrinsics.Arm;
using System.Security.Principal;
class Program
{
    class Bank
    {
        public List<string> accountList = new List<string>(); // AC no, name 
        public List<string> info = new List<string>(); // AC no, password , balance
        public List<string> transactionList = new List<string>(); // AC no, transaction
        public Bank(List<string> ac, List<string> info, List<string> trans) // add all account list and all user information
        {
            accountList = ac;
            this.info = info;
            transactionList = trans;
        }

        public void NewAccount(string details, string information, string trans) // data store in text file 
        {
            accountList.Add(details);
            info.Add(information);
            transactionList.Add(trans);
            File.WriteAllLines("AccountList.txt", accountList);
            File.WriteAllLines("Personalinfo.txt", info);
            File.WriteAllLines("Transaction.txt", transactionList);
        }
        public bool CheckAccount(string name) // check account exist or not
        {
            for (int i = 0; i < accountList.Count; i++)
            {
                string[] s = accountList[i].Split(",");
                if (s[1] == name)
                {
                    return true;
                }
            }
            return false;
        }
    }
    class User
    {
        public int acNo;
        public string name;
        private string password;
        private int balance;
        public int GetBalance()
        {
            return balance;
        }
        public bool VerifyAccount(int acNo, string pass, Bank bank) // store user data
        {
            if (acNo > 1000 && acNo <= 1000 + bank.accountList.Count)
            {
                string[] s = bank.info[acNo - 1001].Split(",");
                string[] n = bank.accountList[acNo - 1001].Split(",");
                if (s[1] == pass)
                {
                    this.acNo = acNo;
                    password = pass;
                    name = n[1];
                    balance = int.Parse(s[2]);
                }
                else
                {
                    System.Console.WriteLine("Incorrect Password.\nPlease try again.");
                    return false;
                }
            }
            else
            {
                System.Console.WriteLine("Your account was not found . Please create a new account .");
                return false;
            }
            return true;
        }
        public void Deposit(int money, Bank bank) // diposit function 
        {
            balance += money;
            string[] s = bank.info[acNo - 1001].Split(",");
            s[2] = balance.ToString();

            bank.info[acNo - 1001] = s[0] + "," + s[1] + "," + s[2];
            bank.transactionList[acNo - 1001] += "," + "Cash In : " + money + " Taka";
            File.WriteAllLines("Transaction.txt", bank.transactionList);
            File.WriteAllLines("Personalinfo.txt", bank.info);
            System.Console.WriteLine($"{money} Taka Deposit Successfully ! ");
        }
        public bool Withdraw(int money, Bank bank) // withdraw function
        {
            if (money > balance)
            {
                System.Console.WriteLine("You have not sufficient balance .");

                return false;
            }
            else
            {
                balance -= money;
                string[] s = bank.info[acNo - 1001].Split(",");
                s[2] = balance.ToString();
                bank.info[acNo - 1001] = s[0] + "," + s[1] + "," + s[2];
                bank.transactionList[acNo - 1001] += "," + "Cash Out : " + money + " Taka";
                File.WriteAllLines("Transaction.txt", bank.transactionList);
                File.WriteAllLines("Personalinfo.txt", bank.info);
                return true;
            }
        }
        private bool CheckPass(string oldPass)
        {
            if (oldPass == password)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool CheckAc(int ac, Bank bank)
        {
            if (ac > 1000 && ac <= 1000 + bank.accountList.Count)
            {
                return true;
            }
            System.Console.WriteLine($"{ac} account was not found our bank");
            return false;
        }
        public void Transfer(int acno, int money, Bank bank) // transfer function
        {
            string[] s = bank.info[acno - 1001].Split(",");
            s[2] = (int.Parse(s[2]) + money).ToString();
            bank.info[acno - 1001] = s[0] + "," + s[1] + "," + s[2];
            bank.transactionList[acno - 1001] += "," + "Cash In : " + money + " Taka";
            File.WriteAllLines("Transaction.txt", bank.transactionList);
            File.WriteAllLines("Personalinfo.txt", bank.info);
        }
        public void ShowTransaction(Bank bank) // show transaction history
        {
            System.Console.WriteLine("Your Transaction History :");
            string[] s = bank.transactionList[acNo - 1001].Split(",");
            for (int i = 1; i < s.Length; i++)
            {
                System.Console.WriteLine(s[i]);
            }
        }
        public void ChangePassword(Bank bank)
        {
            System.Console.WriteLine("Enter Old Password:");
            string oldPass = Console.ReadLine();
            if (CheckPass(oldPass))
            {
                System.Console.WriteLine("Enter new Password(must 8 character):");
                string newPass = Console.ReadLine();
                if (newPass.Length < 8)
                {
                    Console.Clear();
                    System.Console.WriteLine("Too Short Password.\nPlease enter Long password.");
                }
                else
                {
                    Console.Clear();
                    password = newPass;
                    string[] s = bank.info[acNo - 1001].Split(",");
                    s[1] = password;
                    bank.info[acNo - 1001] = s[0] + "," + s[1] + "," + s[2];
                    File.WriteAllLines("Personalinfo.txt", bank.info);
                    System.Console.WriteLine("Password Change Successfully !!");
                }
            }
            else
            {
                Console.Clear();
                System.Console.WriteLine("Incorrect Old Password.");
            }
        }
    }
    static void Welcome()
    {
        System.Console.WriteLine("------------------------------------------------------------");
        System.Console.WriteLine("--------------------Welcome To *** Bank---------------------");
        System.Console.WriteLine("------------------------------------------------------------");
    }
    static void AccountCreate(Bank bank) //= this function create new account
    {
        System.Console.WriteLine("Enter Your Account Name:");
        string name = Console.ReadLine();
        if (bank.CheckAccount(name))
        {
            System.Console.WriteLine("Your have already an account .Please Log in this account.");
        }
        else
        {
            string pass;
            int dps;
            while (true)
            {
                System.Console.WriteLine("Enter Password (must  8 length character)");
                pass = Console.ReadLine();

                if (pass.Length >= 8)
                {
                    break;
                }
                System.Console.WriteLine("Too Short Password.\nPlease enter Long password.");
                Console.Clear();
            }
            while (true)
            {
                System.Console.WriteLine("First diposit ammount (minimum 500 taka):");
                dps = int.Parse(Console.ReadLine());
                if (dps >= 500)
                {
                    break;
                }
                System.Console.WriteLine("Ammount is too low .\nPlease Try again.");
                Console.Clear();
            }
            Console.Clear();
            int acno = bank.accountList.Count() + 1001;
            string account = acno.ToString() + "," + name;
            string info = acno.ToString() + "," + pass + "," + dps.ToString();
            string trans = acno.ToString() + "," + "Cash In : " + dps.ToString() + " Taka";
            bank.NewAccount(account, info, trans);
            System.Console.WriteLine("Account Create Successfully !");
            System.Console.WriteLine();
            System.Console.WriteLine();
        }
    }
    static void MenuList()
    {
        System.Console.WriteLine();
        System.Console.WriteLine();
        System.Console.WriteLine("=========================================");
        System.Console.WriteLine("============== ACCOUNT MENU =============");
        System.Console.WriteLine("=========================================");
        System.Console.WriteLine("1.Check Balance");
        System.Console.WriteLine("2.Deposit Money");
        System.Console.WriteLine("3.Withdraw Money");
        System.Console.WriteLine("4.Transfer Money");
        System.Console.WriteLine("5.Transaction History");
        System.Console.WriteLine("6.Change Password");
        System.Console.WriteLine("7.Logout");
        System.Console.WriteLine("Choice Option :");
    }

    static void Main()
    {
        Welcome();

        List<string> acList = new List<string>(File.ReadAllLines("AccountList.txt")); // previous record file add bank system
        List<string> infoList = new List<string>(File.ReadAllLines("Personalinfo.txt"));
        List<string> transList = new List<string>(File.ReadAllLines("Transaction.txt"));
        Bank bank = new Bank(acList, infoList, transList); // create a new bank system
        while (true)
        {
            System.Console.WriteLine();
            System.Console.WriteLine();
            System.Console.WriteLine("1. Create Account");
            System.Console.WriteLine("2. Log In");
            System.Console.WriteLine("3. Exit");
            System.Console.Write("Choice option:");
            int choice;
            if (!int.TryParse(Console.ReadLine(), out choice))
            {
                Console.Clear();
                System.Console.WriteLine("Invalid Input !!");
                continue;
            }
            Console.Clear();
            System.Console.WriteLine();
            if (choice == 1)
            {
                AccountCreate(bank);
            }
            else if (choice == 2)
            {
                System.Console.WriteLine("Enter Your AC No :");
                int acNo;
                if (!int.TryParse(Console.ReadLine(), out acNo))
                {
                    Console.Clear();
                    System.Console.WriteLine("Invalid Input !!");
                    continue;
                }
                System.Console.WriteLine("Password :");
                string pass = Console.ReadLine();
                Console.Clear();
                User user = new User();
                if (!user.VerifyAccount(acNo, pass, bank)) continue;

                System.Console.WriteLine("Log In successfully.");
                System.Console.WriteLine("---------------------------------------------");
                System.Console.WriteLine($"------------Welcome {user.name}-------------");
                System.Console.WriteLine("---------------------------------------------");
                System.Console.WriteLine();
                System.Console.WriteLine();

                while (true)
                {
                    MenuList();
                    int menu;
                    if (!int.TryParse(Console.ReadLine(), out menu))
                    {
                        Console.Clear();
                        System.Console.WriteLine("Invalid Input !!");
                        continue;
                    }
                    Console.Clear();
                    if (menu == 1)
                    {
                        System.Console.WriteLine($"Balance :{user.GetBalance()} Taka");
                    }
                    else if (menu == 2)
                    {
                        System.Console.WriteLine("Enter Your ammount :");
                        int money;
                        if (!int.TryParse(Console.ReadLine(), out money))
                        {
                            Console.Clear();
                            System.Console.WriteLine("Invalid Input !!");
                            continue;
                        }
                        Console.Clear();
                        user.Deposit(money, bank);
                    }
                    else if (menu == 3)
                    {
                        System.Console.WriteLine("Enter Your Withdraw account :");
                        int money;
                        if (!int.TryParse(Console.ReadLine(), out money))
                        {
                            Console.Clear();
                            System.Console.WriteLine("Invalid Input !!");
                            continue;
                        }
                        Console.Clear();
                        if (!user.Withdraw(money, bank))
                        {
                            System.Console.WriteLine("Withdraw unsuccessful.");
                            continue;
                        }

                        System.Console.WriteLine($"{money} Taka Withdaw Successfully ! Please take your money.");
                    }
                    else if (menu == 4)
                    {
                        System.Console.WriteLine("Enter Your receiver AC No. :");
                        int acno;
                        if (!int.TryParse(Console.ReadLine(), out acno))
                        {
                            Console.Clear();
                            System.Console.WriteLine("Invalid Input !!");
                            continue;
                        }
                        System.Console.WriteLine("Enter Ammount :");
                        int money;
                        if (!int.TryParse(Console.ReadLine(), out money))
                        {
                            Console.Clear();
                            System.Console.WriteLine("Invalid Input !!");
                            continue;
                        }
                        Console.Clear();
                        if (!user.CheckAc(acno, bank)) continue;

                        if (!user.Withdraw(money, bank)) continue;

                        user.Transfer(acno, money, bank);
                        System.Console.WriteLine($"{money} Taka Transfer Successfully. From account : {user.acNo} To account : {acno} ");
                    }
                    else if (menu == 5)
                    {
                        user.ShowTransaction(bank);
                    }
                    else if (menu == 6)
                    {
                        user.ChangePassword(bank);
                    }
                    else if (menu == 7)
                    {
                        System.Console.WriteLine("Log out successfully !");
                        break;
                    }
                    else
                    {
                        System.Console.WriteLine("Invalid Input !!");
                    }
                }
            }
            else if (choice == 3)
            {
                System.Console.WriteLine("Thank you for banking with us. Please visit us again.");
                break;
            }
            else
            {
                System.Console.WriteLine("Invalid Input !!");
            }
        }
    }
}