namespace ATM
{
    internal class Account
    {
        private string _password;
        private string _username;
        private string _email;

        private double _money;

        public Account(string username, string email, string password)
        {
            _username = username; 
            _email = email;
            _password = password;
        }

        public Account(string username, string email, string password , double money)
        {
            _username = username;
            _email = email;
            _password = password;
            _money = money;
        }

        public string GetUserName()
        {
            return _username;
        }

        public string GetHowManyMoney()
        {
            return _money.ToString("C2");
        }

        public string GetEmail()
        {
            return _email;
        }

        public string GetPassword()
        {
            return _password;
        }

        public void SetMoney(double amount , bool add)
        {
            if(add) 
            {
                _money += amount;
            }
            else
            {
                if(amount <= _money) 
                { 
                    _money -= amount;
                }
            }
        }
    }
}
