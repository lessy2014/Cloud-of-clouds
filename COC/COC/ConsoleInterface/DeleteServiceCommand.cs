using COC.Application;
using CommandLine;

namespace COC.ConsoleInterface
{
    [Verb("delete_service", HelpText = "Delete service")  ]
    public class DeleteServiceCommand: ICommand
    {
        [Value(index: 0, Required = true, HelpText = "Name of service")]
        public string ServiceName { get; set; }
        
        [Value(index:1, Required = true, HelpText = "Name of account")]
        public string AccountName { get; set; }
        public void Execute()
        {
            Account.DeleteService(ServiceName, AccountName);
        }
    }
}