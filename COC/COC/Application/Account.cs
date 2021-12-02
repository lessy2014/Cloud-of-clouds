using System.Collections.Generic;

namespace COC.Application
{
    public class Account
    {
        public readonly string AccountName;
        public readonly Dictionary<string, string> ServicesTokens;

        public Account(string accountName)
        {
            AccountName = accountName;
            ServicesTokens = new Dictionary<string, string>();
        }
    }
}