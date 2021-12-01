using System.Collections.Generic;

namespace COC.Application
{
    public class Account
    {
        public readonly string Mail;
        public readonly Dictionary<string, string> ServicesTokens;

        public Account(string mail)
        {
            Mail = mail;
            ServicesTokens = new Dictionary<string, string>();
        }
    }
}