using COC.Application;
using CommandLine;

namespace COC.ConsoleInterface
{
    [Verb("delete_account", HelpText = "Delete account")  ]
    public class DeleteAccountCommand: ICommand
    {
        [Value(index:0, Required = true, HelpText = "Name of account")]
        public string AccountName { get; set; }
        public void Execute()
        {
            Account.DeleteAccount(AccountName);
        }
    }
}