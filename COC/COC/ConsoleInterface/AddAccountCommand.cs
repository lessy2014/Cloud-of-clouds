using System;
using System.ComponentModel.Design;
using System.Windows.Input;
using COC.Application;
using CommandLine;

namespace COC.ConsoleInterface
{
    [Verb("add_account", HelpText = "Add an account of one of the available services to the application")  ]
    public class AddAccountCommand: ICommand
    {
        public bool CanExecute(object parameter)
        {
            throw new NotImplementedException();
        }
        
        [Value(index: 0, Required = true, HelpText = "Name of service")]
        public string ServiceName { get; set;}

        public void Execute(object parameter)
        {
            ServiceName = ServiceName.ToLower();
            if (ServiceName == "yandex")
            {
                var account = TokenStorage.AddToken(TokenStorage.GetToken(TokenStorage.YandexOAuth2), ServiceName,
                    ServiceName);
                DataLoader.GetFoldersFromNewAccount(account, ServiceName);
                // Yandex.YandexDataLoader.GetFolders(account, "",
                //     new DiskHttpApi(account.ServicesTokens[argument]));
            }
            else if (ServiceName.ToLower() == "dropbox")
            {
                var account = TokenStorage.AddToken(TokenStorage.GetToken(TokenStorage.DropboxOAuth2), ServiceName,
                    ServiceName.ToLower());
                DataLoader.GetFoldersFromNewAccount(account, ServiceName);
            }
            else
                Console.WriteLine("Unsupported service");
        }

        public event EventHandler CanExecuteChanged;
    }
}