using System.Collections.Generic;

namespace COC.Application
{
    public class Account
    {
        public string Mail;
        public Dictionary<string, string> ServicesTokens;

        public Account(string mail)
        {
            Mail = mail;
            ServicesTokens = new Dictionary<string, string>();
        }
    }
}