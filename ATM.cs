using System.Globalization;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace ATM
{
    internal class ATM
    {
        List<Account> accounts = new List<Account>();
        public ATM()
        {
            var ac = new List<js>();

            using (StreamReader sr = new StreamReader("../../../accounts.json"))
            {
                string json = sr.ReadToEnd();
                ac = JsonSerializer.Deserialize<List<js>>(json);
            }

            foreach(var account in ac)
            {
                accounts.Add(new Account(account.username, account.email, account.password, account.money));
            }

            MainMenu();
        }

        private void MainMenu()
        {
            Console.Clear();
            Console.WriteLine("Welcome to ATM!");
            int opc;
            Console.WriteLine("1.Login / 2.Create / 3.Exit");
            opc = Convert.ToInt16(Console.ReadLine());
            switch(opc)
            {
                case 1:
                    LoginAccount();
                    break;
                case 2:
                    CreateAccount();
                    break; 
                case 3:
                    Exit();
                    break;
                default: LoginAccount();
                    break;
            } 
        }

        private void CreateAccount()
        {
            Console.Clear();

            string? username = "";
            while (!ValidateName(username))
            {
                Console.WriteLine("Create your username:");
                username = Console.ReadLine();

                if (!ValidateName(username))
                {
                    Console.Clear();
                    Console.WriteLine("Wrong username Format");
                }
            };


            Console.Clear();
            string? email = "";
            while(!IsValidEmail(email))
            {
                Console.WriteLine("Create your email:");
                email = Console.ReadLine();
                
                if(!IsValidEmail(email))
                {
                    Console.Clear();
                    Console.WriteLine("Wrong Email Format");
                }

                if(IsEmailRepeat(email))
                {
                    email = "";
                    Console.Clear();
                    Console.WriteLine("That email exist");
                }
            };

            Console.Clear();
            string? password = "";
            while (!ValidatePassword(password))
            {
                Console.WriteLine("Create your password:");
                password = Console.ReadLine();

                if (!ValidatePassword(password))
                {
                    Console.Clear();
                    Console.WriteLine("Wrong Password Format");
                }
            };

            

            accounts.Add(new Account(username, email, password));

            MainMenu();

        }

        private void LoginAccount()
        {
            Console.WriteLine("Put your email:");
            string? email;
            email = Console.ReadLine();

            Console.WriteLine("Put your password:");
            string? password;
            password = Console.ReadLine();

            foreach (var account in accounts)
            {
                if(account.GetEmail() == email && account.GetPassword() == password)
                {
                   MenuLogin(account);
                }
            }

            MainMenu();
        }

        private void MenuLogin(Account account)
        {
            Console.Clear();
            Console.WriteLine($"Welcome {account.GetUserName()}");

            int opc;
            Console.WriteLine("1.Get Money / 2.Add Money / 3.Check Money / 4.Logout");
            opc = Convert.ToInt16(Console.ReadLine());
            switch (opc)
            {
                case 1:
                    GetMoney(account);
                    break;
                case 2:
                    AddMoney(account);
                    break;
                case 3:
                    Console.WriteLine($"You have {account.GetHowManyMoney()}");
                    Console.WriteLine("Press Enter");
                    Console.ReadLine();
                    break;
                case 4:
                    MainMenu();
                    break;
                default:
                    MenuLogin(account);
                    break;
            }
            Console.Clear();
            MenuLogin(account);
        }

        private void AddMoney(Account account)
        {
            Console.WriteLine("How many money do you want to add");
            double amount = Convert.ToDouble(Console.ReadLine());
            account.SetMoney(amount , true);
            CheckMoney(account);
        }

        private void GetMoney(Account account)
        {
            Console.WriteLine("How many money do you want");
            double amount = Convert.ToDouble(Console.ReadLine());
            account.SetMoney(amount, false);
            CheckMoney(account);
        }

        private void CheckMoney(Account account)
        {
            Console.WriteLine($"You have now {account.GetHowManyMoney()}");
            Console.WriteLine("Press Enter");
            Console.ReadLine();
        }
        
        private void Exit()
        {
           foreach(var account in accounts)
            {
                Console.WriteLine(account.GetUserName());
            }
        }

        private bool IsEmailRepeat(string email)
        {
            foreach(var account in accounts)
            {
                if(account.GetEmail() == email)
                {
                    return true;
                }
            }
            return false;
        }

        private static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    string domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException e)
            {
                return false;
            }
            catch (ArgumentException e)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        private bool ValidatePassword(string passWord)
        {
            int validConditions = 0;
            foreach (char c in passWord)
            {
                if (c >= 'a' && c <= 'z')
                {
                    validConditions++;
                    break;
                }
            }
            foreach (char c in passWord)
            {
                if (c >= 'A' && c <= 'Z')
                {
                    validConditions++;
                    break;
                }
            }
            if (validConditions == 0) return false;
            foreach (char c in passWord)
            {
                if (c >= '0' && c <= '9')
                {
                    validConditions++;
                    break;
                }
            }
            if (validConditions == 1) return false;
            if (validConditions == 2)
            {
                char[] special = { '@', '#', '$', '%', '^', '&', '+', '=' }; // or whatever    
                if (passWord.IndexOfAny(special) == -1) return false;
            }
            return true;
        }

        private bool ValidateName(string name)
        {
            if(name == "" || !Regex.IsMatch(name, @"^[a-zA-Z]+$"))
            {
                return false;
            }
            
            if(name.Length <= 2)
            {
                Console.WriteLine("Minimum of 3 letters");
                return false;
            }

            return true;
        }
    }
}
