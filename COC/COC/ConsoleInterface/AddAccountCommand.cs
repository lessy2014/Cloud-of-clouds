using System;
using System.ComponentModel.Design;
using System.IO;
using System.Windows.Input;
using COC.Application;
using CommandLine;

namespace COC.ConsoleInterface
{
    [Verb("add_account", HelpText = "Add an account of one of the available services to the application")  ]
    public class AddAccountCommand: ICommand
    {
        [Value(index: 0, Required = true, HelpText = "Name of service")]
        public string ServiceName { get; set; }
        
        [Value(index:1, Required = true, HelpText = "Name of account")]
        public string AccountName { get; set; }

        public void Execute()
        {
            Account.AddAccount(ServiceName, AccountName);
        }
    }
}